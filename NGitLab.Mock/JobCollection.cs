using System;
using System.Linq;

namespace NGitLab.Mock
{
    public sealed class JobCollection : Collection<Job>
    {
        public JobCollection(GitLabObject parent)
            : base(parent)
        {
        }

        public Job GetById(int id)
        {
            return this.FirstOrDefault(job => job.Id == id);
        }

        public override void Add(Job job)
        {
            if (job is null)
                throw new ArgumentNullException(nameof(job));

            if (job.Id == default)
            {
                job.Id = Server.GetNewJobId();
            }

            base.Add(job);
        }

        public Job Add(Job job, Pipeline pipeline)
        {
            Add(job);
            job.Pipeline = pipeline;
            return job;
        }

        public Job AddNew()
        {
            var job = new Job();
            Add(job);
            return job;
        }

        public Job AddNew(Pipeline pipeline)
        {
            var job = new Job
            {
                Pipeline = pipeline,
            };

            Add(job);
            return job;
        }
    }
}
