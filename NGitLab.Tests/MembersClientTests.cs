using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;
#nullable enable

namespace NGitLab.Tests
{
    public class MembersClientTests
    {
        [Test]
        [NGitLabRetry]
        public async Task AddMemberToProject()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            context.CreateNewUser(out var user);
            var projectId = project.Id.ToString(CultureInfo.InvariantCulture);

            var expiresAt = DateTimeOffset.UtcNow.AddDays(30).ToString("yyyy-MM-dd");
            context.Client.Members.AddMemberToProject(projectId, new ProjectMemberCreate
            {
                AccessLevel = AccessLevel.Developer,
                UserId = user.Id.ToString(CultureInfo.InvariantCulture),
                ExpiresAt = expiresAt,
            });

            var projectUser = context.Client.Members.OfProject(projectId).Single(u => u.Id == user.Id);
            Assert.AreEqual(AccessLevel.Developer, (AccessLevel)projectUser.AccessLevel);
            Assert.AreEqual(expiresAt, projectUser.ExpiresAt?.ToString("yyyy-MM-dd"));
        }

        [Test]
        [NGitLabRetry]
        public async Task UpsertAccessLevelMemberOfProject()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            context.CreateNewUser(out var user);
            var projectId = project.Id.ToString(CultureInfo.InvariantCulture);

            // Add
            context.Client.Members.AddMemberToProject(projectId, new ProjectMemberCreate
            {
                AccessLevel = AccessLevel.Developer,
                UserId = user.Id.ToString(CultureInfo.InvariantCulture),
            });
            var projectUser = context.Client.Members.OfProject(projectId).Single(u => u.Id == user.Id);
            Assert.AreEqual(AccessLevel.Developer, (AccessLevel)projectUser.AccessLevel);

            // Update
            context.Client.Members.UpdateMemberOfProject(projectId, new ProjectMemberUpdate
            {
                AccessLevel = AccessLevel.Maintainer,
                UserId = user.Id.ToString(CultureInfo.InvariantCulture),
            });
            projectUser = context.Client.Members.OfProject(projectId).Single(u => u.Id == user.Id);
            Assert.AreEqual(AccessLevel.Maintainer, (AccessLevel)projectUser.AccessLevel);
        }

        [Test]
        [NGitLabRetry]
        public async Task GetAccessLevelMemberOfProject()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            context.CreateNewUser(out var user);
            var projectId = project.Id.ToString(CultureInfo.InvariantCulture);

            context.Client.Members.AddMemberToProject(projectId, new ProjectMemberCreate
            {
                AccessLevel = AccessLevel.Developer,
                UserId = user.Id.ToString(CultureInfo.InvariantCulture),
            });

            // Get
            var projectUser = context.Client.Members.GetMemberOfProject(projectId, user.Id.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(AccessLevel.Developer, (AccessLevel)projectUser.AccessLevel);
        }

        [Test]
        [NGitLabRetry]
        public async Task AddMemberToGroup()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var @group = context.CreateGroup();
            context.CreateNewUser(out var user);
            var groupId = @group.Id.ToString(CultureInfo.InvariantCulture);

            var expiresAt = DateTimeOffset.UtcNow.AddDays(30).ToString("yyyy-MM-dd");
            context.Client.Members.AddMemberToGroup(groupId, new GroupMemberCreate
            {
                AccessLevel = AccessLevel.Developer,
                UserId = user.Id.ToString(CultureInfo.InvariantCulture),
                ExpiresAt = expiresAt,
            });

            var groupUser = context.Client.Members.OfGroup(groupId).Single(u => u.Id == user.Id);
            Assert.AreEqual(AccessLevel.Developer, (AccessLevel)groupUser.AccessLevel);
            Assert.AreEqual(expiresAt, groupUser.ExpiresAt?.ToString("yyyy-MM-dd"));
        }

        [Test]
        [NGitLabRetry]
        public async Task UpsertAccessLevelMemberOfGroup()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var @group = context.CreateGroup();
            context.CreateNewUser(out var user);
            var groupId = @group.Id.ToString(CultureInfo.InvariantCulture);

            // Add
            context.Client.Members.AddMemberToGroup(groupId, new GroupMemberCreate
            {
                AccessLevel = AccessLevel.Developer,
                UserId = user.Id.ToString(CultureInfo.InvariantCulture),
            });
            var groupUser = context.Client.Members.OfGroup(groupId).Single(u => u.Id == user.Id);
            Assert.AreEqual(AccessLevel.Developer, (AccessLevel)groupUser.AccessLevel);

            // Update
            context.Client.Members.UpdateMemberOfGroup(groupId, new GroupMemberUpdate
            {
                AccessLevel = AccessLevel.Maintainer,
                UserId = user.Id.ToString(CultureInfo.InvariantCulture),
            });
            groupUser = context.Client.Members.OfGroup(groupId).Single(u => u.Id == user.Id);
            Assert.AreEqual(AccessLevel.Maintainer, (AccessLevel)groupUser.AccessLevel);
        }

        [Test]
        [NGitLabRetry]
        public async Task GetAccessLevelMemberOfGroup()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var @group = context.CreateGroup();
            context.CreateNewUser(out var user);
            var groupId = @group.Id.ToString(CultureInfo.InvariantCulture);

            context.Client.Members.AddMemberToGroup(groupId, new GroupMemberCreate
            {
                AccessLevel = AccessLevel.Developer,
                UserId = user.Id.ToString(CultureInfo.InvariantCulture),
            });

            // Get
            var groupUser = context.Client.Members.GetMemberOfGroup(groupId, user.Id.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(AccessLevel.Developer, (AccessLevel)groupUser.AccessLevel);
        }
    }
}
