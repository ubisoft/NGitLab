using System;
using System.Collections.Generic;

namespace NGitLab.Models;

public class PipelineSchedule
{
    public int Id { get; set; }

    public string Description { get; set; }

    public string Ref { get; set; }

    public string Cron { get; set; }

    public DateTime NextRunAt { get; set; }

    public bool Active { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public User Owner { get; set; }

    public IEnumerable<PipelineVariable> Variables { get; set; }
}
