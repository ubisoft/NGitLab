using System;
using System.IO;
using System.Text.RegularExpressions;
using NGitLab.Impl;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class ProtectedBranchTests
    {
        [Test]
        public void DeserializeProtectedBranch_Test()
        {
            var protectedBranchJson = @"{
  ""id"": 1,
  ""name"": ""master"",
  ""push_access_levels"": [
    {
      ""access_level"": 40,
      ""user_id"": null,
      ""group_id"": null,
      ""access_level_description"": ""Maintainers""
    }
  ],
  ""merge_access_levels"": [
    {
      ""access_level"": null,
      ""user_id"": null,
      ""group_id"": 1234,
      ""access_level_description"": ""Example Merge Group""
    }
  ],
  ""allow_force_push"":false,
  ""code_owner_approval_required"": false
}";

            ProtectedBranch protectedBranch = null;
            Assert.DoesNotThrow(() => protectedBranch = SimpleJson.DeserializeObject<ProtectedBranch>(protectedBranchJson));

            Assert.NotNull(protectedBranch);
            Assert.AreEqual("master", protectedBranch.Name);
            Assert.AreEqual(AccessLevel.Maintainer, protectedBranch.PushAccessLevels[0].AccessLevel);
            Assert.AreEqual("Maintainers", protectedBranch.PushAccessLevels[0].Description);
            Assert.AreEqual(AccessLevel.NoAccess, protectedBranch.MergeAccessLevels[0].AccessLevel);
            Assert.AreEqual("Example Merge Group", protectedBranch.MergeAccessLevels[0].Description);
            Assert.False(protectedBranch.AllowForcePush);
            Assert.False(protectedBranch.CodeOwnerApprovalRequired);
        }

        [Test]
        public void DeserializeBranchProtect_Test()
        {
            var branchProtect = new BranchProtect("master")
            {
                PushAccessLevel = AccessLevel.Maintainer,
                MergeAccessLevel = AccessLevel.NoAccess,
                UnprotectAccessLevel = null,
                AllowForcePush = false,
                AllowedToPush = new AccessLevelInfo[]
                {
                    new AccessLevelInfo()
                    {
                        AccessLevel = AccessLevel.Admin,
                        Description = "Admin",
                    },
                },
                AllowedToUnprotect = new AccessLevelInfo[]
                {
                    new AccessLevelInfo()
                    {
                        AccessLevel = AccessLevel.NoAccess,
                        Description = "Example",
                    },
                },
                CodeOwnerApprovalRequired = false,
            };
#pragma warning disable MA0009 // Add regex evaluation timeout
            var expectedBranchProtectJson = Regex.Replace(
                @"{
  ""name"": ""master"",
  ""push_access_level"": 40,
  ""merge_access_level"": 0,
  ""allow_force_push"": false,
  ""allowed_to_push"": [
    {
      ""access_level"": 60,
      ""access_level_description"": ""Admin""
    }
  ],
  ""allowed_to_unprotect"": [
     {
      ""access_level"": 0,
      ""access_level_description"": ""Example""
    }
  ],
  ""code_owner_approval_required"": false
}", @"\s+", string.Empty);
#pragma warning restore MA0009 // Add regex evaluation timeout

            string branchProtectJson = null;
            Assert.DoesNotThrow(() => branchProtectJson = SimpleJson.SerializeObject(branchProtect));

            Assert.AreEqual(expectedBranchProtectJson, branchProtectJson);
        }
    }
}
