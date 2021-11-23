using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace NGitLab.Mock.Internals
{
    internal static class GitLabCollectionResponse
    {
        public static GitLabCollectionResponse<T> Create<T>(IEnumerable<T> items)
        {
            return new GitLabCollectionResponseImpl<T>(items);
        }

        private sealed class GitLabCollectionResponseImpl<T> : GitLabCollectionResponse<T>
        {
            private readonly IEnumerable<T> _items;

            public GitLabCollectionResponseImpl(IEnumerable<T> items)
            {
                _items = items;
            }

            public override async IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
            {
                await Task.Yield();
                foreach (var item in _items)
                {
                    yield return item;
                }
            }

            public override IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        }
    }
}
