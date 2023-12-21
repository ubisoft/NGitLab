using NGitLab.Impl;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests;

public class GitLabChangeDiffCounterTests
{
    [Test]
    public void Compute_return_diffs_stats()
    {
        var mrChange = new MergeRequestChange
        {
            Changes = new[]
            {
                new Change
                {
                    Diff =
                        "@@ -1,4 +1,6 @@\n- using System.Threading.Tasks;\n+﻿using System;\n+using System.Threading.Tasks;\n+using NGitLab.Models;\n using NGitLab.Tests.Docker;\n using NUnit.Framework;\n \n@@ -16,5 +18,32 @@ namespace NGitLab.Tests\n             Assert.IsNotNull(commit.Message);\n             Assert.IsNotNull(commit.ShortId);\n         }\n+\n+        [Test][NGitLabRetry]\n+        public async Task Test_can_get_stats_in_commit()\n+        {\n+            using var context = await GitLabTestContext.CreateAsync();\n+            var project = context.CreateProject(initializeWithCommits: true);\n+            context.Client.GetRepository(project.Id).Files.Create(new FileUpsert\n+            {\n+                Branch = \"master\",\n+                CommitMessage = \"file to be updated\",\n+                Path = \"CommitStats.txt\",\n+                RawContent = \"I'm defective and i need to be fixeddddddddddddddd\",\n+            });\n+\n+            context.Client.GetRepository(project.Id).Files.Update(new FileUpsert\n+            {\n+                Branch = \"master\",\n+                CommitMessage = \"fixing the file\",\n+                Path = \"CommitStats.txt\",\n+                RawContent = \"I'm no longer defective and i have been fixed\\n\\n\\r\\n\\r\\rEnjoy\",\n+            });\n+\n+            var commit = context.Client.GetCommits(project.Id).GetCommit(\"master\");\n+            Assert.AreEqual(4, commit.Stats.Additions);\n+            Assert.AreEqual(1, commit.Stats.Deletions);\n+            Assert.AreEqual(5, commit.Stats.Total);\n+        }\n     }\n }\n",
                },
                new Change
                {
                    Diff =
                        "@@ -46,5 +46,8 @@ namespace NGitLab.Models\n \n         [DataMember(Name = \"status\")]\n         public string Status;\n+\n+        [DataMember(Name = \"stats\")]\n+        public CommitStats Stats;\n     }\n-}\n\\ No newline at end of file\n+}\n",
                },
                new Change
                {
                    Diff =
                        "@@ -0,0 +1,18 @@\n+using System;\n+using System.Runtime.Serialization;\n+\n+namespace NGitLab.Models\n+{\n+    [DataContract]\n+    public class CommitStats\n+    {\n+        [DataMember(Name = \"additions\")]\n+        public int Additions;\n+\n+        [DataMember(Name = \"deletions\")]\n+        public int Deletions;\n+\n+        [DataMember(Name = \"total\")]\n+        public int Total;\n+    }\n+}\n",
                },
                new Change
                {
                    Diff =
                        "@@ -30,6 +30,7 @@ Get it from NuGet. You can simply install it with the Package Manager console:\n \n - Install Docker on your machine\n - It's recommended to use WSL version 2: https://docs.microsoft.com/en-us/windows/wsl/install-win10\n+- Executing tests under Linux requires Powershell to be installed. (https://docs.microsoft.com/en-us/powershell/scripting/install/installing-powershell-core-on-linux)\n \n ## Thanks\n \n",
                },
            },
        };
        var unitUnderTest = new GitLabChangeDiffCounter();
        var diffStats = unitUnderTest.Compute(mrChange);
        Assert.That(diffStats.AddedLines, Is.EqualTo(53));
        Assert.That(diffStats.DeletedLines, Is.EqualTo(2));
    }
}
