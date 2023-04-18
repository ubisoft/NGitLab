using System;
using System.Collections.Generic;
using System.Linq;

namespace NGitLab.Mock
{
    public sealed class ResourceMilestoneEventCollection : Collection<ResourceMilestoneEvent>
    {
        public ResourceMilestoneEventCollection(GitLabObject container)
            : base(container)
        {
        }

        public override void Add(ResourceMilestoneEvent item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (item.Id == default)
            {
                item.Id = GetNewId();
            }

            base.Add(item);
        }

        internal IEnumerable<ResourceMilestoneEvent> Get(int? resourceId)
        {
            var resourceMilestoneEvents = this.AsQueryable();

            if (resourceId.HasValue)
            {
                resourceMilestoneEvents = resourceMilestoneEvents.Where(rle => rle.ResourceId == resourceId);
            }

            return resourceMilestoneEvents;
        }

        private int GetNewId()
        {
            return this.Select(rle => rle.Id).DefaultIfEmpty().Max() + 1;
        }
    }
}
