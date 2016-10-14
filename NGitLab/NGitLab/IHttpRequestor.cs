using NGitLab.Impl;
using System;
using System.Collections.Generic;
using System.IO;

namespace NGitLab
{
    public interface IHttpRequestor
    {
        IHttpRequestor SetMethodType(MethodType methodType);

        IEnumerable<T> GetAll<T>(string tailUrl);

        void Stream(string tailAPIUrl, Action<Stream> parser);

        T To<T>(string tailAPIUrl);

        IHttpRequestor With(object data);
    }
}
