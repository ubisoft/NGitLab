using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class ProjectBadgeClientTests
    {
        [Test]
        public async Task Test_project_badges()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var projectBadgeClient = context.Client.GetProjectBadgeClient(project.Id);

            // Clear badges
            var badges = projectBadgeClient.ProjectsOnly.ToList();
            badges.ForEach(b => projectBadgeClient.Delete(b.Id));

            badges = projectBadgeClient.ProjectsOnly.ToList();
            Assert.IsEmpty(badges);

            // Create
            var badge = projectBadgeClient.Create(new BadgeCreate
            {
                ImageUrl = "http://dummy/image.png",
                LinkUrl = "http://dummy/image.html",
            });

            Assert.AreEqual(BadgeKind.Project, badge.Kind);
            Assert.AreEqual("http://dummy/image.png", badge.ImageUrl);
            Assert.AreEqual("http://dummy/image.html", badge.LinkUrl);

            // Update
            badge = projectBadgeClient.Update(badge.Id, new BadgeUpdate
            {
                ImageUrl = "http://dummy/image_edit.png",
                LinkUrl = "http://dummy/image_edit.html",
            });

            Assert.AreEqual(BadgeKind.Project, badge.Kind);
            Assert.AreEqual("http://dummy/image_edit.png", badge.ImageUrl);
            Assert.AreEqual("http://dummy/image_edit.html", badge.LinkUrl);

            // Delete
            projectBadgeClient.Delete(badge.Id);

            badges = projectBadgeClient.ProjectsOnly.ToList();
            Assert.IsEmpty(badges);

            // All
            projectBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image1.png", LinkUrl = "http://dummy/image1.html", });
            projectBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image2.png", LinkUrl = "http://dummy/image2.html", });
            projectBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image3.png", LinkUrl = "http://dummy/image3.html", });
            badges = projectBadgeClient.ProjectsOnly.ToList();
            Assert.AreEqual(3, badges.Count);
        }
    }
}
