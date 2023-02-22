using System.Collections.Generic;
using System.Globalization;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class DeploymentClient : IDeploymentClient
    {
        private const string ProjectDeploymentsUrl = "/projects/{0}/deployments";
        private const string DeploymentMergeRequestsUrl = "/projects/{0}/deployments/{1}/merge_requests";

        private readonly API _api;

        public DeploymentClient(API api)
        {
            _api = api;
        }

        public IEnumerable<Deployment> Get(int projectId, DeploymentQuery query)
        {
            var url = QueryStringHelper.BuildAndAppendQueryString(string.Format(ProjectDeploymentsUrl, projectId), query);
            return _api.Get().GetAll<Deployment>(url);
        }

        public IEnumerable<MergeRequest> GetMergeRequests(int projectId, int deploymentId)
        {
            return _api.Get().GetAll<MergeRequest>(string.Format(CultureInfo.InvariantCulture, DeploymentMergeRequestsUrl, projectId, deploymentId));
        }
    }
}
