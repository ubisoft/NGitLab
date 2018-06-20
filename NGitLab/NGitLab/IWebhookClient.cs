using System.Collections.Generic;
using NGitLab.Models;

namespace NGitLab
{
    public interface IWebhookClient
    {
        IEnumerable<Webhook> AllFromProject { get; }

        Webhook Create(Webhook webhook);
    }
}