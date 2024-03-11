using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace NGitLab.Impl;

/// <summary>
/// A <see cref="IReadOnlyDictionary"/> wrapper around a <see cref="WebHeaderCollection"/>.
/// The purpose of this class is to keep the WebHeaderCollection type out of the public interface.
/// </summary>
internal sealed class WebHeadersDictionaryAdaptor : IReadOnlyDictionary<string, IEnumerable<string>>
{
    private readonly WebHeaderCollection _headers;
    private IEnumerable<string>[] _cachedValues;

    public WebHeadersDictionaryAdaptor(WebHeaderCollection headers)
    {
        _headers = headers ?? throw new ArgumentNullException(nameof(headers));
    }

    public int Count => _headers.Count;

    public IEnumerable<string> this[string key] => _headers.GetValues(key) ?? throw new InvalidOperationException("Header not found");

    public IEnumerable<string> Keys => _headers.AllKeys;

    public IEnumerable<IEnumerable<string>> Values => _cachedValues ??= this.Select(p => p.Value).ToArray();

    public bool ContainsKey(string key) => _headers.GetValues(key) is not null;

    public bool TryGetValue(string key, out IEnumerable<string> value) => (value = _headers.GetValues(key)) is not null;

    public IEnumerator<KeyValuePair<string, IEnumerable<string>>> GetEnumerator() => AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => AsEnumerable().GetEnumerator();

    private IEnumerable<KeyValuePair<string, IEnumerable<string>>> AsEnumerable()
    {
        foreach (var key in _headers.AllKeys)
        {
            // DO NOT use _headers.GetValues(int index) -- it does not return the expected value.
            yield return new(key, _headers.GetValues(key) ?? Enumerable.Empty<string>());
        }
    }
}
