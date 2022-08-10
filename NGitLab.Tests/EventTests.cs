using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class EventTests
    {
        [Test]
        [NGitLabRetry]
        public async Task Test_get_user_events_works()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();

            var currentUserId = context.Client.Users.Current.Id;
            var userEvents = context.Client.GetUserEvents(currentUserId);
            var globalEvents = context.Client.GetEvents();

            var firstEvent = userEvents.Get(new EventQuery { After = DateTime.UtcNow.AddMonths(-1) }).FirstOrDefault();

            if (firstEvent != null)
            {
                Assert.That(firstEvent.AuthorId, Is.EqualTo(currentUserId));
            }
        }

        [Test]
        [NGitLabRetry]
        public async Task Test_get_global_events_works()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();

            var currentUserId = context.Client.Users.Current.Id;
            var globalEvents = context.Client.GetEvents();

            var firstEvent = globalEvents.Get(new EventQuery { After = DateTime.UtcNow.AddMonths(-1) }).FirstOrDefault();

            Assert.That(firstEvent, Is.Not.Null);
        }
    }
}
