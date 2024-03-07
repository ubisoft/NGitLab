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

    public IEnumerator<KeyValuePair<string, IEnumerable<string>>> GetEnumerator() => new WebHeaderCollectionEnumerator(_headers);

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private IEnumerable<KeyValuePair<string, IEnumerable<string>>> AsEnumerable()
    {
        // FYI: WebHeaderCollection is implemented using arrays, so this will be fast.
        for (var i = 0; i < _headers.Count; i++)
        {
            yield return new(_headers.GetKey(i), _headers.GetValues(i) ?? Enumerable.Empty<string>());
        }
    }

    private class WebHeaderCollectionEnumerator : IEnumerator<KeyValuePair<string, IEnumerable<string>>>
    {
        private readonly WebHeaderCollection _headers;
        private readonly IEnumerator _keyEnumerator;

        public WebHeaderCollectionEnumerator(WebHeaderCollection headers)
        {
            _headers = headers ?? throw new ArgumentNullException(nameof(headers));
            _keyEnumerator = _headers.GetEnumerator();
        }

        public KeyValuePair<string, IEnumerable<string>> Current
        {
            get
            {
                var key = (string)_keyEnumerator.Current;
                return new(key, _headers.GetValues(key) ?? Enumerable.Empty<string>());
            }
        }

        object IEnumerator.Current => Current;

        public bool MoveNext() => _keyEnumerator.MoveNext();

        public void Reset() => _keyEnumerator.Reset();

        public void Dispose()
        {
        }
    }
}
