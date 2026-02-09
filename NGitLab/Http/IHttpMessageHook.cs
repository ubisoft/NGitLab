using System.Net.Http;

namespace NGitLab.Http;

/// <summary>
/// Provides an extensibility point for customizing HTTP request behavior.
/// This interface replaces the obsolete virtual methods on <see cref="RequestOptions"/>.
/// </summary>
public interface IHttpMessageHook
{
    void BeforeSending(HttpRequestMessage request);

    void AfterReceived(HttpResponseMessage response);
}
