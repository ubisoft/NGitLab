using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock;

public sealed class PipelineSchedule : GitLabObject
{
    public Project Project => (Project)Parent;

    public long Id { get; set; }

    public string Description { get; set; }

    public string Ref { get; set; }

    public string Cron { get; set; }

    public DateTime NextRunAt { get; set; }

    public bool Active { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public User Owner { get; set; }

    public IDictionary<string, string> Variables { get; set; } = new Dictionary<string, string>(StringComparer.Ordinal);

    public PipelineScheduleBasic ToPipelineScheduleBasicClient()
        => new()
        {
            Id = Id,
            Active = Active,
            CreatedAt = CreatedAt,
            Cron = Cron,
            Description = Description,
            NextRunAt = NextRunAt,
            Owner = Owner.ToClientUser(),
            Ref = Ref,
            UpdatedAt = UpdatedAt,
        };

    public Models.PipelineSchedule ToPipelineScheduleClient()
        => new()
        {
            Id = Id,
            Active = Active,
            CreatedAt = CreatedAt,
            Cron = Cron,
            Description = Description,
            NextRunAt = NextRunAt,
            Owner = Owner.ToClientUser(),
            Ref = Ref,
            UpdatedAt = UpdatedAt,
            Variables = Variables.Select((kvp) => new PipelineVariable
            {
                Key = kvp.Key,
                Value = kvp.Value,
            }),
        };

    public Pipeline AddNewPipeline()
    {
        var pipeline = new Pipeline(Ref)
        {
            Source = "schedule",
            Variables = Variables.Select((kvp) => new PipelineVariable
            {
                Key = kvp.Key,
                Value = kvp.Value,
            }),
            User = Owner,
        };

        Project.Pipelines.Add(pipeline);
        return pipeline;
    }
}
