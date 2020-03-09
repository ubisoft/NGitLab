using System.Linq;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class WikiTests
    {
        private static IWikiClient WikiClient => Initialize.GitLabClient.GetWikiClient(Initialize.UnitTestProject.Id);

        [Test]
        public void Test_wiki()
        {
            // Create a page
            var page = WikiClient.Create(new Models.WikiPageCreate
            {
                Title = "Titi/Test Title",
                Content = "Test Content",
            });

            Assert.That(page.Title, Is.EqualTo("Test Title"));
            Assert.That(page.Content, Is.EqualTo("Test Content"));
            Assert.That(page.Slug, Is.EqualTo("Titi/Test-Title"));

            // Edit the page
            page = WikiClient.Update(page.Slug, new Models.WikiPageUpdate
            {
                Content = "Edited content",
                Title = "titi/toto/Edited Title",
            });

            Assert.That(page.Title, Is.EqualTo("Edited Title"));
            Assert.That(page.Content, Is.EqualTo("Edited content"));
            Assert.That(page.Slug, Is.EqualTo("titi/toto/Edited-Title"));

            // Get the page
            var getPage = WikiClient[page.Slug];
            Assert.That(getPage.Slug, Is.EqualTo(page.Slug));

            // Get all pages
            var pages = WikiClient.All.ToList();
            Assert.That(pages, Has.Count.EqualTo(1));
            Assert.That(pages[0].Slug, Is.EqualTo(page.Slug));

            // Delete the page
            WikiClient.Delete(page.Slug);
            pages = WikiClient.All.ToList();
            Assert.That(pages, Has.Count.EqualTo(0));
        }
    }
}
