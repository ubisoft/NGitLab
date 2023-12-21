using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class GroupBadgeClientTests
{
    [Test]
    [NGitLabRetry]
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

        Assert.That(badge.Kind, Is.EqualTo(BadgeKind.Group));
        Assert.That(badge.ImageUrl, Is.EqualTo("http://dummy/image.png"));
        Assert.That(badge.LinkUrl, Is.EqualTo("http://dummy/image.html"));

        // Update
        badge = groupBadgeClient.Update(badge.Id, new BadgeUpdate
        {
            ImageUrl = "http://dummy/image_edit.png",
            LinkUrl = "http://dummy/image_edit.html",
        });

        Assert.That(badge.Kind, Is.EqualTo(BadgeKind.Group));
        Assert.That(badge.ImageUrl, Is.EqualTo("http://dummy/image_edit.png"));
        Assert.That(badge.LinkUrl, Is.EqualTo("http://dummy/image_edit.html"));

        // Delete
        groupBadgeClient.Delete(badge.Id);

        var badges = groupBadgeClient.All.ToList();
        Assert.That(badges, Is.Empty);

        // All
        groupBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image1.png", LinkUrl = "http://dummy/image1.html", });
        groupBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image2.png", LinkUrl = "http://dummy/image2.html", });
        groupBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image3.png", LinkUrl = "http://dummy/image3.html", });
        badges = groupBadgeClient.All.ToList();
        Assert.That(badges, Has.Count.EqualTo(3));
    }
}
