using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests.Milestone
{
    public class MilestoneClientTests
    {
        private IMilestoneClient _milestoneClient;

        [SetUp]
        public void Setup()
        {
            _milestoneClient = Initialize.GitLabClient.GetMilestone(Initialize.UnitTestProject.Id);
        }

        [Test]
        public void Test_milestone_api()
        {
            var milestone = CreateMilestone("my-super-milestone");

            Assert.AreEqual(milestone.Id, _milestoneClient[milestone.Id].Id, "Test we can get a milestone by Id");
            Assert.IsTrue(_milestoneClient.All.Any(x => x.Id == milestone.Id), "Test 'All' accessor returns the milestone");
            Assert.IsTrue(_milestoneClient.AllInState(MilestoneState.active).Any(x => x.Id == milestone.Id), "Can return all active milestone");

            milestone = UpdateMilestone(milestone);

            milestone = UpdatePartialMilestone(milestone);

            milestone = CloseMilestone(milestone);

            Assert.IsTrue(_milestoneClient.AllInState(MilestoneState.closed).Any(x => x.Id == milestone.Id), "Can return all closed milestone");

            milestone = ActivateMilestone(milestone);

            DeleteMilestone(milestone);
        }

        private Models.Milestone CreateMilestone(string title)
        {
            var milestone = _milestoneClient.Create(new MilestoneCreate
            {
                Title = title,
                Description = $"{title} description",
                StartDate = "2017-08-20",
                DueDate = "2017-09-20",
            });

            Assert.That(milestone, Is.Not.Null);
            Assert.That(milestone.Title, Is.EqualTo(title));
            Assert.That(milestone.Description, Is.EqualTo($"{title} description"));
            Assert.That(milestone.StartDate, Is.EqualTo("2017-08-20"));
            Assert.That(milestone.DueDate, Is.EqualTo("2017-09-20"));

            return milestone;
        }

        private Models.Milestone UpdateMilestone(Models.Milestone milestone)
        {
            var updatedMilestone = _milestoneClient.Update(milestone.Id, new MilestoneUpdate
            {
                Title = milestone.Title + "new",
                Description = milestone.Description + "new",
                StartDate = "2018-08-20",
                DueDate = "2018-09-20",
            });

            Assert.That(updatedMilestone, Is.Not.Null);
            Assert.That(updatedMilestone.Title, Is.EqualTo(milestone.Title + "new"));
            Assert.That(updatedMilestone.Description, Is.EqualTo(milestone.Description + "new"));
            Assert.That(updatedMilestone.StartDate, Is.EqualTo("2018-08-20"));
            Assert.That(updatedMilestone.DueDate, Is.EqualTo("2018-09-20"));
            Assert.That(updatedMilestone.State, Is.EqualTo(milestone.State));

            return updatedMilestone;
        }

        private Models.Milestone UpdatePartialMilestone(Models.Milestone milestone)
        {
            var updatedMilestone = _milestoneClient.Update(milestone.Id, new MilestoneUpdate
            {
                Description = milestone.Description + "partial new",
            });

            Assert.That(updatedMilestone, Is.Not.Null);
            Assert.That(updatedMilestone.Title, Is.EqualTo(milestone.Title));
            Assert.That(updatedMilestone.Description, Is.EqualTo(milestone.Description + "partial new"));
            Assert.That(updatedMilestone.StartDate, Is.EqualTo(milestone.StartDate));
            Assert.That(updatedMilestone.DueDate, Is.EqualTo(milestone.DueDate));
            Assert.That(updatedMilestone.State, Is.EqualTo(milestone.State));

            return updatedMilestone;
        }

        private Models.Milestone ActivateMilestone(Models.Milestone milestone)
        {
            var closedMilestone = _milestoneClient.Activate(milestone.Id);

            Assert.That(closedMilestone.State, Is.EqualTo(nameof(MilestoneState.active)));
            Assert.That(closedMilestone.Title, Is.EqualTo(milestone.Title));
            Assert.That(closedMilestone.Description, Is.EqualTo(milestone.Description));
            Assert.That(closedMilestone.StartDate, Is.EqualTo(milestone.StartDate));
            Assert.That(closedMilestone.DueDate, Is.EqualTo(milestone.DueDate));

            return closedMilestone;
        }

        private Models.Milestone CloseMilestone(Models.Milestone milestone)
        {
            var closedMilestone = _milestoneClient.Close(milestone.Id);

            Assert.That(closedMilestone.State, Is.EqualTo(nameof(MilestoneState.closed)));
            Assert.That(closedMilestone.Title, Is.EqualTo(milestone.Title));
            Assert.That(closedMilestone.Description, Is.EqualTo(milestone.Description));
            Assert.That(closedMilestone.StartDate, Is.EqualTo(milestone.StartDate));
            Assert.That(closedMilestone.DueDate, Is.EqualTo(milestone.DueDate));

            return closedMilestone;
        }

        private void DeleteMilestone(Models.Milestone milestone)
        {
            _milestoneClient.Delete(milestone.Id);

            Assert.Throws<GitLabException>(() =>
            {
                var ms = _milestoneClient[milestone.Id];
            });
        }
    }
}
