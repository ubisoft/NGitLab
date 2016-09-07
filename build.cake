#tool "nuget:?package=NUnit.ConsoleRunner"
#tool "nuget:?package=GitReleaseNotes"
#tool "nuget:?package=GitVersion.CommandLine&prerelease"

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

string version = null;
string nugetVersion = null;
string preReleaseTag = null;
string semVersion = null;

Task("Version")
    .Does(() =>
{
    GitVersion(new GitVersionSettings
    {
        UpdateAssemblyInfo = AppVeyor.IsRunningOnAppVeyor,
        LogFilePath = "console",
        OutputType = GitVersionOutput.BuildServer
    });

    GitVersion assertedVersions = GitVersion(new GitVersionSettings
    {
        OutputType = GitVersionOutput.Json
    });

    Information("New Version:" + assertedVersions.NuGetVersion);

    version = assertedVersions.MajorMinorPatch;
    nugetVersion = assertedVersions.NuGetVersion;
    preReleaseTag = assertedVersions.PreReleaseTag;
    semVersion = assertedVersions.LegacySemVerPadded;
});

Task("NuGet-Package-Restore")
    .Does(() =>
{
    NuGetRestore("./NGitLab/NGitLab.sln");
});

Task("Build")
    .IsDependentOn("Version")
    .IsDependentOn("NuGet-Package-Restore")
    .Does(() =>
{
  var msBuildSettings = new MSBuildSettings()
           .SetConfiguration(configuration)
           .SetPlatformTarget(PlatformTarget.MSIL)
           .WithProperty("Windows", "True")
           .UseToolVersion(MSBuildToolVersion.VS2015)
           .SetVerbosity(Verbosity.Minimal)
           .SetNodeReuse(false);

       if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
       {
           msBuildSettings = msBuildSettings
               .WithProperty("GitVersion_NuGetVersion", nugetVersion)
               .WithProperty("GitVersion_SemVer", semVersion)
               .WithProperty("GitVersion_MajorMinorPatch", version)
               .WithProperty("GitVersion_PreReleaseTag", preReleaseTag);
       }
       MSBuild("./NGitLab/NGitLab.sln", msBuildSettings);
});

Task("Run-NUnit-Tests")
    .IsDependentOn("Build")
    .Does(() =>
{
    var settings = new NUnit3Settings
    {
        Where = "cat != Server_Required"
    };

    NUnit3(new [] {
        "NGitLab/NGitLab.Tests/bin/" + configuration + "/NGitLab.Tests.dll" },
        settings);

    if (AppVeyor.IsRunningOnAppVeyor)
    {
        Information("Uploading test results");
        AppVeyor.UploadTestResults("TestResult.xml", AppVeyorTestResultsType.NUnit3);
    }
});

Task("Check-Build-Folder-Exists")
  .IsDependentOn("Run-NUnit-Tests")
  .Does(() =>
  {
    EnsureDirectoryExists("./build");
  });

Task("Create-Release-Notes")
    .IsDependentOn("Check-Build-Folder-Exists")
    .Does(() =>
{
    var releaseNotesExitCode = StartProcess(@"tools\GitReleaseNotes\tools\gitreleasenotes.exe",
     new ProcessSettings
      {
        Arguments = ". /o ./build/releasenotes.md"
      });

      if (string.IsNullOrEmpty(System.IO.File.ReadAllText("./build/releasenotes.md")))
          System.IO.File.WriteAllText("./build/releasenotes.md", "No issues closed since last release");

    if (releaseNotesExitCode != 0) throw new Exception("Failed to generate release notes");
});

Task("Create-NuGet-Packages")
  .IsDependentOn("Create-Release-Notes")
  .Does(() =>
{
  NuGetPack("./NGitLab.nuspec", new NuGetPackSettings {
        Version = semVersion,
        OutputDirectory = "./build",
        Symbols = false,
        NoPackageAnalysis = true
    });
});

Task("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Create-NuGet-Packages")
    .WithCriteria(() => AppVeyor.IsRunningOnAppVeyor)
    .Does(() =>
{
  AppVeyor.UploadArtifact("build/releasenotes.md");
  AppVeyor.UploadArtifact("build/NGitLab." + nugetVersion +".nupkg");
});

Task("Default")
  .IsDependentOn("Upload-AppVeyor-Artifacts");

RunTarget(target);
