namespace NGitLab.Mock
{
    public sealed class MergeRequestComment : Note
    {
        public new MergeRequest Parent => (MergeRequest)base.Parent;

        public override string NoteableType => "MergeRequest";

        public override int NoticableId => Parent.Id;

        public override int NoticableIid => Parent.Iid;

        internal Models.MergeRequestComment ToMergeRequestCommentClient()
        {
            return new Models.MergeRequestComment
            {
                Id = Id,
                Body = Body,
                CreatedAt = CreatedAt.UtcDateTime,
                UpdatedAt = UpdatedAt.UtcDateTime,
                Resolved = Resolved,
                Resolvable = Resolvable,
                System = System,
                Type = NoteableType,
                Author = Author.ToUserClient(),
            };
        }
    }
}
