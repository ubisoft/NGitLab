﻿using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock
{
    public sealed class Pipeline : GitLabObject
    {
        public Pipeline(string @ref)
        {
            Ref = @ref;
        }

        public new Project Parent => (Project)base.Parent;

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

        public IEnumerable<PipelineVariable> Variables { get; set; }

        public IEnumerable<TestReports> TestReports { get; set; }

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
                StartedAt = StartedAt.HasValue ? StartedAt.Value.UtcDateTime : default,
                FinishedAt = FinishedAt.HasValue ? FinishedAt.Value.UtcDateTime : default,
                Duration = Duration.HasValue ? Duration.Value.Ticks : 0,
                Coverage = Coverage,
            };
        }
    }
}
