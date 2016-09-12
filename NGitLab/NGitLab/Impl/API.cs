namespace NGitLab.Impl
{
    public class API
    {
        private readonly IHttpRequestor _httpRequestor;

        public API(IHttpRequestor httpRequestor)
        {
            _httpRequestor = httpRequestor;
        }

        public IHttpRequestor Get() => _httpRequestor.SetMethodType(MethodType.Get);

        public IHttpRequestor Post() => _httpRequestor.SetMethodType(MethodType.Post);

        public IHttpRequestor Put() => _httpRequestor.SetMethodType(MethodType.Put);

        public IHttpRequestor Delete() => _httpRequestor.SetMethodType(MethodType.Delete);
    }
}