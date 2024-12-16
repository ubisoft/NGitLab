using YamlDotNet.Serialization;

namespace NGitLab.Mock.Config;

public abstract class GitLabObject
{
    protected internal GitLabObject()
    {
    }

    public long Id { get; set; }

    [YamlIgnore]
    public object ParentObject { get; internal set; }
}

public abstract class GitLabObject<TParent> : GitLabObject
{
    protected internal GitLabObject()
    {
    }

    [YamlIgnore]
    public TParent Parent
    {
        get => (TParent)ParentObject;
        internal set => ParentObject = value;
    }
}
