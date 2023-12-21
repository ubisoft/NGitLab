using System;
using System.Net;
using NGitLab.Models;

namespace NGitLab.Impl;

/// <summary>
/// View api documentation at: https://docs.gitlab.com/ce/api/README.html
/// </summary>
public class API
{
    private readonly GitLabCredentials _credentials;

    public string ConnectionToken { get; set; }

    public RequestOptions RequestOptions { get; set; }

    public API(GitLabCredentials credentials)
        : this(credentials, RequestOptions.Default)
    {
    }

    public API(GitLabCredentials credentials, RequestOptions options)
    {
        _credentials = credentials;
        RequestOptions = options;
    }

    public IHttpRequestor Get() => CreateRequestor(MethodType.Get);

    public IHttpRequestor Post() => CreateRequestor(MethodType.Post);

    public IHttpRequestor Put() => CreateRequestor(MethodType.Put);

    public IHttpRequestor Delete() => CreateRequestor(MethodType.Delete);

    public IHttpRequestor Head() => CreateRequestor(MethodType.Head);

    protected virtual IHttpRequestor CreateRequestor(MethodType methodType)
    {
        _credentials.ApiToken ??= OpenPrivateSession();

        return new HttpRequestor(_credentials.HostUrl, _credentials.ApiToken, methodType, RequestOptions);
    }

    private string OpenPrivateSession()
    {
        var httpRequestor = new HttpRequestor(_credentials.HostUrl, string.Empty, MethodType.Post, RequestOptions);
        var url =
            $"/session?login={WebUtility.UrlEncode(_credentials.UserName)}&password={WebUtility.UrlEncode(_credentials.Password)}";
        try
        {
            var session = httpRequestor.To<Session>(url);
            return session.PrivateToken;
        }
        catch (GitLabException ex)
        {
            const string hiddenPassword = "*****";

            var securedException = new GitLabException(ex.Message.Replace(_credentials.Password, hiddenPassword))
            {
                OriginalCall = new Uri(ex.OriginalCall.OriginalString.Replace(_credentials.Password, hiddenPassword)),
                StatusCode = ex.StatusCode,
                ErrorObject = ex.ErrorObject,
                MethodType = ex.MethodType,
            };

            throw securedException;
        }
    }
}
