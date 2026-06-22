using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Mock;

public sealed class ContainerRepository : GitLabObject
{
    public new Project Parent => (Project)base.Parent;

    public long Id { get; set; }

    public string Name { get; set; }

    public List<ContainerRegistryTagEntry> Tags { get; } = [];

    public Models.ContainerRepository ToClientContainerRepository()
    {
        return new Models.ContainerRepository
        {
            Id = Id,
            Name = Name,
            ProjectId = Parent?.Id ?? 0,
            TagsCount = Tags.Count,
        };
    }
}
