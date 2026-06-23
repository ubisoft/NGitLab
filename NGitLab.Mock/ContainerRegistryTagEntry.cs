using NGitLab.Models;

namespace NGitLab.Mock;

public sealed class ContainerRegistryTagEntry
{
    public string Name { get; set; }

    public Models.ContainerRegistryTag ToClientContainerRegistryTag()
    {
        return new Models.ContainerRegistryTag
        {
            Name = Name,
        };
    }
}
