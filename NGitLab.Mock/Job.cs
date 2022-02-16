using System;
using System.Globalization;
using NGitLab.Models;

namespace NGitLab.Mock
{
    public sealed class Job : GitLabObject
    {
        public Job()
        {
        }

        public string Name { get; set; }

        public int Id { get; set; }

        public string Ref { get; set; }

        public Commit Commit { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime StartedAt { get; set; }

        public DateTime FinishedAt { get; set; }

        public string Stage { get; set; }

        public double? Coverage { get; set; }

        public Models.Job.JobArtifact Artifacts { get; set; }

        public Models.Job.JobRunner Runner { get; set; }

        public Pipeline Pipeline { get; set; }

        public Project Project => Pipeline.Parent;

        public JobStatus Status { get; set; }

        public bool Tag { get; set; }

        public bool AllowFailure { get; set; }

        public User User { get; set; }

        public string WebUrl => Server.MakeUrl($"{Project.PathWithNamespace}/-/jobs/{Id.ToString(CultureInfo.InvariantCulture)}");

        public float? Duration { get; set; }

        public float? QueuedDuration { get; set; }

        public string Trace { get; set; }

        internal Models.Job ToJobClient()
        {
            return new Models.Job
            {
                Name = Name,
                Id = Id,
                Ref = Ref,
                Commit = Commit,
                CreatedAt = CreatedAt,
                StartedAt = StartedAt,
                FinishedAt = FinishedAt,
                Stage = Stage,
                Coverage = Coverage,
                Artifacts = Artifacts,
                Runner = Runner,
                Pipeline = new Models.Job.JobPipeline
                {
                    Id = Pipeline.Id,
                    Ref = Pipeline.Ref,
                    Sha = Pipeline.Sha,
                    Status = Pipeline.Status,
                },
                Project = new Models.Job.JobProject
                {
                    Id = Project.Id,
                    Name = Project.Name,
                    PathWithNamespace = Project.PathWithNamespace,
                },
                Status = Status,
                AllowFailure = AllowFailure,
                Tag = Tag,
                User = User?.ToClientUser(),
                WebUrl = WebUrl,
                Duration = Duration,
                QueuedDuration = QueuedDuration,
            };
        }

        internal Job Clone()
        {
            return new Job
            {
                Id = Server.GetNewJobId(),
                Name = Name,
                Ref = Ref,
                Commit = Commit,
                CreatedAt = CreatedAt,
                StartedAt = StartedAt,
                FinishedAt = FinishedAt,
                Stage = Stage,
                Coverage = Coverage,
                Artifacts = Artifacts,
                Runner = Runner,
                Pipeline = Pipeline,
                Status = Status,
                Tag = Tag,
                User = User,
                Duration = Duration,
                QueuedDuration = QueuedDuration,
            };
        }
    }
}
