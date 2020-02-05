using System;

namespace NGitLab.Mock
{
    public sealed class Pipeline : GitLabObject
    {
        public Pipeline(string @ref)
        {
            Ref = @ref;
        }

        public int Id { get; set; }
        public JobStatus Status { get; set; }
        public string Ref { get; set; }
        public bool Tag { get; set; }
        public Sha1 Sha { get; set; }
        public Sha1 BeforeSha { get; set; }
        public string YamlError { get; set; }
        public User User { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }
        public DateTimeOffset? StartedAt { get; set; }
        public DateTimeOffset? FinishedAt { get; set; }
        public DateTimeOffset CommittedAt { get; set; }
        public TimeSpan? Duration { get; set; }
        public double Coverage { get; set; }

        public Job AddNewJob(Project project)
        {
            return AddJob(project, new Job());
        }

        public Job AddJob(Project project, Job job)
        {
            return project.Jobs.Add(job, this);
        }

        internal Models.Job.JobPipeline ToJobPipeline()
        {
            return new Models.Job.JobPipeline
            {
                Id = Id,
                Ref = Ref,
                Sha = Sha,
                Status = Status,
            };
        }

        internal Models.PipelineBasic ToPipelineBasicClient()
        {
            return new Models.PipelineBasic
            {
                Id = Id,
                Status = Status,
                Ref = Ref,
                Sha = Sha,
            };
        }

        internal Models.Pipeline ToPipelineClient()
        {
            return new Models.Pipeline
            {
                Id = Id,
                Status = Status,
                Ref = Ref,
                Tag = Tag,
                Sha = Sha,
                BeforeSha = BeforeSha,
                YamlError = YamlError,
                User = User.ToClientUser(),
                CreatedAt = CreatedAt.UtcDateTime,
                UpdatedAt = UpdatedAt.UtcDateTime,
                StartedAt = StartedAt.HasValue ? StartedAt.Value.DateTime : new DateTime(),
                FinishedAt = FinishedAt.HasValue ? FinishedAt.Value.DateTime : new DateTime(),
                Duration = Duration.HasValue ? Duration.Value.Ticks : 0,
                Coverage = Coverage,
            };
        }
    }
}
