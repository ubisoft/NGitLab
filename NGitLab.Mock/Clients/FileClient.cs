using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab.Mock.Clients;

internal sealed class FileClient : ClientBase, IFilesClient
{
    private readonly long _projectId;

    public FileClient(ClientContext context, long projectId)
        : base(context)
    {
        _projectId = projectId;
    }

    public void Create(FileUpsert file)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var commitCreate = new CommitCreate
            {
                Branch = file.Branch,
                CommitMessage = file.CommitMessage,
                AuthorEmail = Context.User.Email,
                AuthorName = Context.User.Name,
                Actions = new[]
                {
                    new CreateCommitAction
                    {
                        Action = nameof(CommitAction.Create),
                        Content = file.Content,
                        Encoding = file.Encoding,
                        FilePath = file.Path,
                    },
                },
            };

            project.Repository.Commit(commitCreate);
        }
    }

    public async Task CreateAsync(FileUpsert file, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        Create(file);
    }

    public void Delete(FileDelete file)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var commitCreate = new CommitCreate
            {
                Branch = file.Branch,
                CommitMessage = file.CommitMessage,
                AuthorEmail = Context.User.Email,
                AuthorName = Context.User.Name,
                Actions = new[]
                {
                    new CreateCommitAction
                    {
                        Action = nameof(CommitAction.Delete),
                        FilePath = file.Path,
                    },
                },
            };

            project.Repository.Commit(commitCreate);
        }
    }

    public async Task DeleteAsync(FileDelete file, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        Delete(file);
    }

    public FileData Get(string filePath, string @ref)
    {
        using (Context.BeginOperationScope())
        {
            var fileSystemPath = WebUtility.UrlDecode(filePath);

            var project = GetProject(_projectId, ProjectPermission.View);
            return project.Repository.GetFile(fileSystemPath, @ref);
        }
    }

    public bool FileExists(string filePath, string @ref)
    {
        using (Context.BeginOperationScope())
        {
            try
            {
                return Get(filePath, @ref) != null;
            }
            catch (GitLabNotFoundException)
            {
                return false;
            }
        }
    }
    public async Task<bool> FileExistsAsync(string filePath, string @ref, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return FileExists(filePath, @ref);
    }

    public Blame[] Blame(string filePath, string @ref)
    {
        throw new NotImplementedException();
    }

    public void Update(FileUpsert file)
    {
        using (Context.BeginOperationScope())
        {
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var commitCreate = new CommitCreate
            {
                Branch = file.Branch,
                CommitMessage = file.CommitMessage,
                AuthorEmail = Context.User.Email,
                AuthorName = Context.User.Name,
                Actions = new[]
                {
                    new CreateCommitAction
                    {
                        Action = nameof(CommitAction.Update),
                        Content = file.Content,
                        Encoding = file.Encoding,
                        FilePath = file.Path,
                    },
                },
            };

            project.Repository.Commit(commitCreate);
        }
    }

    public async Task UpdateAsync(FileUpsert file, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        Update(file);
    }

    public async Task<FileData> GetAsync(string filePath, string @ref, CancellationToken cancellationToken = default)
    {
        await Task.Yield();
        return Get(filePath, @ref);
    }
}
