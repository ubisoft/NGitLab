namespace NGitLab.Tests {
    public static class Config {
        public const string ServiceUrl = "http://gitserver";
        public const string Secret = "kpdcucE1Y4wykmqBGD4x";

        public static GitLabClient Connect() {
            return GitLabClient.Connect(ServiceUrl, Secret);
        }
    }
}