using System.Linq;
using NGitLab.Models;
using NUnit.Framework;

namespace NGitLab.Tests
{
    public class ProjectIssueNoteTests
    {
        private readonly IProjectIssueNoteClient _client;

        public ProjectIssueNoteTests()
        {
            _client = Initialize.GitLabClient.GetProjectIssueNoteClient(Initialize.UnitTestProject.Id);
        }

        [Test]
        public void CreateNote()
        {
            var noteCreate = new ProjectIssueNoteCreate()
            {
                IssueId = Initialize.UnitTestIssue.IssueId,
                Body = "Issue Body"
            };

            var createdNote = _client.Create(noteCreate);

            Assert.That(createdNote.Body, Is.EqualTo(noteCreate.Body));
        }

        [Test]
        public void ListNotes()
        {
            var createdIssue = Initialize.GitLabClient.Issues.Create(new IssueCreate()
            {
                Id = Initialize.UnitTestProject.Id,
                Title = "title",
                Description = "desc",
            });

            var noteCreate = new ProjectIssueNoteCreate()
            {
                IssueId = createdIssue.IssueId,
                Body = "Issue Body"
            };

            _client.Create(noteCreate);
            _client.Create(noteCreate);

            var listedNote = _client.ForIssue(createdIssue.IssueId).ToList();
            // Gitlab create additional issue notes about initial manipulation on the issue,
            // so a starting count greater than one is expected.  Those issue notes have the 'system'
            // boolean set to true.
            // We validate that there is only two issues that don't have the 'system' flag.
            Assert.That(listedNote.Where(x => !x.System).Count, Is.EqualTo(2));
            Assert.That(listedNote[0].Body, Is.EqualTo(noteCreate.Body));
        }

        [Test]
        public void GetNote()
        {
            var noteCreate = new ProjectIssueNoteCreate()
            {
                IssueId = Initialize.UnitTestIssue.IssueId,
                Body = "Issue Body"
            };

            var createdNote = _client.Create(noteCreate);

            var getNote = _client.Get(Initialize.UnitTestIssue.IssueId, createdNote.NoteId);
            Assert.That(getNote.Body, Is.EqualTo(noteCreate.Body));
        }

        [Test]
        public void EditNote()
        {
            var noteCreate = new ProjectIssueNoteCreate()
            {
                IssueId = Initialize.UnitTestIssue.IssueId,
                Body = "Issue Body"
            };

            var createdNote = _client.Create(noteCreate);

            Assert.That(createdNote.Body, Is.EqualTo(noteCreate.Body));

            var noteEdit = new ProjectIssueNoteEdit()
            {
                IssueId = Initialize.UnitTestIssue.IssueId,
                NoteId = createdNote.NoteId,
                Body = "Modified Issue Body"
            };
            var editedNote = _client.Edit(noteEdit);
            Assert.That(editedNote.Body, Is.EqualTo(noteEdit.Body));
        }
    }
}
