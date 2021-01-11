using System;
using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class EventTests
    {
        private IEventClient _userEvents;
        private IEventClient _globalEvents;
        private int _currentUserId;

        [OneTimeSetUp]
        public void FixtureSetup()
        {
            var gitlabClient = Initialize.GitLabClient;
            _currentUserId = gitlabClient.Users.Current.Id;
            _userEvents = gitlabClient.GetUserEvents(_currentUserId);
            _globalEvents = gitlabClient.GetEvents();
        }

        [Test]
        public void Test_get_user_events_works()
        {
            var firstEvent = _userEvents.Get(new EventQuery { After = DateTime.Now.AddMonths(-1) }).FirstOrDefault();

            if (firstEvent != null)
            {
                Assert.That(firstEvent.AuthorId, Is.EqualTo(_currentUserId));
            }
        }

        [Test]
        public void Test_get_global_events_works()
        {
            var firstEvent = _globalEvents.Get(new EventQuery { After = DateTime.Now.AddMonths(-1) }).FirstOrDefault();

            Assert.That(firstEvent, Is.Not.Null);
        }
    }
}
