using System;
using System.Collections;
using System.Collections.Generic;

namespace NGitLab.Mock
{
    public abstract class Collection<T> : IReadOnlyCollection<T>
        where T : GitLabObject
    {
        private readonly List<T> _items = new List<T>();

        protected Collection(GitLabObject parent)
        {
            Parent = parent ?? throw new ArgumentNullException(nameof(parent));
        }

        protected GitLabServer Server => Parent.Server;
        protected GitLabObject Parent { get; }

        public int Count => _items.Count;

        protected void Clear() => _items.Clear();
        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool Contains(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            return _items.Contains(item);
        }

        public virtual void Add(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            item.Parent = Parent;
            _items.Add(item);
        }

        public bool Remove(T item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            item.Parent = null;
            return _items.Remove(item);
        }
    }
}
