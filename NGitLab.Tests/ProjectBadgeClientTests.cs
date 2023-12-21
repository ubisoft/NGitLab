using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class ProjectBadgeClientTests
{
    [Test]
    [NGitLabRetry]
    public async Task Test_project_badges()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var projectBadgeClient = context.Client.GetProjectBadgeClient(project.Id);

        // Clear badges
        var badges = projectBadgeClient.ProjectsOnly.ToList();
        badges.ForEach(b => projectBadgeClient.Delete(b.Id));

        badges = projectBadgeClient.ProjectsOnly.ToList();
        Assert.That(badges, Is.Empty);

        // Create
        var badge = projectBadgeClient.Create(new BadgeCreate
        {
            ImageUrl = "http://dummy/image.png",
            LinkUrl = "http://dummy/image.html",
        });

        Assert.That(badge.Kind, Is.EqualTo(BadgeKind.Project));
        Assert.That(badge.ImageUrl, Is.EqualTo("http://dummy/image.png"));
        Assert.That(badge.LinkUrl, Is.EqualTo("http://dummy/image.html"));

        // Update
        badge = projectBadgeClient.Update(badge.Id, new BadgeUpdate
        {
            ImageUrl = "http://dummy/image_edit.png",
            LinkUrl = "http://dummy/image_edit.html",
        });

        Assert.That(badge.Kind, Is.EqualTo(BadgeKind.Project));
        Assert.That(badge.ImageUrl, Is.EqualTo("http://dummy/image_edit.png"));
        Assert.That(badge.LinkUrl, Is.EqualTo("http://dummy/image_edit.html"));

        // Delete
        projectBadgeClient.Delete(badge.Id);

        badges = projectBadgeClient.ProjectsOnly.ToList();
        Assert.That(badges, Is.Empty);

        // All
        projectBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image1.png", LinkUrl = "http://dummy/image1.html", });
        projectBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image2.png", LinkUrl = "http://dummy/image2.html", });
        projectBadgeClient.Create(new BadgeCreate { ImageUrl = "http://dummy/image3.png", LinkUrl = "http://dummy/image3.html", });
        badges = projectBadgeClient.ProjectsOnly.ToList();
        Assert.That(badges, Has.Count.EqualTo(3));
    }
}
