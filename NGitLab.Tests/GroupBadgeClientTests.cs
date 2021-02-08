using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class GroupBadgeClientTests
    {
        [Test]
        public async Task Test_group_badges()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var group = context.CreateGroup();
            var groupBadgeClient = context.Client.GetGroupBadgeClient(group.Id);

            // Create
            var badge = groupBadgeClient.Create(new BadgeCreate
            {
                ImageUrl = "http://dummy/image.png",
                LinkUrl = "http://dummy/image.html",
            });

            Assert.AreEqual(BadgeKind.Group, badge.Kind);
            Assert.AreEqual("http://dummy/image.png", badge.ImageUrl);
            Assert.AreEqual("http://dummy/image.html", badge.LinkUrl);

            // Update
            badge = groupBadgeClient.Update(badge.Id, new BadgeUpdate
            {
                ImageUrl = "http://dummy/image_edit.png",
                LinkUrl = "http://dummy/image_edit.html",
            });

            Assert.AreEqual(BadgeKind.Group, badge.Kind);
            Assert.AreEqual("http://dummy/image_edit.png", badge.ImageUrl);
            Assert.AreEqual("http://dummy/image_edit.html", badge.LinkUrl);

            // Delete
            groupBadgeClient.Delete(badge.Id);

            var badges = groupBadgeClient.All.ToList();
            Assert.IsEmpty(badges);

            // All
            groupBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image1.png", LinkUrl = "http://dummy/image1.html", });
            groupBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image2.png", LinkUrl = "http://dummy/image2.html", });
            groupBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image3.png", LinkUrl = "http://dummy/image3.html", });
            badges = groupBadgeClient.All.ToList();
            Assert.AreEqual(3, badges.Count);
        }
    }
}
