using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab
{
    public interface IMergeRequestClient
    {
        IEnumerable<MergeRequest> All { get; }

        IEnumerable<MergeRequest> AllInState(MergeRequestState state);

        IEnumerable<MergeRequest> Get(MergeRequestQuery query);

        MergeRequest this[int iid] { get; }

        Task<MergeRequest> GetByIidAsync(int iid, SingleMergeRequestQuery options, CancellationToken cancellationToken = default);

        MergeRequest Create(MergeRequestCreate mergeRequest);

        MergeRequest Update(int mergeRequestIid, MergeRequestUpdate mergeRequest);

        MergeRequest Close(int mergeRequestIid);

        MergeRequest Reopen(int mergeRequestIid);

        void Delete(int mergeRequestIid);

        MergeRequest CancelMergeWhenPipelineSucceeds(int mergeRequestIid);

        [Obsolete("You should use MergeRequestMerge instead of MergeRequestAccept")]
        MergeRequest Accept(int mergeRequestIid, MergeRequestAccept message);

        MergeRequest Accept(int mergeRequestIid, MergeRequestMerge message);

        MergeRequest Approve(int mergeRequestIid, MergeRequestApprove message);

        RebaseResult Rebase(int mergeRequestIid);

        Task<RebaseResult> RebaseAsync(int mergeRequestIid, MergeRequestRebase options, CancellationToken cancellationToken = default);

        IEnumerable<PipelineBasic> GetPipelines(int mergeRequestIid);

        IEnumerable<Author> GetParticipants(int mergeRequestIid);

        GitLabCollectionResponse<MergeRequestVersion> GetVersionsAsync(int mergeRequestIid);

        IMergeRequestCommentClient Comments(int mergeRequestIid);

        IMergeRequestDiscussionClient Discussions(int mergeRequestIid);

        IMergeRequestCommitClient Commits(int mergeRequestIid);

        IMergeRequestChangeClient Changes(int mergeRequestIid);

        IMergeRequestApprovalClient ApprovalClient(int mergeRequestIid);

        IEnumerable<Issue> ClosesIssues(int mergeRequestIid);

        /// <summary>
        /// Gets the resource label events.
        ///
        /// url like GET /projects/:id/merge_requests/:merge_request_iid/resource_label_events
        ///
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="mergeRequestIid">The id of the merge request in the project's scope.</param>
        /// <returns>A collection of the resource label events linked to this merge request.</returns>
        IEnumerable<ResourceLabelEvent> ResourceLabelEvents(int projectId, int mergeRequestIid);

        GitLabCollectionResponse<ResourceLabelEvent> ResourceLabelEventsAsync(int projectId, int mergeRequestIid);

        /// <summary>
        /// Gets the resource milestone events.
        ///
        /// url like GET /projects/:id/merge_requests/:merge_request_iid/resource_milestone_events
        ///
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="mergeRequestIid">The id of the merge request in the project's scope.</param>
        /// <returns>A collection of the resource milestone events linked to this merge request.</returns>
        IEnumerable<ResourceMilestoneEvent> ResourceMilestoneEvents(int projectId, int mergeRequestIid);

        GitLabCollectionResponse<ResourceMilestoneEvent> ResourceMilestoneEventsAsync(int projectId, int mergeRequestIid);

        /// <summary>
        /// Gets the resource state events.
        ///
        /// url like GET /projects/:id/merge_requests/:merge_request_iid/resource_state_events
        ///
        /// </summary>
        /// <param name="projectId">The project id.</param>
        /// <param name="mergeRequestIid">The id of the merge request in the project's scope.</param>
        /// <returns>A collection of the resource state events linked to this merge request.</returns>
        IEnumerable<ResourceStateEvent> ResourceStateEvents(int projectId, int mergeRequestIid);

        GitLabCollectionResponse<ResourceStateEvent> ResourceStateEventsAsync(int projectId, int mergeRequestIid);
    }
}
