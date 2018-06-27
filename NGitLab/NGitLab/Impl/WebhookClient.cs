using System.Collections.Generic;
using System.Linq;
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

            var existingWebhook = GetWebhookByUrl(webhook.WebhookUrl);
            if (existingWebhook != null)
            {
                return Update(webhook, existingWebhook);
            }

            return _api
                .Post().With(webhook)
                .To<Webhook>(_projectPath + "/hooks");
        }

        public Webhook Update(Webhook webhook, Webhook existingWebhook)
        {
            if (!webhook.Equals(existingWebhook))
            {
                return _api
                    .Put().With(webhook)
                    .To<Webhook>(_projectPath + "/hooks/" + existingWebhook.Id);
            }

            return null;
        }

        public bool Delete(int webhookId)
        {
            Webhook webhook = GetWebhookById(webhookId);
            if (webhook != null)
            {
                _api.Delete()
                    .Execute(_projectPath + "/hooks/" + webhookId);
                return true;
            }

            return false;
        }

        public Webhook GetWebhookById(int webhookId)
        {
            try
            {
                return _api.Get()
                    .To<Webhook>(_projectPath + "/hooks/" + webhookId);
            }
            catch (GitLabException e) when (e.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public Webhook GetWebhookByUrl(string webhookUrl)
        {
            var allWebhooksFromProject = AllFromProject;
            return allWebhooksFromProject.FirstOrDefault(x => x.WebhookUrl == webhookUrl);
        }
    }
}
