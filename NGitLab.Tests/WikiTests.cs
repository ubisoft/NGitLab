using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class WikiTests
{
    [Test]
    [NGitLabRetry]
    public async Task Test_wiki()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var wikiClient = context.Client.GetWikiClient(project.Id);

        // Create a page
        var page = wikiClient.Create(new WikiPageCreate
        {
            Title = "Titi/Test Title",
            Content = "Test Content",
        });

        Assert.That(page.Title, Is.EqualTo("Test Title"));
        Assert.That(page.Content, Is.EqualTo("Test Content"));
        Assert.That(page.Slug, Is.EqualTo("Titi/Test-Title"));

        // Edit the page
        page = wikiClient.Update(page.Slug, new WikiPageUpdate
        {
            Content = "Edited content",
            Title = "titi/toto/Edited Title",
        });

        Assert.That(page.Title, Is.EqualTo("Edited Title"));
        Assert.That(page.Content, Is.EqualTo("Edited content"));
        Assert.That(page.Slug, Is.EqualTo("titi/toto/Edited-Title"));

        // Get the page
        var getPage = wikiClient[page.Slug];
        Assert.That(getPage.Slug, Is.EqualTo(page.Slug));

        // Get all pages
        var pages = wikiClient.All.ToList();
        Assert.That(pages, Has.Count.EqualTo(1));
        Assert.That(pages[0].Slug, Is.EqualTo(page.Slug));

        // Delete the page
        wikiClient.Delete(page.Slug);
        pages = wikiClient.All.ToList();
        Assert.That(pages, Has.Count.EqualTo(0));
    }
}
