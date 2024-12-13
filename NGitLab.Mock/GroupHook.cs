using System;

namespace NGitLab.Mock;

public sealed class GroupHook : GitLabObject
{
    public new Group Parent => (Group)base.Parent;

    public long Id { get; internal set; }

    public Uri Url { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool PushEvents { get; set; }

    public bool MergeRequestsEvents { get; set; }

    public bool IssuesEvents { get; set; }

    public bool TagPushEvents { get; set; }

    public bool NoteEvents { get; set; }

    public bool JobEvents { get; set; }

    public bool PipelineEvents { get; set; }

    public bool WikiPagesEvents { get; set; }

    public bool EnableSslVerification { get; set; }

    public string Token { get; set; }

    public Models.GroupHook ToClientGroupHook()
    {
        return new Models.GroupHook
        {
            Id = Id,
            Url = Url,
            GroupId = Parent.Id,
            CreatedAt = CreatedAt,
            PushEvents = PushEvents,
            MergeRequestsEvents = MergeRequestsEvents,
            IssuesEvents = IssuesEvents,
            TagPushEvents = TagPushEvents,
            NoteEvents = NoteEvents,
            JobEvents = JobEvents,
            PipelineEvents = PipelineEvents,
            WikiPagesEvents = WikiPagesEvents,
            EnableSslVerification = EnableSslVerification,
            Token = Token,
        };
    }
}
