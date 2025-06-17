﻿using System;
using System.Collections.Generic;
using System.Net;
#if !NET8_0_OR_GREATER
using System.Runtime.Serialization;
#endif
using NGitLab.Impl;

namespace NGitLab;

/// <summary>
/// Thrown when GitLab returns an error on a call.
/// </summary>
[Serializable]
public class GitLabException : Exception
{
    public GitLabException()
    {
    }

    public GitLabException(string message)
        : base(message)
    {
    }

    public GitLabException(string message, Exception inner)
        : base(message, inner)
    {
    }

#if !NET8_0_OR_GREATER
    protected GitLabException(
        SerializationInfo info,
        StreamingContext context)
        : base(info, context)
    {
    }
#endif

    /// <summary>
    /// The error code returned from the server.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// Dictionary of JSON properties providing further details about the GitLab error.
    /// </summary>
    public IDictionary<string, object> ErrorObject { get; set; }

    /// <summary>
    /// The extracted message from the error object.
    /// </summary>
    public string ErrorMessage { get; set; }

    /// <summary>
    /// The call that triggerred the error.
    /// </summary>
    public Uri OriginalCall { get; set; }

    /// <summary>
    /// HTTP request method, if any, that triggered this exception
    /// </summary>
    public MethodType? MethodType { get; set; }

    internal static GitLabException NotFound(string message = null) => new(message) { StatusCode = HttpStatusCode.NotFound };

    internal static GitLabException Forbidden(string message = null) => new(message) { StatusCode = HttpStatusCode.Forbidden };

    internal static GitLabException BadRequest(string message = null) => new(message) { StatusCode = HttpStatusCode.BadRequest };
}
