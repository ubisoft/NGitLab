namespace NGitLab.Tests
{
    public static class Config
    {
        public const string ServiceUrl = "https://gitlab.com/api/v3";
        public const string Secret = "TOKEN";

        public static GitLabClient Connect()
        {
            return GitLabClient.Connect(ServiceUrl, Secret);
        }
    }
}
