using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NGitLab.Models;
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

        [Test, Timeout(10000)]
        public void Test_add_existing_webhook()
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

            var webhooks = _webhooks.AllFromProject.Where(x => x.WebhookUrl == url).ToList();

            Assert.AreEqual(1, webhooks.Count);
        }

        [Test, Timeout(10000)]
        public void Test_update_existing_webhook()
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
            const bool enableSslVerification = true;

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
                WikiPageEvents = wikiPageEvents,
                EnableSslVerification = enableSslVerification
            });

            _webhooks.Create(new Models.Webhook
            {
                WebhookUrl = url,
                MergeRequestsEvents = false,
                PushEvents = pushEvents,
                TagPushEvents = tagPushEvents,
                IssuesEvents = issuesEvents,
                JobEvents = jobEvents,
                PipelineEvents = false,
                NoteEvents = noteEvents,
                WikiPageEvents = wikiPageEvents,
                EnableSslVerification = false
            });

            var webhook = _webhooks.AllFromProject.First(x => x.WebhookUrl == url);

            Assert.AreEqual(false, webhook.MergeRequestsEvents);
            Assert.AreEqual(false, webhook.PipelineEvents);
        }

        [Test, Timeout(10000)]
        public void Test_delete_existing_webhook()
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
            const bool enableSslVerification = true;

            Webhook whook = _webhooks.Create(new Models.Webhook
            {
                WebhookUrl = url,
                MergeRequestsEvents = mergeRequestsEvents,
                PushEvents = pushEvents,
                TagPushEvents = tagPushEvents,
                IssuesEvents = issuesEvents,
                JobEvents = jobEvents,
                PipelineEvents = pipelineEvents,
                NoteEvents = noteEvents,
                WikiPageEvents = wikiPageEvents,
                EnableSslVerification = enableSslVerification
            });

            bool success = _webhooks.Delete(whook.Id);

            var webhook = _webhooks.AllFromProject.FirstOrDefault(x => x.WebhookUrl == url);

            Assert.AreEqual(true, success);
            Assert.AreEqual(null, webhook);
        }

        [Test, Timeout(10000)]
        public void Test_get_existing_webhook_by_id()
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
            const bool enableSslVerification = true;

            Webhook whook = _webhooks.Create(new Models.Webhook
            {
                WebhookUrl = url,
                MergeRequestsEvents = mergeRequestsEvents,
                PushEvents = pushEvents,
                TagPushEvents = tagPushEvents,
                IssuesEvents = issuesEvents,
                JobEvents = jobEvents,
                PipelineEvents = pipelineEvents,
                NoteEvents = noteEvents,
                WikiPageEvents = wikiPageEvents,
                EnableSslVerification = enableSslVerification
            });

            var webhook = _webhooks.GetWebhookById(whook.Id);

            Assert.IsNotNull(webhook);
            Assert.AreEqual("http://test-webhook:9999/", webhook.WebhookUrl);
            Assert.AreEqual(true, webhook.MergeRequestsEvents);
            Assert.AreEqual(true, webhook.PushEvents);
            Assert.AreEqual(true, webhook.TagPushEvents);
            Assert.AreEqual(true, webhook.IssuesEvents);
            Assert.AreEqual(true, webhook.JobEvents);
            Assert.AreEqual(true, webhook.PipelineEvents);
            Assert.AreEqual(true, webhook.NoteEvents);
            Assert.AreEqual(true, webhook.WikiPageEvents);
            Assert.AreEqual(true, webhook.EnableSslVerification);
        }


        [Test, Timeout(10000)]
        public void Test_get_existing_webhook_by_url()
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
            const bool enableSslVerification = true;

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
                WikiPageEvents = wikiPageEvents,
                EnableSslVerification = enableSslVerification
            });

            var webhook = _webhooks.GetWebhookByUrl(url);

            Assert.IsNotNull(webhook);
            Assert.AreEqual("http://test-webhook:9999/", webhook.WebhookUrl);
            Assert.AreEqual(true, webhook.MergeRequestsEvents);
            Assert.AreEqual(true, webhook.PushEvents);
            Assert.AreEqual(true, webhook.TagPushEvents);
            Assert.AreEqual(true, webhook.IssuesEvents);
            Assert.AreEqual(true, webhook.JobEvents);
            Assert.AreEqual(true, webhook.PipelineEvents);
            Assert.AreEqual(true, webhook.NoteEvents);
            Assert.AreEqual(true, webhook.WikiPageEvents);
            Assert.AreEqual(true, webhook.EnableSslVerification);
        }

        [Test, Timeout(10000)]
        public void Test_get_not_existing_webhook_by_id()
        {
            var webhook = _webhooks.GetWebhookById(3454);

            Assert.IsNull(webhook);
        }

        [Test, Timeout(10000)]
        public void Test_get_not_existing_webhook_by_url()
        {
            var webhook = _webhooks.GetWebhookByUrl("http://test-webhook:9999");

            Assert.IsNull(webhook);
        }

        [Test, Timeout(10000)]
        public void Test_delete_not_existing_webhook_by_url()
        {
            bool success = _webhooks.Delete(459);

            Assert.AreEqual(false, success);
        }

        [TearDown]
        public void TearDown()
        {
            _webhooks = null;
        }
    }
}
