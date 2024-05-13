using System.Collections.Generic;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class EventClient : ClientBase, IEventClient
{
    private readonly int? _userId;
    private readonly int? _projectId;

    public EventClient(ClientContext context)
        : base(context)
    {
    }

    public EventClient(ClientContext context, int? userId = null, ProjectId? projectId = null)
        : base(context)
    {
        _userId = userId;
        _projectId = projectId.HasValue ? Server.AllProjects.FindProject(projectId.ValueAsString()).Id : null;
    }

    IEnumerable<Models.Event> IEventClient.Get(EventQuery query)
    {
        using (Context.BeginOperationScope())
        {
            return Server.Events.Get(query, _userId, _projectId).Select(e => e.ToClientEvent());
        }
    }
}
