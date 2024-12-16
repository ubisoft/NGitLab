namespace NGitLab.Mock;

public class Change : GitLabObject
{
    public string OldPath { get; set; }

    public string NewPath { get; set; }

    public long AMode { get; set; }

    public long BMode { get; set; }

    public bool NewFile { get; set; }

    public bool RenamedFile { get; set; }

    public bool DeletedFile { get; set; }

    public string Diff { get; set; }

    public Models.Change ToChange()
    {
        return new()
        {
            Diff = Diff,
            AMode = AMode,
            BMode = BMode,
            DeletedFile = DeletedFile,
            NewFile = NewFile,
            NewPath = NewPath,
            OldPath = OldPath,
            RenamedFile = RenamedFile,
        };
    }
}
