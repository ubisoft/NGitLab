using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace NGitLab
{
    public abstract class GitLabCollectionResponse<T> : IEnumerable<T>, IAsyncEnumerable<T>
    {
        public abstract IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default);

        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
