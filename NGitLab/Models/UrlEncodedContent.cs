using System.Collections.Generic;

namespace NGitLab.Models;

public sealed class UrlEncodedContent
{
    public IReadOnlyDictionary<string, string> Values { get; }

    public UrlEncodedContent(IReadOnlyDictionary<string, string> values)
    {
        Values = values ?? throw new System.ArgumentNullException(nameof(values));
    }
}
