using System.Runtime.Serialization;

namespace NGitLab.Models;

/// <summary>
/// Possible <see href="https://docs.gitlab.com/ci/jobs/job_rules/#ci_pipeline_source-predefined-variable">pipeline source</see> values.
/// Refer to <seealso href="https://gitlab.com/gitlab-org/gitlab/-/blob/master/doc/api/openapi/openapi_v2.yaml">/api/v4/projects/{id}/pipelines</seealso>
/// </summary>
public enum PipelineSource
{
    [EnumMember(Value = "unknown")]
    Unknown,
    [EnumMember(Value = "push")]
    Push,
    [EnumMember(Value = "web")]
    Web,
    [EnumMember(Value = "trigger")]
    Trigger,
    [EnumMember(Value = "schedule")]
    Schedule,
    [EnumMember(Value = "api")]
    Api,
    [EnumMember(Value = "external")]
    External,
    [EnumMember(Value = "pipeline")]
    Pipeline,
    [EnumMember(Value = "chat")]
    Chat,
    [EnumMember(Value = "webide")]
    WebIde,
    [EnumMember(Value = "merge_request_event")]
    MergeRequestEvent,
    [EnumMember(Value = "external_pull_request_event")]
    ExternalPullRequestEvent,
    [EnumMember(Value = "parent_pipeline")]
    ParentPipeline,
    [EnumMember(Value = "ondemand_dast_scan")]
    OndemandDastScan,
    [EnumMember(Value = "ondemand_dast_validation")]
    OndemandDastValidation,
    [EnumMember(Value = "security_orchestration_policy")]
    SecurityOrchestrationPolicy,
    [EnumMember(Value = "container_registry_push")]
    ContainerRegistryPush,
    [EnumMember(Value = "duo_workflow")]
    DuoWorkflow,
    [EnumMember(Value = "pipeline_execution_policy_schedule")]
    PipelineExecutionPolicySchedule,
}
