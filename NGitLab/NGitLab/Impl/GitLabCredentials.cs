using System;

namespace NGitLab.Impl
{
    /// <summary>
    /// To connect to to GitLab, you need a token,
    /// you can either fix one in your profile or ask for one
    /// on demand.
    /// </summary>
    /// <remarks>The credentials will try to resolve the api token on demand only to make sure not cost is
    /// incurred until the credentials are really used.</remarks>
    public class GitLabCredentials
    {
        private string _apiToken;

        public string HostUrl { get; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public GitLabCredentials(string hostUrl, string apiToken)
        {
            if (string.IsNullOrEmpty(hostUrl)) throw new ArgumentException("HostUrl is mandatory", nameof(hostUrl));
            if (string.IsNullOrEmpty(apiToken)) throw new ArgumentException("Token is mandatory", nameof(apiToken));

            ValidateHostUrl(hostUrl);

            HostUrl = GetApiUrl(hostUrl);
            _apiToken = apiToken;
        }

        public GitLabCredentials(string hostUrl, string userName, string password)
        {
            if (string.IsNullOrEmpty(hostUrl)) throw new ArgumentException("HostUrl is mandatory", nameof(hostUrl));
            if (string.IsNullOrEmpty(userName)) throw new ArgumentException("UserName is mandatory", nameof(userName));
            if (string.IsNullOrEmpty(password)) throw new ArgumentException("Password is mandatory", nameof(password));

            ValidateHostUrl(hostUrl);

            HostUrl = GetApiUrl(hostUrl);
            UserName = userName;
            Password = password;
        }

        public string ApiToken
        {
            get
            {
                return _apiToken;
            }
            set
            {
                if (value == null) throw new ArgumentException();

                _apiToken = value;

                // Passwords and user names and not useful anymore.
                UserName = null;
                Password = null;
            }
        }

        private void ValidateHostUrl(string url)
        {
            if (url.EndsWith("/api/v3", StringComparison.OrdinalIgnoreCase) ||
                url.EndsWith("/api/v3/", StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("API v3 endpoint is not supported.");
        }

        private string GetApiUrl(string url)
        {
            url = url.TrimEnd('/');
            if (url.EndsWith("/api/v4", StringComparison.OrdinalIgnoreCase))
                return url;

            return url + "/api/v4";
        }
    }
}