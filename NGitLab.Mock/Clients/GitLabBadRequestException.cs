using System;
using System.Net;
using System.Runtime.Serialization;

namespace NGitLab.Mock.Clients;

public sealed class GitLabBadRequestException : GitLabException
{
    public GitLabBadRequestException()
    {
        Initialize();
    }

    public GitLabBadRequestException(string message)
        : base(message)
    {
        Initialize();
    }

    public GitLabBadRequestException(string message, Exception inner)
        : base(message, inner)
    {
        Initialize();
    }

    private GitLabBadRequestException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
        Initialize();
    }

    private void Initialize()
    {
        StatusCode = HttpStatusCode.BadRequest;
    }
}
