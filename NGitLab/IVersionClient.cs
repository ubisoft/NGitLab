using NGitLab.Models;

namespace NGitLab;

public interface IVersionClient
{
    GitLabVersion Get();
}
