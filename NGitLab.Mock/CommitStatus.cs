namespace NGitLab.Mock
{
    public sealed class CommitStatus : GitLabObject
    {
        public new Project Parent => (Project)base.Parent;

        public string Sha { get; set; }

        public string Status { get; set; }

        public string Ref { get; set; }

        public string Name { get; set; }

        public string TargetUrl { get; set; }

        public string Description { get; set; }

        public int? Coverage { get; set; }

        public Models.CommitStatusCreate ToClientCommitStatusCreate()
        {
            return new Models.CommitStatusCreate
            {
                Name = Name,
                TargetUrl = TargetUrl,
                Status = Status,
                Ref = Ref,
                CommitSha = Sha,
                Description = Description,
                State = Status,
                Coverage = Coverage,
                ProjectId = Parent.Id,
            };
        }

        public Models.CommitStatus ToClientCommitStatus()
        {
            return new Models.CommitStatus
            {
                CommitSha = Sha,
                Name = Name,
                ProjectId = Parent.Id,
                Ref = Ref,
                Status = Status,
            };
        }
    }
}
