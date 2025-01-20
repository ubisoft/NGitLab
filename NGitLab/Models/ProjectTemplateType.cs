using System.Runtime.Serialization;

namespace NGitLab.Models;

public enum ProjectTemplateType
{
    [EnumMember(Value = "dockerfiles")]
    Dockerfiles,

    [EnumMember(Value = "gitignores")]
    Gitignores,

    [EnumMember(Value = "gitlab_ci_ymls")]
    GitlabCiYmls,

    [EnumMember(Value = "licenses")]
    Licenses,

    [EnumMember(Value = "issues")]
    Issues,

    [EnumMember(Value = "merge_requests")]
    MergeRequests,
}
