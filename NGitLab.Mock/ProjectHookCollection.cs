using System;
using System.Linq;

namespace NGitLab.Mock
{
    public class ProjectHookCollection : Collection<ProjectHook>
    {
        public ProjectHookCollection(GitLabObject container)
            : base(container)
        {
        }

        public override void Add(ProjectHook item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (item.Id == default)
            {
                item.Id = GetNewId();
            }

            base.Add(item);
        }

        private int GetNewId()
        {
            return this.Select(hook => hook.Id).DefaultIfEmpty().Max() + 1;
        }
    }
}
