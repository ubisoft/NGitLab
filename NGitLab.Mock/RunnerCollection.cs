using System;

namespace NGitLab.Mock
{
    public sealed class RunnerCollection : Collection<Runner>
    {
        public RunnerCollection(Project parent)
            : base(parent)
        {
        }

        public override void Add(Runner item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));

            if (Server is null)
                throw new ObjectNotAttachedException();

            if (item.Id == default)
            {
                item.Id = Server.GetNewRunnerId();
            }

            base.Add(item);
        }
    }
}
