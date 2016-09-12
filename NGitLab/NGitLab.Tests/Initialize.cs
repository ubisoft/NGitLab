using NUnit.Framework;
using System.IO;
using System.Reflection;

namespace NGitLab.Tests
{
    [SetUpFixture]
    public class Initialize
    {
        string[] files =
            {
                "users",
                "user",
                "projects",
                "hooks",
                "branches",
                "branch_master",
                "merge_requests_comments",
                "merge_requests_comment",
                "commits",
                "merge_requests",
                "merge_requests_opened",
                "commit_sha",
                "commit_diff",
                "merge_request",
            };

        [OneTimeSetUp]
        public void Unpack()
        {
            Assembly a = Assembly.GetExecutingAssembly();
            foreach (var file in files)
            {
                var fileName = $"NGitLab.Tests.{file}.json";
                using (Stream s = a.GetManifestResourceStream(fileName))
                {
                    using (var sr = new StreamReader(s))
                    {
                        using (StreamWriter sw = File.CreateText(Path.Combine(Path.GetTempPath(), fileName)))
                        {
                            sw.Write(sr.ReadToEnd());
                            sw.Flush();
                        }
                    }
                }
            }
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            foreach (var file in files)
            {
                var path = Path.Combine(Path.GetTempPath(), file);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
        }
    }
}
