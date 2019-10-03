using System;
using System.Net;
using LibGit2Sharp;
using NGitLab.Models;

namespace NGitLab.Mock.Clients
{
    internal sealed class FileClient : ClientBase, IFilesClient
    {
        private readonly int _projectId;

        public FileClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        public void Create(FileUpsert file)
        {
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var commitCreate = new CommitCreate
            {
                Branch = file.Branch,
                CommitMessage = file.CommitMessage,
                AuthorEmail = Context.User.Email,
                AuthorName = Context.User.Name,
                ProjectId = _projectId,
                Actions = new[]
                {
                    new CreateCommitAction()
                    {
                        Action = CommitAction.Create.ToString(),
                        Content = file.Content,
                        Encoding = file.Encoding,
                        FilePath = file.Path,
                    },
                },
            };

            project.Repository.Commit(commitCreate);
        }

        public void Delete(FileDelete file)
        {
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var commitCreate = new CommitCreate
            {
                Branch = file.Branch,
                CommitMessage = file.CommitMessage,
                AuthorEmail = Context.User.Email,
                AuthorName = Context.User.Name,
                ProjectId = _projectId,
                Actions = new[]
                {
                    new CreateCommitAction()
                    {
                        Action = CommitAction.Delete.ToString(),
                        FilePath = file.Path,
                    },
                },
            };

            project.Repository.Commit(commitCreate);
        }

        public FileData Get(string filePath, string @ref)
        {
            var fileSystemPath = WebUtility.UrlDecode(filePath);

            var project = GetProject(_projectId, ProjectPermission.View);
            return project.Repository.GetFile(fileSystemPath, @ref);
        }

        public void Update(FileUpsert file)
        {
            var project = GetProject(_projectId, ProjectPermission.Contribute);
            var commitCreate = new CommitCreate
            {
                Branch = file.Branch,
                CommitMessage = file.CommitMessage,
                AuthorEmail = Context.User.Email,
                AuthorName = Context.User.Name,
                ProjectId = _projectId,
                Actions = new[]
                {
                    new CreateCommitAction()
                    {
                        Action = CommitAction.Update.ToString(),
                        Content = file.Content,
                        Encoding = file.Encoding,
                        FilePath = file.Path,
                    },
                },
            };

            project.Repository.Commit(commitCreate);
        }
    }
}
