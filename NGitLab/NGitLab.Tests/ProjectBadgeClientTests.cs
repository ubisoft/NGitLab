using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class ProjectBadgeClientTests
    {
        private IProjectBadgeClient _projectBadgeClient;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            _projectBadgeClient = Initialize.GitLabClient.GetProjectBadgeClient(Initialize.UnitTestProject.Id);
        }

        [Test]
        public void Test_project_badges()
        {
            // Create
            var badge = _projectBadgeClient.Create(new BadgeCreate
            {
                ImageUrl = "http://dummy/image.png",
                LinkUrl = "http://dummy/image.html",
            });

            Assert.AreEqual(BadgeKind.Project, badge.Kind);
            Assert.AreEqual("http://dummy/image.png", badge.ImageUrl);
            Assert.AreEqual("http://dummy/image.html", badge.LinkUrl);

            // Update
            badge = _projectBadgeClient.Update(badge.Id, new BadgeUpdate
            {
                ImageUrl = "http://dummy/image_edit.png",
                LinkUrl = "http://dummy/image_edit.html",
            });

            Assert.AreEqual(BadgeKind.Project, badge.Kind);
            Assert.AreEqual("http://dummy/image_edit.png", badge.ImageUrl);
            Assert.AreEqual("http://dummy/image_edit.html", badge.LinkUrl);

            // Delete
            _projectBadgeClient.Delete(badge.Id);

            var badges = _projectBadgeClient.All.ToList();
            Assert.IsEmpty(badges);

            // All
            _projectBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image1.png", LinkUrl = "http://dummy/image1.html", });
            _projectBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image2.png", LinkUrl = "http://dummy/image2.html", });
            _projectBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image3.png", LinkUrl = "http://dummy/image3.html", });
            badges = _projectBadgeClient.All.ToList();
            Assert.AreEqual(3, badges.Count);
        }
    }

    public class GroupBadgeClientTests
    {
        private IGroupBadgeClient _groupBadgeClient;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            _groupBadgeClient = Initialize.GitLabClient.GetGroupBadgeClient(Initialize.UnitTestGroup.Id);
        }

        [Test]
        public void Test_group_badges()
        {
            // Create
            var badge = _groupBadgeClient.Create(new BadgeCreate
            {
                ImageUrl = "http://dummy/image.png",
                LinkUrl = "http://dummy/image.html",
            });

            Assert.AreEqual(BadgeKind.Group, badge.Kind);
            Assert.AreEqual("http://dummy/image.png", badge.ImageUrl);
            Assert.AreEqual("http://dummy/image.html", badge.LinkUrl);

            // Update
            badge = _groupBadgeClient.Update(badge.Id, new BadgeUpdate
            {
                ImageUrl = "http://dummy/image_edit.png",
                LinkUrl = "http://dummy/image_edit.html",
            });

            Assert.AreEqual(BadgeKind.Group, badge.Kind);
            Assert.AreEqual("http://dummy/image_edit.png", badge.ImageUrl);
            Assert.AreEqual("http://dummy/image_edit.html", badge.LinkUrl);

            // Delete
            _groupBadgeClient.Delete(badge.Id);

            var badges = _groupBadgeClient.All.ToList();
            Assert.IsEmpty(badges);

            // All
            _groupBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image1.png", LinkUrl = "http://dummy/image1.html", });
            _groupBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image2.png", LinkUrl = "http://dummy/image2.html", });
            _groupBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image3.png", LinkUrl = "http://dummy/image3.html", });
            badges = _groupBadgeClient.All.ToList();
            Assert.AreEqual(3, badges.Count);
        }
    }
}
