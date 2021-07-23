using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests.Milestone
{
    public class MilestoneClientTests
    {
        [Test]
        [NGitLabRetry]
        public async Task Test_milestone_api()
        {
            using var context = await GitLabTestContext.CreateAsync();
            var project = context.CreateProject();
            var milestoneClient = context.Client.GetMilestone(project.Id);

            var milestone = CreateMilestone(context, project, "my-super-milestone");

            Assert.AreEqual(milestone.Id, milestoneClient[milestone.Id].Id, "Test we can get a milestone by Id");
            Assert.IsTrue(milestoneClient.All.Any(x => x.Id == milestone.Id), "Test 'All' accessor returns the milestone");
            Assert.IsTrue(milestoneClient.AllInState(MilestoneState.active).Any(x => x.Id == milestone.Id), "Can return all active milestone");

            milestone = UpdateMilestone(context, project, milestone);

            milestone = UpdatePartialMilestone(context, project, milestone);

            milestone = CloseMilestone(context, project, milestone);

            Assert.IsTrue(milestoneClient.AllInState(MilestoneState.closed).Any(x => x.Id == milestone.Id), "Can return all closed milestone");

            milestone = ActivateMilestone(context, project, milestone);

            DeleteMilestone(context, project, milestone);
        }

        private static Models.Milestone CreateMilestone(GitLabTestContext context, Project project, string title)
        {
            var milestoneClient = context.Client.GetMilestone(project.Id);
            var milestone = milestoneClient.Create(new MilestoneCreate
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

        private static Models.Milestone UpdateMilestone(GitLabTestContext context, Project project, Models.Milestone milestone)
        {
            var milestoneClient = context.Client.GetMilestone(project.Id);
            var updatedMilestone = milestoneClient.Update(milestone.Id, new MilestoneUpdate
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

        private static Models.Milestone UpdatePartialMilestone(GitLabTestContext context, Project project, Models.Milestone milestone)
        {
            var milestoneClient = context.Client.GetMilestone(project.Id);
            var updatedMilestone = milestoneClient.Update(milestone.Id, new MilestoneUpdate
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

        private static Models.Milestone ActivateMilestone(GitLabTestContext context, Project project, Models.Milestone milestone)
        {
            var milestoneClient = context.Client.GetMilestone(project.Id);
            var closedMilestone = milestoneClient.Activate(milestone.Id);

            Assert.That(closedMilestone.State, Is.EqualTo(nameof(MilestoneState.active)));
            Assert.That(closedMilestone.Title, Is.EqualTo(milestone.Title));
            Assert.That(closedMilestone.Description, Is.EqualTo(milestone.Description));
            Assert.That(closedMilestone.StartDate, Is.EqualTo(milestone.StartDate));
            Assert.That(closedMilestone.DueDate, Is.EqualTo(milestone.DueDate));

            return closedMilestone;
        }

        private static Models.Milestone CloseMilestone(GitLabTestContext context, Project project, Models.Milestone milestone)
        {
            var milestoneClient = context.Client.GetMilestone(project.Id);
            var closedMilestone = milestoneClient.Close(milestone.Id);

            Assert.That(closedMilestone.State, Is.EqualTo(nameof(MilestoneState.closed)));
            Assert.That(closedMilestone.Title, Is.EqualTo(milestone.Title));
            Assert.That(closedMilestone.Description, Is.EqualTo(milestone.Description));
            Assert.That(closedMilestone.StartDate, Is.EqualTo(milestone.StartDate));
            Assert.That(closedMilestone.DueDate, Is.EqualTo(milestone.DueDate));

            return closedMilestone;
        }

        private static void DeleteMilestone(GitLabTestContext context, Project project, Models.Milestone milestone)
        {
            var milestoneClient = context.Client.GetMilestone(project.Id);
            milestoneClient.Delete(milestone.Id);

            Assert.Throws<GitLabException>(() =>
            {
                var ms = milestoneClient[milestone.Id];
            });
        }
    }
}
