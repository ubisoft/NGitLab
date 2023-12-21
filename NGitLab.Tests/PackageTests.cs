using System.IO;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class PackageTests
    {
        [Test]
        [NGitLabRetry]
        public async Task Test_publish_package()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var packagesClient = context.Client.Packages;

            var packagePublish = new PackagePublish
            {
                FileName = "README.md",
                PackageName = "Packages",
                PackageVersion = "1.0.0",
                Status = "default",
                PackageStream = File.OpenRead("../../../../README.md"),
            };

            var newGenericPackage = await packagesClient.PublishAsync(project.Id, packagePublish);

            var packageQuery = new PackageQuery { PackageType = PackageType.generic };
            var genericPackages = packagesClient.Get(project.Id, packageQuery).ToList();
            var singleGenericPackage = await packagesClient.GetByIdAsync(project.Id, newGenericPackage.PackageId);

            Assert.AreEqual(1, genericPackages.Count);
            Assert.AreEqual(newGenericPackage.PackageId, genericPackages[0].PackageId);
            Assert.AreEqual(singleGenericPackage.PackageId, newGenericPackage.PackageId);
        }
    }
}
