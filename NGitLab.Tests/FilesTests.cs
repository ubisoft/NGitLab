using System;
using System.Threading.Tasks;
using NGitLab.Models;
using NGitLab.Tests.Docker;
using NUnit.Framework;

namespace NGitLab.Tests;

public class FilesTests
{
    [Test]
    [NGitLabRetry]
    public async Task Test_add_update_delete_get_and_exists_file()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var filesClient = context.Client.GetRepository(project.Id).Files;

        // Don't use txt extensions: https://gitlab.com/gitlab-org/gitlab-ce/issues/31790
        var fileName = "test.md";
        var fileUpsert = new FileUpsert
        {
            Branch = project.DefaultBranch,
            CommitMessage = "Add SonarQube badges to README.md",
            RawContent = "test",
            Encoding = "base64",
            Path = fileName,
        };
        filesClient.Create(fileUpsert);

        var exists = filesClient.FileExists(fileName, project.DefaultBranch);
        Assert.That(exists, Is.True);

        var file = filesClient.Get(fileName, project.DefaultBranch);
        Assert.That(file, Is.Not.Null);
        Assert.That(file.Name, Is.EqualTo(fileName));
        Assert.That(file.DecodedContent, Is.EqualTo("test"));

        fileUpsert.RawContent = "test2";
        filesClient.Update(fileUpsert);

        file = filesClient.Get(fileName, project.DefaultBranch);
        Assert.That(file, Is.Not.Null);
        Assert.That(file.DecodedContent, Is.EqualTo("test2"));

        var fileDelete = new FileDelete
        {
            Path = fileName,
            Branch = project.DefaultBranch,
            CommitMessage = "Delete file",
        };
        filesClient.Delete(fileDelete);

        exists = filesClient.FileExists(fileName, project.DefaultBranch);
        Assert.That(exists, Is.False);

        Assert.Throws(Is.InstanceOf<GitLabException>(), () => filesClient.Get("testDelete.md", project.DefaultBranch));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_add_update_delete_get_and_exists_file_async()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var filesClient = context.Client.GetRepository(project.Id).Files;

        // Don't use txt extensions: https://gitlab.com/gitlab-org/gitlab-ce/issues/31790
        var fileName = "test.md";
        var fileUpsert = new FileUpsert
        {
            Branch = project.DefaultBranch,
            CommitMessage = "Add SonarQube badges to README.md",
            RawContent = "test",
            Encoding = "base64",
            Path = fileName,
        };
        await filesClient.CreateAsync(fileUpsert);

        var exists = await filesClient.FileExistsAsync(fileName, project.DefaultBranch);
        Assert.That(exists, Is.True);

        var file = await filesClient.GetAsync(fileName, project.DefaultBranch);
        Assert.That(file, Is.Not.Null);
        Assert.That(file.Name, Is.EqualTo(fileName));
        Assert.That(file.DecodedContent, Is.EqualTo("test"));

        fileUpsert.RawContent = "test2";
        await filesClient.UpdateAsync(fileUpsert);

        file = await filesClient.GetAsync(fileName, project.DefaultBranch);
        Assert.That(file, Is.Not.Null);
        Assert.That(file.DecodedContent, Is.EqualTo("test2"));

        var fileDelete = new FileDelete
        {
            Path = fileName,
            Branch = project.DefaultBranch,
            CommitMessage = "Delete file",
        };
        await filesClient.DeleteAsync(fileDelete);

        exists = await filesClient.FileExistsAsync(fileName, project.DefaultBranch);
        Assert.That(exists, Is.False);

        Assert.ThrowsAsync(Is.InstanceOf<GitLabException>(), () => filesClient.GetAsync("testDelete.md", project.DefaultBranch));
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_blame_of_latest_commit()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var filesClient = context.Client.GetRepository(project.Id).Files;

        var fileName = "blame_test_2.md";
        var content1 = "test";
        var fileUpsert1 = new FileUpsert
        {
            Branch = project.DefaultBranch,
            CommitMessage = "Add SonarQube badges to README.md",
            RawContent = $"{content1}{Environment.NewLine}",
            Encoding = "base64",
            Path = fileName,
        };
        filesClient.Create(fileUpsert1);

        var blameArray1 = filesClient.Blame(fileName, project.DefaultBranch);

        Assert.That(blameArray1, Has.Length.EqualTo(1));
        Assert.That(blameArray1, Is.Not.Null);

        var firstBlameInfo = blameArray1[0];

        Assert.That(string.Join(Environment.NewLine, firstBlameInfo.Lines), Is.EqualTo(content1));
        Assert.That(firstBlameInfo.Commit.Message, Is.EqualTo(fileUpsert1.CommitMessage));
        Assert.That(firstBlameInfo.Commit.CommittedDate, Is.Not.EqualTo(default(DateTime)));
        Assert.That(firstBlameInfo.Commit.AuthorEmail, Is.Not.Empty);
        Assert.That(firstBlameInfo.Commit.AuthorName, Is.Not.Empty);
        Assert.That(firstBlameInfo.Commit.CommitterEmail, Is.Not.Empty);
        Assert.That(firstBlameInfo.Commit.CommitterName, Is.Not.Empty);
        Assert.That(firstBlameInfo.Commit.AuthoredDate, Is.Not.EqualTo(default(DateTime)));

