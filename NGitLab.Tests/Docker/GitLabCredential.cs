namespace NGitLab.Tests.Docker;

public record GitLabCredential
{
    public string AdminUserToken { get; set; }

    public string UserToken { get; set; }
}
