using System;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class LabelClientTests
{
    [Test]
    [NGitLabRetry]
    public async Task CreateProjectLabel()
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var labelClient = context.Client.Labels;
        var labelName = Guid.NewGuid().ToString();

        // Act
        var createdLabel = labelClient.CreateProjectLabel(project.Id, new ProjectLabelCreate { Name = labelName, Color = "blue" });

        // Assert
        Assert.That(createdLabel, Is.Not.Null);
        Assert.That(createdLabel.Name, Is.EqualTo(labelName));
        Assert.That(createdLabel.Color, Is.EqualTo("#0000FF").IgnoreCase);
    }

    [Test]
    [NGitLabRetry]
    public async Task GetProjectLabel()
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var labelClient = context.Client.Labels;
        var labelName = Guid.NewGuid().ToString();

        labelClient.CreateProjectLabel(project.Id, new ProjectLabelCreate { Name = labelName, Color = "#ff0099" });

        // Act
        var rightLabel = labelClient.GetProjectLabel(project.Id, labelName);
        var wrongLabel = labelClient.GetProjectLabel(project.Id, "NotMyLabel");

        // Assert
        Assert.That(rightLabel, Is.Not.Null);
        Assert.That(rightLabel.Name, Is.EqualTo(labelName));
        Assert.That(rightLabel.Color, Is.EqualTo("#ff0099").IgnoreCase);

        Assert.That(wrongLabel, Is.Null);
    }

    [Test]
    [NGitLabRetry]
    public async Task DeleteProjectLabelById()
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var labelClient = context.Client.Labels;
        var labelName = Guid.NewGuid().ToString();

        var createdLabel = labelClient.CreateProjectLabel(project.Id, new ProjectLabelCreate { Name = labelName, Color = "#ff0099" });

        // Act
        await labelClient.DeleteProjectLabelAsync(project.Id, createdLabel.Id);

        // Assert
        Assert.That(labelClient.GetProjectLabel(project.Id, labelName), Is.Null);
    }

    [Test]
    [NGitLabRetry]
    public async Task DeleteProjectLabelByName()
    {
        // Arrange
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var labelClient = context.Client.Labels;
        var labelName = Guid.NewGuid().ToString();

        var createdLabel = labelClient.CreateProjectLabel(project.Id, new ProjectLabelCreate { Name = labelName, Color = "#ff0099" });

        // Act
        await labelClient.DeleteProjectLabelAsync(project.Id, createdLabel.Name);

        // Assert
        Assert.That(labelClient.GetProjectLabel(project.Id, labelName), Is.Null);
    }
}
