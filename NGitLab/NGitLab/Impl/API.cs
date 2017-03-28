using NGitLab.Models;

namespace NGitLab.Impl
{
    /// <summary>
    /// View api documentation at: https://docs.gitlab.com/ce/api/README.html
    /// </summary>
    public class API
    {
        private readonly GitLabCredentials _credentials;

        public string ConnectionToken { get; set; }

        public API(GitLabCredentials credentials)
        {
            _credentials = credentials;
        }

        public IHttpRequestor Get() => CreateRequestor(MethodType.Get);

        public IHttpRequestor Post() => CreateRequestor(MethodType.Post);

        public IHttpRequestor Put() => CreateRequestor(MethodType.Put);

        public IHttpRequestor Delete() => CreateRequestor(MethodType.Delete);

        protected virtual IHttpRequestor CreateRequestor(MethodType methodType)
        {
            if (_credentials.ApiToken == null)
            {
                _credentials.ApiToken = OpenPrivateSession();
            }
            
            return new HttpRequestor(_credentials.HostUrl, _credentials.ApiToken, methodType);
        }

        private string OpenPrivateSession()
        {
            var httpRequestor = new HttpRequestor(_credentials.HostUrl, "", MethodType.Post);
            var url = $"/session?login={System.Web.HttpUtility.UrlEncode(_credentials.UserName)}&password={System.Web.HttpUtility.UrlEncode(_credentials.Password)}";
            var session = httpRequestor.To<Session>(url);

            return session.PrivateToken;
        }
    }
}