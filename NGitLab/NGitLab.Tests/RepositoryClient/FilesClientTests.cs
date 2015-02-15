namespace NGitLab.Tests.RepositoryClient
{
    public class FilesClientTests
    {
        private readonly IFilesClient _client;

        public FilesClientTests()
        {
            _client = _RepositoryClientTests.RepositoryClient.Files;
        }
    }
}