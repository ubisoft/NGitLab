using System;

namespace NGitLab.Mock;

public sealed class SystemHook : GitLabObject
{
    public long Id { get; internal set; }

    public Uri Url { get; set; }

    public DateTime CreatedAt { get; set; }

    public bool PushEvents { get; set; }

    public bool MergeRequestsEvents { get; set; }

    public bool TagPushEvents { get; set; }

    public bool RepositoryUpdateEvents { get; set; }

    public bool EnableSslVerification { get; set; }

    public Models.SystemHook ToClientSystemHook()
    {
        return new Models.SystemHook
        {
            Id = Id,
            Url = Url,
            CreatedAt = CreatedAt,
            PushEvents = PushEvents,
            MergeRequestsEvents = MergeRequestsEvents,
            TagPushEvents = TagPushEvents,
            RepositoryUpdateEvents = RepositoryUpdateEvents,
            EnableSslVerification = EnableSslVerification,
        };
    }
}
