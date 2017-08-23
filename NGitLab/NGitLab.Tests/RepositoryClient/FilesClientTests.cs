namespace NGitLab.Tests.RepositoryClient {
    public class FilesClientTests {
        readonly IFilesClient client;

        public FilesClientTests() {
            client = _RepositoryClientTests.RepositoryClient.Files;
        }
    }
}