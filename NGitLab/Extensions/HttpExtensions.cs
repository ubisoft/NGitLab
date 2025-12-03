using System.IO;
using System.Net.Http;

namespace NGitLab.Extensions;

public static class HttpExtensions
{
    public static Stream GetRequestStream(this HttpRequestMessage request)
    {
#if NET472 || NETSTANDARD2_0
        return request.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
#else
        return request.Content.ReadAsStream();
#endif
    }

    public static Stream GetResponseStream(this HttpResponseMessage response)
    {
#if NET472 || NETSTANDARD2_0
        return  response.Content.ReadAsStreamAsync().GetAwaiter().GetResult();
#else
        return response.Content.ReadAsStream();
#endif
    }
}
