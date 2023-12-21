using System;
using System.Net;
using System.Runtime.Serialization;

namespace NGitLab.Mock.Clients;

public sealed class GitLabNotFoundException : GitLabException
{
    public GitLabNotFoundException()
    {
        Initialize();
    }

    public GitLabNotFoundException(string message)
        : base(message)
    {
        Initialize();
    }

    public GitLabNotFoundException(string message, Exception inner)
        : base(message, inner)
    {
        Initialize();
    }

    private GitLabNotFoundException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        Initialize();
    }

    private void Initialize()
    {
        StatusCode = HttpStatusCode.NotFound;
    }
}
