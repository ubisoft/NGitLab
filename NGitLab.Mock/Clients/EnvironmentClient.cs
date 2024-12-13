using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Mock.Internals;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class EnvironmentClient : ClientBase, IEnvironmentClient
{
    private readonly long _projectId;

    public EnvironmentClient(ClientContext context, ProjectId projectId)
        : base(context)
    {
        _projectId = Server.AllProjects.FindProject(projectId.ValueAsString()).Id;
    }

    public IEnumerable<EnvironmentInfo> All => throw new NotImplementedException();

    public EnvironmentInfo Create(string name, string externalUrl)
    {
        throw new NotImplementedException();
    }

    public void Delete(long environmentId)
    {
        throw new NotImplementedException();
    }

    public EnvironmentInfo Edit(long environmentId, string externalUrl) => Edit(environmentId, null, externalUrl);

    [EditorBrowsable(EditorBrowsableState.Never)]
    public EnvironmentInfo Edit(long environmentId, string name, string externalUrl)
    {
        throw new NotImplementedException();
    }

    public EnvironmentInfo Stop(long environmentId)
    {
        throw new NotImplementedException();
    }

    public GitLabCollectionResponse<EnvironmentInfo> GetEnvironmentsAsync(EnvironmentQuery query)
    {
        return GitLabCollectionResponse.Create(Array.Empty<EnvironmentInfo>());
    }

    public EnvironmentInfo GetById(long environmentId)
    {
        throw new NotImplementedException();
    }

    public Task<EnvironmentInfo> GetByIdAsync(long environmentId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
