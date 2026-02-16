using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace NGitLab.Http;

/// <summary>
/// Adapter that provides dictionary-like access to HTTP response headers.
/// Replaces the obsolete WebHeadersDictionaryAdaptor.
/// </summary>
internal sealed class HttpResponseHeadersAdapter : IDictionary<string, string>
{
    private readonly HttpResponseMessage _response;

    public HttpResponseHeadersAdapter(HttpResponseMessage response)
    {
        _response = response ?? throw new ArgumentNullException(nameof(response));
    }

    public string this[string key]
    {
        get
        {
            // Try response headers first
            if (_response.Headers.TryGetValues(key, out var values))
            {
                return string.Join(",", values);
            }

            // Try content headers
            if (_response.Content?.Headers != null && _response.Content.Headers.TryGetValues(key, out values))
            {
                return string.Join(",", values);
            }

            throw new KeyNotFoundException($"Header '{key}' not found.");
        }
        set => throw new NotSupportedException("Headers are read-only.");
    }

    public ICollection<string> Keys
    {
        get
        {
            var keys = new List<string>(_response.Headers.Select(h => h.Key));
            if (_response.Content?.Headers != null)
            {
                keys.AddRange(_response.Content.Headers.Select(h => h.Key));
            }

            return keys;
        }
    }

    public ICollection<string> Values
    {
        get
        {
            var values = new List<string>();
            foreach (var header in _response.Headers)
            {
                values.Add(string.Join(",", header.Value));
            }

            if (_response.Content?.Headers != null)
            {
                foreach (var header in _response.Content.Headers)
                {
                    values.Add(string.Join(",", header.Value));
                }
            }

            return values;
        }
    }

    public int Count => _response.Headers.Count() + (_response.Content?.Headers?.Count() ?? 0);

    public bool IsReadOnly => true;

    public void Add(string key, string value) => throw new NotSupportedException("Headers are read-only.");

    public void Add(KeyValuePair<string, string> item) => throw new NotSupportedException("Headers are read-only.");

    public void Clear() => throw new NotSupportedException("Headers are read-only.");

    public bool Contains(KeyValuePair<string, string> item)
    {
        if (_response.Headers.TryGetValues(item.Key, out var values))
        {
            return string.Equals(string.Join(",", values), item.Value, StringComparison.Ordinal);
        }

        if (_response.Content?.Headers != null && _response.Content.Headers.TryGetValues(item.Key, out values))
        {
            return string.Equals(string.Join(",", values), item.Value, StringComparison.Ordinal);
        }

        return false;
    }

    public bool ContainsKey(string key)
    {
        return _response.Headers.Contains(key) ||
               (_response.Content?.Headers?.Contains(key) ?? false);
    }

    public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));

        var index = arrayIndex;
        foreach (var header in _response.Headers)
        {
            array[index++] = new KeyValuePair<string, string>(header.Key, string.Join(",", header.Value));
        }

        if (_response.Content?.Headers != null)
        {
            foreach (var header in _response.Content.Headers)
            {
                array[index++] = new KeyValuePair<string, string>(header.Key, string.Join(",", header.Value));
            }
        }
    }

    public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
    {
        foreach (var header in _response.Headers)
        {
            yield return new KeyValuePair<string, string>(header.Key, string.Join(",", header.Value));
        }

        if (_response.Content?.Headers != null)
        {
            foreach (var header in _response.Content.Headers)
            {
                yield return new KeyValuePair<string, string>(header.Key, string.Join(",", header.Value));
            }
        }
    }

    public bool Remove(string key) => throw new NotSupportedException("Headers are read-only.");

    public bool Remove(KeyValuePair<string, string> item) => throw new NotSupportedException("Headers are read-only.");

    public bool TryGetValue(string key, out string value)
    {
        if (_response.Headers.TryGetValues(key, out var values))
        {
            value = string.Join(",", values);
            return true;
        }

        if (_response.Content?.Headers != null && _response.Content.Headers.TryGetValues(key, out values))
        {
            value = string.Join(",", values);
            return true;
        }

        value = null!;
        return false;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
