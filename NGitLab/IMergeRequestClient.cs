using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

public interface IMergeRequestClient
{
    IEnumerable<MergeRequest> All { get; }

    IEnumerable<MergeRequest> AllInState(MergeRequestState state);

    IEnumerable<MergeRequest> Get(MergeRequestQuery query);

    MergeRequest this[long iid] { get; }

    Task<MergeRequest> GetByIidAsync(long iid, SingleMergeRequestQuery options, CancellationToken cancellationToken = default);

    MergeRequest Create(MergeRequestCreate mergeRequest);

    MergeRequest Update(long mergeRequestIid, MergeRequestUpdate mergeRequest);

    MergeRequest Close(long mergeRequestIid);

    MergeRequest Reopen(long mergeRequestIid);

    void Delete(long mergeRequestIid);

    MergeRequest CancelMergeWhenPipelineSucceeds(long mergeRequestIid);

    [Obsolete("You should use MergeRequestMerge instead of MergeRequestAccept")]
    MergeRequest Accept(long mergeRequestIid, MergeRequestAccept message);

    MergeRequest Accept(long mergeRequestIid, MergeRequestMerge message);

    MergeRequest Approve(long mergeRequestIid, MergeRequestApprove message);

    RebaseResult Rebase(long mergeRequestIid);

    Task<RebaseResult> RebaseAsync(long mergeRequestIid, MergeRequestRebase options, CancellationToken cancellationToken = default);

    IEnumerable<PipelineBasic> GetPipelines(long mergeRequestIid);

    IEnumerable<Author> GetParticipants(long mergeRequestIid);

    GitLabCollectionResponse<MergeRequestVersion> GetVersionsAsync(long mergeRequestIid);

    IMergeRequestCommentClient Comments(long mergeRequestIid);

    IMergeRequestDiscussionClient Discussions(long mergeRequestIid);

    IMergeRequestCommitClient Commits(long mergeRequestIid);

    IMergeRequestChangeClient Changes(long mergeRequestIid);

    IMergeRequestApprovalClient ApprovalClient(long mergeRequestIid);

    IEnumerable<Issue> ClosesIssues(long mergeRequestIid);

    /// <summary>
    /// Get time tracking statistics
    /// </summary>
    /// <param name="mergeRequestIid">The id of the merge request in the project's scope.</param>
    /// <returns>The time tracking statistics of the merge request.</returns>
    Task<TimeStats> TimeStatsAsync(long mergeRequestIid, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the resource label events.
    ///
    /// url like GET /projects/:id/merge_requests/:merge_request_iid/resource_label_events
    ///
    /// </summary>
    /// <param name="projectId">The project id.</param>
    /// <param name="mergeRequestIid">The id of the merge request in the project's scope.</param>
    /// <returns>A collection of the resource label events linked to this merge request.</returns>
    GitLabCollectionResponse<ResourceLabelEvent> ResourceLabelEventsAsync(long projectId, long mergeRequestIid);

    /// <summary>
    /// Gets the resource milestone events.
    ///
    /// url like GET /projects/:id/merge_requests/:merge_request_iid/resource_milestone_events
    ///
    /// </summary>
    /// <param name="projectId">The project id.</param>
    /// <param name="mergeRequestIid">The id of the merge request in the project's scope.</param>
    /// <returns>A collection of the resource milestone events linked to this merge request.</returns>
    GitLabCollectionResponse<ResourceMilestoneEvent> ResourceMilestoneEventsAsync(long projectId, long mergeRequestIid);

    /// <summary>
    /// Gets the resource state events.
    ///
    /// url like GET /projects/:id/merge_requests/:merge_request_iid/resource_state_events
    ///
    /// </summary>
    /// <param name="projectId">The project id.</param>
    /// <param name="mergeRequestIid">The id of the merge request in the project's scope.</param>
    /// <returns>A collection of the resource state events linked to this merge request.</returns>
    GitLabCollectionResponse<ResourceStateEvent> ResourceStateEventsAsync(long projectId, long mergeRequestIid);
}
