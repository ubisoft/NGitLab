using System;
using System.Net.Http;

namespace NGitLab;

public sealed class GitLabRequestResult
{
    public HttpRequestMessage Request { get; set; }

    public HttpResponseMessage   Response { get; set; }

    public Exception Exception { get; set; }
}
