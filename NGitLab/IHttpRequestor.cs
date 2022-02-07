using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace NGitLab
{
    public interface IHttpRequestor
    {
        IEnumerable<T> GetAll<T>(string tailUrl);

        GitLabCollectionResponse<T> GetAllAsync<T>(string tailUrl);

        void Stream(string tailAPIUrl, Action<Stream> parser);

        Task StreamAsync(string tailAPIUrl, Func<Stream, Task> parser, CancellationToken cancellationToken);

        T To<T>(string tailAPIUrl);

        Task<T> ToAsync<T>(string tailAPIUrl, CancellationToken cancellationToken);

        void Execute(string tailAPIUrl);

        Task ExecuteAsync(string tailAPIUrl, CancellationToken cancellationToken);

        IHttpRequestor With(object data);
    }
}
