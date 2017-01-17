using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class TagsTests
    {
        [Test]
        public void Test_can_tag_a_project()
        {
            var result = Tags.Create(new TagCreate
            {
                Name = "v0.5",
                Ref = "master",
                ReleaseDescription = "Test description"
            });

            Assert.IsNotNull(result);
            Assert.IsNotNull(Tags.All.FirstOrDefault(x => x.Name == "v0.5"));

            Tags.Delete("v0.5");
            Assert.IsNull(Tags.All.FirstOrDefault(x => x.Name == "v0.5"));
        }

        private ITagClient Tags
        {
            get
            {
                Assert.IsNotNull(Initialize.UnitTestProject);
                return Initialize.GitLabClient.GetRepository(Initialize.UnitTestProject.Id).Tags;
            }
        }
    }
}