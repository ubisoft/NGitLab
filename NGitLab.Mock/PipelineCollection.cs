using System;
using System.Linq;

namespace NGitLab.Mock
{
    public sealed class PipelineCollection : Collection<Pipeline>
    {
        public PipelineCollection(GitLabObject parent)
            : base(parent)
        {
        }

        public Pipeline GetById(int id)
        {
            return this.FirstOrDefault(pipeline => pipeline.Id == id);
        }

        public override void Add(Pipeline pipeline)
        {
            if (pipeline is null)
                throw new ArgumentNullException(nameof(pipeline));

            if (pipeline.Id == default)
            {
                pipeline.Id = Server.GetNewPipelineId();
            }

            base.Add(pipeline);
        }

        public Pipeline Add(string @ref, JobStatus status, User user)
        {
            var pipeline = new Pipeline(@ref)
            {
                Status = status,
                User = user,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            Add(pipeline);
            return pipeline;
        }
    }
}
