namespace NGitLab.Tests.Docker;

public record GitLabCredential
{
    public string AdminUserToken { get; set; }

    public string AdminCookies { get; set; }

    public string UserToken { get; set; }

    public string ProfileToken { get; set; }
}
