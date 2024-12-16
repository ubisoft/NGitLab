namespace NGitLab.Mock;

public sealed class ProjectIssueNote : Note
{
    public new Issue Parent => (Issue)base.Parent;

    public override string NoteableType => "Issue";

    public override long NoticableId => Parent.Id;

    public override long NoticableIid => Parent.Iid;

    internal Models.ProjectIssueNote ToProjectIssueNote()
    {
        return new Models.ProjectIssueNote
        {
            NoteId = (int)Id,
            Body = Body,
            Author = Author.ToClientAuthor(),
            CreatedAt = CreatedAt.UtcDateTime,
            UpdatedAt = UpdatedAt.UtcDateTime,
            System = System,
            Resolvable = Resolvable,
            Confidential = Confidential,
        };
    }
}
