using System;
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

    [Test]
    public void AppendSegmentToUrl_ValueIsNullIncludeSegmentSeparatorFalse_ReturnsUrlWithoutAnyChange()
    {
        // Arrange
        const string basePath = "https://gitlab.org/api/v4/stuff";
        var url = basePath;
        var expected = basePath;

        // Act
        var actual = NGitLab.Impl.Utils.AppendSegmentToUrl<string>(url, value: null, includeSegmentSeparator: false);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void AppendSegmentToUrl_ValueIsNullIncludeSegmentSeparatorTrue_ReturnsUrlWithoutAnyChange()
    {
        // Arrange
        const string basePath = "https://gitlab.org/api/v4/stuff";
        var url = basePath;
        var expected = basePath;

        // Act
        var actual = NGitLab.Impl.Utils.AppendSegmentToUrl<string>(url, value: null, includeSegmentSeparator: true);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void AppendSegmentToUrl_UrlAlreadyContainsQueryString_ThrowsInvalidOperationException()
    {
        // Arrange
        const string basePath = "https://gitlab.org/api/v4/stuff";
        var url = basePath;

        url = NGitLab.Impl.Utils.AddParameter(url, "param1", "one");
        var action = new Action(() => NGitLab.Impl.Utils.AppendSegmentToUrl(url, "segment"));

        // Act and Assert
        Assert.Throws<InvalidOperationException>(action);
    }

    [TestCase("https://gitlab.org/api/v4/stuff/", "/segment")]
    [TestCase("https://gitlab.org/api/v4/stuff/", "segment")]
    [TestCase("https://gitlab.org/api/v4/stuff", "/segment")]
    [TestCase("https://gitlab.org/api/v4/stuff", "segment")]
    public void AppendSegmentToUrl_IncludeSegmentSeparatorIsTrue_SegmentAppendedWithSeparator(string basePath, string segment)
    {
        // Arrange
        var url = basePath;
        var expected = "https://gitlab.org/api/v4/stuff/segment";

        // Act
        var actual = NGitLab.Impl.Utils.AppendSegmentToUrl(url, value: segment, includeSegmentSeparator: true);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase("https://gitlab.org/api/v4/stuff/", "/segment")]
    [TestCase("https://gitlab.org/api/v4/stuff/", "segment")]
    [TestCase("https://gitlab.org/api/v4/stuff", "/segment")]
    [TestCase("https://gitlab.org/api/v4/stuff", "segment")]
    public void AppendSegmentToUrl_IncludeSegmentSeparatorIsFalse_SegmentAppendedWithoutSeparator(string basePath, string segment)
    {
        // Arrange
        var url = basePath;
        var expected = "https://gitlab.org/api/v4/stuffsegment";

        // Act
        var actual = NGitLab.Impl.Utils.AppendSegmentToUrl(url, value: segment, includeSegmentSeparator: false);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase("https://gitlab.org/api/v4/stuff.bz2", FileArchiveFormat.Bz2)]
    [TestCase("https://gitlab.org/api/v4/stuff.tar", FileArchiveFormat.Tar)]
    [TestCase("https://gitlab.org/api/v4/stuff.tar.bz2", FileArchiveFormat.TarBz2)]
    [TestCase("https://gitlab.org/api/v4/stuff.tar.gz", FileArchiveFormat.TarGz)]
    [TestCase("https://gitlab.org/api/v4/stuff.tb2", FileArchiveFormat.Tb2)]
    [TestCase("https://gitlab.org/api/v4/stuff.tbz", FileArchiveFormat.Tbz)]
    [TestCase("https://gitlab.org/api/v4/stuff.tbz2", FileArchiveFormat.Tbz2)]
    [TestCase("https://gitlab.org/api/v4/stuff.zip", FileArchiveFormat.Zip)]
    public void AppendSegmentToUrl_ValueIsEnumWithEnumMemberAttribute_EnumMemberValueAppended(string expected, FileArchiveFormat fileArchiveFormat)
    {
        // Arrange
        const string basePath = "https://gitlab.org/api/v4/stuff";
        var url = basePath;

        // Act
        var actual = NGitLab.Impl.Utils.AppendSegmentToUrl(url, value: fileArchiveFormat, includeSegmentSeparator: false);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase("https://gitlab.org/api/v4/stuff/Group", BadgeKind.Group)]
    [TestCase("https://gitlab.org/api/v4/stuff/Project", BadgeKind.Project)]
    public void AppendSegmentToUrl_ValueIsEnumWithoutEnumMemberAttribute_EnumToStringValueAppended(string expected, BadgeKind badgeKind)
    {
        // Arrange
        const string basePath = "https://gitlab.org/api/v4/stuff";
        var url = basePath;

        // Act
        var actual = NGitLab.Impl.Utils.AppendSegmentToUrl(url, value: badgeKind, includeSegmentSeparator: true);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [TestCase(EventAction.PushedTo, "pushed to")]
    [TestCase(EventAction.Accepted, "accepted")]
    public void ToValueString_ConsidersEnumMemberAttribute(EventAction value, string expectedValueResult)
    {
        var valueMember = NGitLab.Impl.Utils.ToValueString(value);

        Assert.That(valueMember, Is.EqualTo(expectedValueResult));
    }
}
