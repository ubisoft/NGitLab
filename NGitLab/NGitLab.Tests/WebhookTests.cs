using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class WebhookTests
    {
        private IWebhookClient _webhooks;

        [SetUp]
        public void FixtureSetup()
        {
            _webhooks = Initialize.GitLabClient.GetWebhook(Initialize.UnitTestProject.Id);
        }

        [Test, Timeout(10000)]
        public void Test_can_create_webhooks_for_project()
        {
            const string url = "http://test-webhook:9999/";
            const bool mergeRequestsEvents = true;
            const bool pushEvents = true;
            const bool tagPushEvents = true;
            const bool issuesEvents = true;
            const bool jobEvents = true;
            const bool pipelineEvents = true;
            const bool noteEvents = true;
            const bool wikiPageEvents = true;

            _webhooks.Create(new Models.Webhook
            {
                WebhookUrl = url,
                MergeRequestsEvents = mergeRequestsEvents,
                PushEvents = pushEvents,
                TagPushEvents = tagPushEvents,
                IssuesEvents = issuesEvents,
                JobEvents = jobEvents,
                PipelineEvents = pipelineEvents,
                NoteEvents = noteEvents,
                WikiPageEvents = wikiPageEvents
            });

            var webhook = _webhooks.AllFromProject.FirstOrDefault(x => x.WebhookUrl == url);

            Assert.IsNotNull(webhook);
            Assert.AreEqual(true, webhook.MergeRequestsEvents);
            Assert.AreEqual(true, webhook.PushEvents);
            Assert.AreEqual(true, webhook.TagPushEvents);
            Assert.AreEqual(true, webhook.WikiPageEvents);
            Assert.AreEqual(true, webhook.IssuesEvents);
            Assert.AreEqual(true, webhook.JobEvents);
            Assert.AreEqual(true, webhook.NoteEvents);
            Assert.AreEqual(true, webhook.PipelineEvents);
        }
    }
}
