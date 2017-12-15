namespace NGitLab.Tests {
    public static class Config {
        public const string ServiceUrl = "https://gitclub.cn";
        public const string Secret = "y1ZcmHSidM4bqwYzjFPU";

        public static GitLabClient Connect() {
            return GitLabClient.Connect(ServiceUrl,"maikebing","285220Myh", Impl.Api.ApiVersion.V4_Oauth);
        }
    }
}