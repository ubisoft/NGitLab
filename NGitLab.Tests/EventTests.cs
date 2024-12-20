using System;
using System.Linq;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class EventTests
{
    [Test]
    [NGitLabRetry]
    public async Task Test_get_user_events_works()
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();

        var currentUserId = context.Client.Users.Current.Id;
        var userEventClient = context.Client.GetUserEvents(currentUserId);

        // Act
        var firstEvent = userEventClient.Get(new EventQuery { After = DateTime.UtcNow.AddMonths(-1) }).FirstOrDefault();

        // Assert
        Assert.That(firstEvent, Is.Not.Null);
        Assert.That(firstEvent.AuthorId, Is.EqualTo(currentUserId));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_global_events_works()
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();

        var globalEventClient = context.Client.GetEvents();

        // Act
        var firstEvent = globalEventClient.Get(new EventQuery { After = DateTime.UtcNow.AddMonths(-1) }).FirstOrDefault();

        // Assert
        Assert.That(firstEvent, Is.Not.Null);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_events_of_specific_action_type()
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();

        var issueClient = context.Client.Issues;
        var issueTitle = $"Temporary Issue {Guid.NewGuid()}";
        var issue = issueClient.Create(new IssueCreate
        {
            ProjectId = project.Id,
            Title = issueTitle,
        });

        issueClient.Edit(new IssueEdit
        {
            ProjectId = project.Id,
            IssueId = issue.IssueId,
            State = "close",
        });

        var currentUserId = context.Client.Users.Current.Id;
        var userEventClient = context.Client.GetUserEvents(currentUserId);

        // Act
        var closedEvents = userEventClient.Get(new EventQuery { Action = EventAction.Closed }).ToArray();

        // Assert
        Assert.That(closedEvents.All(e => e.Action.EnumValue is EventAction.Closed), Is.True);

        var issueClosedEvent = closedEvents.SingleOrDefault(e => string.Equals(e.TargetTitle, issueTitle, StringComparison.Ordinal));
        Assert.That(issueClosedEvent, Is.Not.Null);
        Assert.That(issueClosedEvent.TargetType.EnumValue, Is.EqualTo(EventTargetType.Issue));
    }
}
