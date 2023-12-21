using YamlDotNet.Serialization;

namespace NGitLab.Mock.Config;

public abstract class GitLabObject
{
    protected internal GitLabObject()
    {
    }

    public int Id { get; set; }

    [YamlIgnore]
    public object Parent { get; internal set; }
}

public abstract class GitLabObject<TParent> : GitLabObject
{
    protected internal GitLabObject()
    {
    }

    [YamlIgnore]
    public new TParent Parent
    {
        get => (TParent)base.Parent;
        internal set => base.Parent = value;
    }
}
