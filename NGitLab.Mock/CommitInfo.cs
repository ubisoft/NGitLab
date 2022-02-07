namespace NGitLab.Mock
{
    public sealed class CommitInfo : GitLabObject
    {
        public string Sha { get; set; }

        public string Status { get; set; }
    }
}
