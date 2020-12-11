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

        public Models.Job.JobPipeline Pipeline { get; set; }

        public Models.Job.JobProject Project { get; set; }

        public JobStatus Status { get; set; }

        public bool Tag { get; set; }

        public bool AllowFailure { get; set; }

        public User User { get; set; }

        public string WebUrl => Server.MakeUrl($"{Project.PathWithNamespace}/-/jobs/{Id.ToString(CultureInfo.InvariantCulture)}");

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
                Pipeline = Pipeline,
                Project = Project,
                Status = Status,
                AllowFailure = AllowFailure,
                Tag = Tag,
                User = User.ToClientUser(),
                WebUrl = WebUrl,
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
                Project = Project,
                Status = Status,
                Tag = Tag,
                User = User,
            };
        }
    }
}
