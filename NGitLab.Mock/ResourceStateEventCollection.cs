using System;
using System.Collections.Generic;
using System.Linq;

namespace NGitLab.Mock
{
    public sealed class ResourceStateEventCollection : Collection<ResourceStateEvent>
    {
        public ResourceStateEventCollection(GitLabObject container)
            : base(container)
        {
        }

        public override void Add(ResourceStateEvent item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (item.Id == default)
            {
                item.Id = GetNewId();
            }

            base.Add(item);
        }

        internal IEnumerable<ResourceStateEvent> Get(int? resourceId)
        {
            var resourceStateEvents = this.AsQueryable();

            if (resourceId.HasValue)
            {
                resourceStateEvents = resourceStateEvents.Where(rle => rle.ResourceId == resourceId);
            }

            return resourceStateEvents;
        }

        private int GetNewId()
        {
            return this.Select(rle => rle.Id).DefaultIfEmpty().Max() + 1;
        }
    }
}
