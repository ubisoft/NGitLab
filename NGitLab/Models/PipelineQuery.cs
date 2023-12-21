using System;

namespace NGitLab.Models;

public class PipelineQuery
{
    public PipelineScope? Scope { get; set; }

    public JobStatus? Status { get; set; }

    public string Ref { get; set; }

    public string Sha { get; set; }

    public bool? YamlErrors { get; set; }

    public string Name { get; set; }

    public string Username { get; set; }

    public DateTimeOffset? UpdatedAfter { get; set; }

    public DateTimeOffset? UpdatedBefore { get; set; }

    public PipelineOrderBy? OrderBy { get; set; }

    public PipelineSort? Sort { get; set; }

    public int? PerPage { get; set; }
}
