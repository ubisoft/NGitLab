using System;
using System.Collections.Generic;
using System.Linq;

namespace NGitLab.Mock
{
    public sealed class ResourceLabelEventCollection : Collection<ResourceLabelEvent>
    {
        public ResourceLabelEventCollection(GitLabObject container)
            : base(container)
        {
        }

        public override void Add(ResourceLabelEvent item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (item.Id == default)
            {
                item.Id = GetNewId();
            }

            base.Add(item);
        }

        internal IEnumerable<ResourceLabelEvent> Get(int? resourceId)
        {
            var resourceLabelEvents = this.AsQueryable();

            if (resourceId.HasValue)
            {
                resourceLabelEvents = resourceLabelEvents.Where(rle => rle.ResourceId == resourceId);
            }

            return resourceLabelEvents;
        }

        private int GetNewId()
        {
            return this.Select(rle => rle.Id).DefaultIfEmpty().Max() + 1;
        }
    }
}
