#tool "nuget:?package=NUnit.ConsoleRunner"
#tool "nuget:?package=GitReleaseNotes"
#tool "nuget:?package=GitVersion.CommandLine&prerelease"
#tool "nuget:?package=OpenCover"
#tool coveralls.net
#tool coveralls.io

#addin Cake.Coveralls

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
           .SetNodeReuse(false)
           .WithTarget("Rebuild");

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

Task("Check-Build-Folder-Exists")
  .IsDependentOn("Build")
  .Does(() =>
  {
    EnsureDirectoryExists("./build");
  });

Task("Run-NUnit-Tests")
    .IsDependentOn("Check-Build-Folder-Exists")
    .WithCriteria(() => !AppVeyor.IsRunningOnAppVeyor) // Skip tests because no GitLab Server is available at the moment.
    .Does(() =>
    {
    var settings = new NUnit3Settings
    {
        Where = "cat != Server_Required",
        ShadowCopy = false
    };

  	var testAssemblies = GetFiles("./**/bin/" + configuration + "/*.Tests.dll");

    NUnit3(testAssemblies, settings);

    OpenCover(tool => {
      tool.NUnit3(testAssemblies, settings);
      },
      new FilePath("build/coverage.xml"),
      new OpenCoverSettings()
        .WithFilter("+[NGitLab]*")
        .WithFilter("-[NGitLab.Tests]*"));

    if (AppVeyor.IsRunningOnAppVeyor)
    {
        Information("Uploading test results");
        AppVeyor.UploadTestResults("TestResult.xml", AppVeyorTestResultsType.NUnit3);
    }
});

Task("Create-Release-Notes")
    .IsDependentOn("Run-NUnit-Tests")
    .Does(() =>
{
    var releaseNotesExitCode = StartProcess(@"tools\GitReleaseNotes\tools\gitreleasenotes.exe",
     new ProcessSettings
      {
        Arguments = ". /o ./build/releasenotes.md /allTags"
      });

      if(!System.IO.File.Exists("./build/releasenotes.md"))
      {
          Information("Did not create release notes!!");
      }

      //if (string.IsNullOrEmpty(System.IO.File.ReadAllText("./build/releasenotes.md")))
          //System.IO.File.WriteAllText("./build/releasenotes.md", "No issues closed since last release");

      //if (releaseNotesExitCode != 0) throw new Exception("Failed to generate release notes");
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

Task("Upload-Coverage-Report")
  .IsDependentOn("Create-NuGet-Packages")
  .WithCriteria(() => AppVeyor.IsRunningOnAppVeyor)
  .Does(() =>
  {
    var coverallsToken = Argument<string>("coverallsToken");
    CoverallsIo("./build/coverage.xml", new CoverallsIoSettings()
    {
        RepoToken = coverallsToken
    });
  });

Task("Upload-AppVeyor-Artifacts")
    .IsDependentOn("Upload-Coverage-Report")
    .WithCriteria(() => AppVeyor.IsRunningOnAppVeyor)
    .Does(() =>
{
  AppVeyor.UploadArtifact("./build/coverage.xml");
  AppVeyor.UploadArtifact("./build/releasenotes.md");
  AppVeyor.UploadArtifact("./build/NGitLab." + nugetVersion +".nupkg");
});

Task("Default")
  .IsDependentOn("Upload-AppVeyor-Artifacts");

RunTarget(target);
