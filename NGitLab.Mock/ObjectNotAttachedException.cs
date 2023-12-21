using System;

namespace NGitLab.Mock;

public sealed class ObjectNotAttachedException : Exception
{
    public ObjectNotAttachedException()
        : base(message: "Object is not attacted to a GitLab server")
    {
    }
}
