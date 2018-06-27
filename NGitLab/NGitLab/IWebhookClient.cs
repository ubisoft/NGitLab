using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IWebhookClient
    {
        IEnumerable<Webhook> AllFromProject { get; }

        Webhook Create(Webhook webhook);

        Webhook Update(Webhook webhook, Webhook existingWebhook);

        bool Delete(int webhookId);

        Webhook GetWebhookById(int webhookId);

        Webhook GetWebhookByUrl(string webhookUrl);
    }
}