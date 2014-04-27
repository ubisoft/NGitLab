using System;
using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public class API
    {
        private readonly string _hostUrl;
        private readonly string _apiToken;
        private const string APINamespace = "/api/v3";

        private API(string hostUrl, string apiToken)
        {
            IsIgnoreCertificateErrors = false;
            _hostUrl = hostUrl.EndsWith("/") ? hostUrl.Replace("/$", "") : hostUrl;
            _apiToken = apiToken;
        }

        public static Session Connect(string hostUrl, string username, string password)
        {
            const string tailUrl = Session.Url;
            var api = Connect(hostUrl, null);
            return api.Dispatch().With("login", username).With("password", password)
                .To<Session>(tailUrl);
        }

        public static API Connect(string hostUrl, string apiToken)
        {
            return new API(hostUrl, apiToken);
        }

        public API IgnoreCertificateErrors(bool ignoreCertificateErrors)
        {
            IsIgnoreCertificateErrors = ignoreCertificateErrors;
            return this;
        }

        private HttpRequestor Retrieve()
        {
            return new HttpRequestor(this);
        }

        public HttpRequestor Dispatch()
        {
            return new HttpRequestor(this).Method(HttpRequestor.MethodType.Post);
        }

        public bool IsIgnoreCertificateErrors { get; private set; }

        public Uri GetAPIUrl(string tailAPIUrl)
        {
            if (_apiToken != null)
            {
                tailAPIUrl = tailAPIUrl + (tailAPIUrl.IndexOf('?') > 0 ? '&' : '?') + "private_token=" + _apiToken;
            }

            if (!tailAPIUrl.StartsWith("/"))
            {
                tailAPIUrl = "/" + tailAPIUrl;
            }
            return new Uri(_hostUrl + APINamespace + tailAPIUrl);
        }

        public Uri GetUrl(string tailAPIUrl)
        {
            if (!tailAPIUrl.StartsWith("/"))
            {
                tailAPIUrl = "/" + tailAPIUrl;
            }

            return new Uri(_hostUrl + tailAPIUrl);
        }

        public Project GetProject(int projectId)
        {
            return Retrieve().To<Project>(Project.Url + "/" + projectId);
        }

        public IEnumerable<Project> GetProjects()
        {
            return Retrieve().GetAll<Project>(Project.Url);
        }

        //    public List<MergeRequest> GetMergeRequests(int projectId, MergeRequestState? state=null) 
        //    {
        //        return fetchMergeRequests(Project.Url + "/" + projectId + MergeRequest.Url);
        //    }

        //    public List<MergeRequest> GetMergeRequests(Project project, MergeRequestState? state=null)
        //    {
        //        return GetMergeRequests(project.Id, state);
        //    }

        //    public MergeRequest GetMergeRequest(Project project, int mergeRequestId)
        //    {
        //        return Retrieve().To<MergeRequest>(Project.Url + "/" + project.Id + "/merge_request/" + mergeRequestId);
        //    }

        //    public List<Commit> getCommits(GitlabMergeRequest mergeRequest) throws IOException {
        //        int projectId = mergeRequest.getSourceProjectId();
        //    if (projectId == null) {
        //        projectId = mergeRequest.getProjectId();
        //    }
        //        string tailUrl = GitlabProject.URL + "/" + projectId +
        //                         "/repository" + GitlabCommit.URL + "?ref_name=" + mergeRequest.getSourceBranch();

        //        GitlabCommit[] commits = retrieve().to(tailUrl, GitlabCommit[].class);
        //    return Arrays.asList(commits);
        //    }

        //    public GitlabNote createNote(GitlabMergeRequest mergeRequest, string body) throws IOException {
        //        string tailUrl = GitlabProject.URL + "/" + mergeRequest.getProjectId() +
        //                         GitlabMergeRequest.URL + "/" + mergeRequest.getId() + GitlabNote.URL;

        //    return dispatch().with("body", body).to(tailUrl, GitlabNote.class);
        //    }

        //    public List<GitlabBranch> getBranches(GitlabProject project) throws IOException {
        //        string tailUrl = GitlabProject.URL + "/" + project.getId() + GitlabBranch.URL;
        //        GitlabBranch[] branches = retrieve().to(tailUrl, GitlabBranch[].class);
        //    return Arrays.asList(branches);
        //    }

        //    public GitlabBranch getBranch(GitlabProject project, string branchName) throws IOException {
        //        string tailUrl = GitlabProject.URL + "/" + project.getId() + GitlabBranch.URL + branchName;
        //        GitlabBranch branch = retrieve().to(tailUrl, GitlabBranch.class);
        //    return branch;
        //    }

        //    public void protectBranch(GitlabProject project, string branchName) throws IOException {
        //        string tailUrl = GitlabProject.URL + "/" + project.getId() + GitlabBranch.URL + branchName + "/protect";
        //        retrieve().method("PUT").to(tailUrl, Void.class);
        //    }

        //    public void unprotectBranch(GitlabProject project, string branchName) throws IOException {
        //        string tailUrl = GitlabProject.URL + "/" + project.getId() + GitlabBranch.URL + branchName + "/unprotect";
        //        retrieve().method("PUT").to(tailUrl, Void.class);
        //    }

        //    public List<GitlabProjectHook> getProjectHooks(GitlabProject project) throws IOException {
        //        string tailUrl = GitlabProject.URL + "/" + project.getId() + GitlabProjectHook.URL;
        //        GitlabProjectHook[] hooks = retrieve().to(tailUrl, GitlabProjectHook[].class);
        //    return Arrays.asList(hooks);
        //    }

        //    public GitlabProjectHook getProjectHook(GitlabProject project, string hookId) throws IOException {
        //        string tailUrl = GitlabProject.URL + "/" + project.getId() + GitlabProjectHook.URL + "/" + hookId;
        //        GitlabProjectHook hook = retrieve().to(tailUrl, GitlabProjectHook.class);
        //    return hook;
        //    }

        //    public GitlabProjectHook addProjectHook(GitlabProject project, string url) throws IOException {
        //        string tailUrl = GitlabProject.URL + "/" + project.getId() + GitlabProjectHook.URL + "?url=" + URLEncoder.encode(url, "UTF-8");
        //    return dispatch().to(tailUrl, GitlabProjectHook.class);
        //    }

        //    public GitlabProjectHook editProjectHook(GitlabProject project, string hookId, string url) throws IOException {
        //        string tailUrl = GitlabProject.URL + "/" + project.getId() + GitlabProjectHook.URL + "/" + hookId + "?url=" + URLEncoder.encode(url, "UTF-8");
        //    return retrieve().method("PUT").to(tailUrl, GitlabProjectHook.class);
        //    }

        //    public void deleteProjectHook(GitlabProject project, string hookId) throws IOException {
        //        string tailUrl = GitlabProject.URL + "/" + project.getId() + GitlabProjectHook.URL + "/" + hookId;
        //        retrieve().method("DELETE").to(tailUrl, Void.class);
        //    }

        //    private List<GitlabMergeRequest> fetchMergeRequests(string tailUrl) throws IOException {
        //        GitlabMergeRequest[] mergeRequests = retrieve().to(tailUrl, GitlabMergeRequest[].class);
        //    return Arrays.asList(mergeRequests);
        //    }

        //    public List<GitlabIssue> getIssues(GitlabProject project) throws IOException {
        //        string tailUrl = GitlabProject.URL + "/" + project.getId() + GitlabIssue.URL;
        //    return retrieve().getAll(tailUrl, GitlabIssue[].class);
        //    }

        //    public GitlabIssue getIssue(int projectId, int issueId) throws IOException {
        //        string tailUrl = GitlabProject.URL + "/" + projectId + GitlabIssue.URL + "/" + issueId;
        //    return retrieve().to(tailUrl, GitlabIssue.class);
        //    }

        //    public GitlabIssue createIssue(int projectId, int assigneeId, int milestoneId, string labels, 
        //    string description, string title) throws IOException {
        //        string tailUrl = GitlabProject.URL + "/" + projectId + GitlabIssue.URL;
        //        HttpRequestor requestor = dispatch();
        //        applyIssue(requestor, projectId, assigneeId, milestoneId, labels, description, title);

        //    return requestor.to(tailUrl, GitlabIssue.class);
        //    }

        //    public GitlabIssue editIssue(int projectId, int issueId, int assigneeId, int milestoneId, string labels,
        //    string description, string title, GitlabIssue.Action action) throws IOException {
        //        string tailUrl = GitlabProject.URL + "/" + projectId + GitlabIssue.URL + "/" + issueId;
        //        HttpRequestor requestor = retrieve().method("PUT");
        //        applyIssue(requestor, projectId, assigneeId, milestoneId, labels, description, title);

        //    if(action != GitlabIssue.Action.LEAVE) {
        //        requestor.with("state_event", action.tostring().toLowerCase());
        //    }

        //    return requestor.to(tailUrl, GitlabIssue.class);
        //}

        //    private void applyIssue(HttpRequestor requestor, int projectId,
        //    int assigneeId, int milestoneId, string labels, string description,
        //    string title) {

        //    requestor.with("title", title)
        //    .with("description", description)
        //    .with("labels", labels)
        //    .with("milestone_id", milestoneId);

        //    if(assigneeId != 0) {
        //    requestor.with("assignee_id", assigneeId == -1 ? 0 : assigneeId);
        //    }
        //    }

        //    public List<GitlabNote> getNotes(GitlabIssue issue) throws IOException {
        //    string tailUrl = GitlabProject.URL + "/" + issue.getProjectId() + GitlabIssue.URL + "/" 
        //    + issue.getId() + GitlabNote.URL;
        //    return Arrays.asList(retrieve().to(tailUrl, GitlabNote[].class));
        //    }

        //    public GitlabNote createNote(int projectId, int issueId, string message) throws IOException {
        //    string tailUrl = GitlabProject.URL + "/" + projectId + GitlabIssue.URL
        //    + "/" + issueId + GitlabNote.URL;
        //    return dispatch().with("body", message).to(tailUrl, GitlabNote.class);
        //    }

        //    public GitlabNote createNote(GitlabIssue issue, string message) throws IOException {
        //    return createNote(issue.getProjectId(), issue.getId(), message);
        //    }

        //    public List<GitlabMilestone> getMilestones(GitlabProject project) throws IOException {
        //    return getMilestones(project.getId());
        //    }

        //    public List<GitlabMilestone> getMilestones(int projectId) throws IOException {
        //    string tailUrl = GitlabProject.URL + "/" + projectId + GitlabMilestone.URL;
        //    return Arrays.asList(retrieve().to(tailUrl, GitlabMilestone[].class));
        //    }

        //    public List<GitlabProjectMember> getProjectMembers(GitlabProject project) throws IOException {
        //    return getProjectMembers(project.getId());
        //    }

        //    public List<GitlabProjectMember> getProjectMembers(int projectId) throws IOException {
        //    string tailUrl = GitlabProject.URL + "/" + projectId + GitlabProjectMember.URL;
        //    return Arrays.asList(retrieve().to(tailUrl, GitlabProjectMember[].class));
        //    }

        //    /**
        //     * This will fail, if the given namespace is a user and not a group
        //     * @param namespace
        //     * @return
        //     * @throws IOException
        //     */
        //    public List<GitlabProjectMember> getNamespaceMembers(GitlabNamespace namespace) throws IOException {
        //    return getNamespaceMembers(namespace.getId());
        //    }

        //    /**
        //     * This will fail, if the given namespace is a user and not a group
        //     * @param namespaceId
        //     * @return
        //     * @throws IOException
        //     */
        //    public List<GitlabProjectMember> getNamespaceMembers(int namespaceId) throws IOException {
        //    string tailUrl = GitlabNamespace.URL + "/" + namespaceId + GitlabProjectMember.URL;
        //    return Arrays.asList(retrieve().to(tailUrl, GitlabProjectMember[].class));
        //    }

        public Session GetCurrentSession()
        {
            return Retrieve().To<Session>("/user");
        }
    }
}