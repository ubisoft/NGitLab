using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class MembersClientTests
    {
        [Test]
        public async Task UpsertAccessLevelMemberOfProject()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            context.CreateNewUserAsync(out var user);
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
    }
}
