using System;
using System.Net;
using System.Runtime.Serialization;

namespace NGitLab.Mock.Clients;

public sealed class GitLabForbiddenException : GitLabException
{
    public GitLabForbiddenException()
    {
        Initialize();
    }

    public GitLabForbiddenException(string message)
        : base(message)
    {
        Initialize();
    }

    public GitLabForbiddenException(string message, Exception inner)
        : base(message, inner)
    {
        Initialize();
    }

    private GitLabForbiddenException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        Initialize();
    }

    private void Initialize()
    {
        StatusCode = HttpStatusCode.Forbidden;
    }
}
