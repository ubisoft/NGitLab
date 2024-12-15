using System;
using System.Linq;

namespace NGitLab.Mock;

public sealed class Runner : GitLabObject
{
    public new Project Parent => (Project)base.Parent;

    public long Id { get; set; }

    public string Name { get; set; }

    public bool Paused { get; set; }

    public bool? Online { get; set; }

    public string Status { get; set; }

    public string Description { get; set; }

    public bool IsShared { get; set; }

    public DateTime ContactedAt { get; set; }

    public string Token { get; set; }

    public string[] TagList { get; set; }

    public string Version { get; set; }

    public string IpAddress { get; set; }

    public bool Locked { get; set; }

    public bool RunUntagged { get; set; }

    internal Models.Runner ToClientRunner(User currentUser)
    {
        return new Models.Runner
        {
            Id = Id,
            Name = Name,
            Paused = Paused,
            Online = Online ?? false,
            Status = Status,
            Description = Description,
            IsShared = IsShared,
            Projects = Parent.Server.AllProjects.Where(p => p.EnabledRunners.Any(r => r.Id == Id)).Select(p => p.ToClientProject(currentUser)).ToArray(),
            ContactedAt = ContactedAt,
            Token = Token,
            TagList = TagList,
            Version = Version,
            IpAddress = IpAddress,
            Locked = Locked,
            RunUntagged = RunUntagged,
            Groups = Parent.Server.AllGroups.Where(g => g.RegisteredRunners.Any(r => r.Id == Id)).Select(g => g.ToClientGroup(currentUser)).ToArray(),
        };
    }
}
