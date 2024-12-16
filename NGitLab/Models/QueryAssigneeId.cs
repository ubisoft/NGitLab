namespace NGitLab.Models;

/// <summary>
/// Specifies an assignee ID for a data query. It can be specified via
/// - an integer (the actual assignee ID on GitLab):    queries items assigned to the given user
/// - 'Any':                                            queries all assigned items
/// - 'None':                                           queries all unassigned items
/// </summary>
/// <see href="https://docs.gitlab.com/ee/api/issues.html#list-issues"/>
public sealed class QueryAssigneeId
{
    private readonly string _id;

    private QueryAssigneeId(string id)
    {
        _id = id;
    }

    public static QueryAssigneeId Any { get; } = new QueryAssigneeId("Any");

    public static QueryAssigneeId None { get; } = new QueryAssigneeId("None");

    public static implicit operator QueryAssigneeId(long id)
    {
        if (id == 0)
            return None;

        return new QueryAssigneeId(id.ToString());
    }

    public override string ToString()
    {
        return _id;
    }
}
