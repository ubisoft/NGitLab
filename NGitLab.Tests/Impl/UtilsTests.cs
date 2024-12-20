using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.Impl;

public class UtilsTests
{
    [TestCase(EventAction.PushedTo, "pushed+to")]
    [TestCase(EventAction.Accepted, "accepted")]
    public void AddParameter_ConsidersEnumMemberAttribute(EventAction value, string expectedQueryParamValue)
    {
        const string basePath = "https://gitlab.org/api/v4/stuff";
        var url = basePath;
        url = NGitLab.Impl.Utils.AddParameter(url, "event_action", value);

        Assert.That(url, Is.EqualTo($"{basePath}?event_action={expectedQueryParamValue}"));
    }
}
