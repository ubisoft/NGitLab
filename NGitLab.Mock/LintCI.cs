namespace NGitLab.Mock;

public sealed class LintCI : GitLabObject
{
    public LintCI(string @ref, bool valid, params string[] errors)
    {
        Ref = @ref;
        Valid = valid;
        Errors = errors;
    }

    public string Ref { get; }

    public bool Valid { get; }

    public string[] Errors { get; }
}
