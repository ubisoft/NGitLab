using System;
using System.Linq;

namespace NGitLab.Mock;

public sealed class MergeRequestCollection : Collection<MergeRequest>
{
    private Project Project => (Project)Parent;

    public MergeRequestCollection(GitLabObject parent)
        : base(parent)
    {
    }

    public MergeRequest GetByIid(long iid)
    {
        return this.FirstOrDefault(mr => mr.Iid == iid);
    }

    public MergeRequest AddNew(User author, string title = null, bool addCommit = false)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            title = Guid.NewGuid().ToString();
        }

        var branch = Project.Repository.CreateBranch(Guid.NewGuid().ToString(), Project.DefaultBranch);
        if (addCommit)
        {
            Project.Repository.Commit(author, title, branch.CanonicalName, new[] { File.CreateFromText("test.txt", "test") });
        }

        return Add(branch.FriendlyName, Project.DefaultBranch, title, author);
    }

    public override void Add(MergeRequest mergeRequest)
    {
        if (mergeRequest is null)
            throw new ArgumentNullException(nameof(mergeRequest));

        if (mergeRequest.Id == default)
        {
            mergeRequest.Id = Server.GetNewMergeRequestId();
        }

        if (mergeRequest.Iid == default)
        {
            mergeRequest.Iid = GetNewIid();
        }
        else if (GetByIid(mergeRequest.Iid) != null)
        {
            throw new GitLabException("Merge request already exists");
        }

        base.Add(mergeRequest);
    }

    public MergeRequest Add(string sourceBranch, string targetBranch, string title, User user)
    {
        return Add(Project, sourceBranch, targetBranch, title, user);
    }

    public MergeRequest Add(Project sourceProject, string sourceBranch, string targetBranch, string title, User user)
    {
        if (sourceProject is null)
            throw new ArgumentNullException(nameof(sourceProject));
        if (sourceBranch is null)
            throw new ArgumentNullException(nameof(sourceBranch));
        if (targetBranch is null)
            throw new ArgumentNullException(nameof(targetBranch));
        if (title is null)
            throw new ArgumentNullException(nameof(title));
        if (user is null)
            throw new ArgumentNullException(nameof(user));

        var mergeRequest = new MergeRequest
        {
            Author = user,
            SourceBranch = sourceBranch,
            SourceProject = sourceProject,
            TargetBranch = targetBranch,
            Title = title,
        };

        mergeRequest.UpdateSha();
        Add(mergeRequest);
        return mergeRequest;
    }

    private long GetNewIid()
    {
        return this.Select(mr => mr.Iid).DefaultIfEmpty().Max() + 1;
    }
}
