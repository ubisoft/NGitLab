using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab.Impl
{
    public class WebhookClient : IWebhookClient
    {
        private readonly API _api;
        private readonly int _projectId;
        private readonly string _projectPath;

        public WebhookClient(API api, int projectId)
        {
            _api = api;
            _projectId = projectId;
            _projectPath = Project.Url + "/" + projectId;
        }

        public IEnumerable<Webhook> AllFromProject => _api.Get().GetAll<Webhook>(_projectPath + "/hooks");

        public Webhook Create(Webhook webhook)
        {
            if (webhook == null)
                throw new System.ArgumentNullException(nameof(webhook));

            return _api
                .Post().With(webhook)
                .To<Webhook>(_projectPath + "/hooks");
        }
    }
}
