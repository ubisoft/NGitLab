namespace NGitLab.Mock.Fluent
{
    public abstract class GitLabObject
    {
        protected internal GitLabObject()
        {
        }

        public int Id { get; set; }

        public object Parent { get; protected set; }
    }

    public abstract class GitLabObject<TParent> : GitLabObject
    {
        protected internal GitLabObject()
        {
        }

        public new TParent Parent
        {
            get => (TParent)base.Parent;
            internal set => base.Parent = value;
        }
    }
}