        var content2 = "second line";
        var fileUpsert2 = new FileUpsert
        {
            Branch = project.DefaultBranch,
            CommitMessage = "SecondCommit",
            RawContent = $"{content1}{Environment.NewLine}{content2}",
            Encoding = "base64",
            Path = fileName,
        };
        filesClient.Update(fileUpsert2);

        var blameArray2 = filesClient.Blame(fileName, project.DefaultBranch);

        Assert.That(blameArray2, Has.Length.EqualTo(2));
        Assert.That(blameArray2[0], Is.EqualTo(firstBlameInfo));

        var secondBlameInfo = blameArray2[1];

        Assert.That(string.Join(Environment.NewLine, secondBlameInfo.Lines), Is.EqualTo(content2));
        Assert.That(secondBlameInfo.Commit.Message, Is.EqualTo(fileUpsert2.CommitMessage));
        Assert.That(secondBlameInfo.Commit.CommittedDate, Is.Not.EqualTo(default(DateTime)));
        Assert.That(secondBlameInfo.Commit.AuthorEmail, Is.Not.Empty);
        Assert.That(secondBlameInfo.Commit.AuthorName, Is.Not.Empty);
        Assert.That(secondBlameInfo.Commit.CommitterEmail, Is.Not.Empty);
        Assert.That(secondBlameInfo.Commit.CommitterName, Is.Not.Empty);
        Assert.That(secondBlameInfo.Commit.AuthoredDate, Is.Not.EqualTo(default(DateTime)));

        var fileDelete = new FileDelete
        {
            Path = fileName,
            Branch = project.DefaultBranch,
            CommitMessage = "Delete file",
        };
        filesClient.Delete(fileDelete);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_blame_of_an_old_commit()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var filesClient = context.Client.GetRepository(project.Id).Files;

        var fileName = "blame_test_2.md";
        var content1 = $"test{Environment.NewLine}";
        var fileUpsert1 = new FileUpsert
        {
            Branch = project.DefaultBranch,
            CommitMessage = "Add SonarQube badges to README.md",
            RawContent = content1,
            Encoding = "base64",
            Path = fileName,
        };
        filesClient.Create(fileUpsert1);

        var initialBlame = filesClient.Blame(fileName, project.DefaultBranch);

        Assert.That(initialBlame, Is.Not.Null);
        Assert.That(initialBlame, Has.Length.EqualTo(1));

        var initialBlameInfo = initialBlame[0];

        var content2 = "second line";
        var fileUpsert2 = new FileUpsert
        {
            Branch = project.DefaultBranch,
            CommitMessage = $"SecondCommit{Environment.NewLine}",
            RawContent = $"{content1}{content2}",
            Encoding = "base64",
            Path = fileName,
        };
        filesClient.Update(fileUpsert2);

        var blameById = filesClient.Blame(fileName, initialBlameInfo.Commit.Id.ToString());

        Assert.That(blameById, Has.Length.EqualTo(1));
        Assert.That(blameById[0], Is.EqualTo(initialBlameInfo));

        var fileDelete = new FileDelete
        {
            Path = fileName,
            Branch = project.DefaultBranch,
            CommitMessage = "Delete file",
        };
        filesClient.Delete(fileDelete);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_blame_comparison()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var filesClient = context.Client.GetRepository(project.Id).Files;

        var fileName = "blame_test_3.md";
        var content1 = $"test{Environment.NewLine}";
        var fileUpsert1 = new FileUpsert
        {
            Branch = project.DefaultBranch,
            CommitMessage = "Add SonarQube badges to README.md",
            RawContent = content1,
            Encoding = "base64",
            Path = fileName,
        };
        filesClient.Create(fileUpsert1);

        var realBlame = filesClient.Blame(fileName, project.DefaultBranch);

        Assert.That(realBlame, Is.Not.Null);
        Assert.That(realBlame, Has.Length.EqualTo(1));

        var realBlameInfo = realBlame[0];

        var dummyBlameInfo = new Blame();

        Assert.That(realBlameInfo, Is.Not.EqualTo(dummyBlameInfo));
        Assert.That(realBlameInfo, Is.Not.Null);

        var fileDelete = new FileDelete
        {
            Path = fileName,
            Branch = project.DefaultBranch,
            CommitMessage = "Delete file",
        };
        filesClient.Delete(fileDelete);
    }

    [Test]
    [NGitLabRetry]
    public async Task Test_get_file_with_bom()
    {
        using var context = await GitLabTestContext.CreateAsync();
        var project = context.CreateProject();
        var filesClient = context.Client.GetRepository(project.Id).Files;

        var fileName = "test.md";
        var fileUpsert = new FileUpsert
        {
            Branch = project.DefaultBranch,
            CommitMessage = "dummy",
            Encoding = "base64",
            Content = Convert.ToBase64String(new byte[] { 0xEF, 0xBB, 0xBF, 0x61 }), // UTF8 BOM + 'a'
            Path = fileName,
        };
        filesClient.Create(fileUpsert);

        var file = filesClient.Get(fileName, project.DefaultBranch);
        Assert.That(file.Content, Is.EqualTo("77u/YQ=="));
        Assert.That(file.DecodedContent, Is.EqualTo("a"));
    }
}
