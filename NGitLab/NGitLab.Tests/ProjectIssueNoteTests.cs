using System;
using System.Linq;
using Castle.Core.Internal;
using NGitLab.Models;
using NUnit.Framework;
using System.Collections.Generic;

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
        public void CreateListEdit()
        {
            var noteCreate = new ProjectIssueNoteCreate()
            {
                IssueId = Initialize.UnitTestIssue.IssueId,
                Body = "Issue Body"
            };

            var createdNote = _client.Create(noteCreate);

            Assert.That(createdNote.Body, Is.EqualTo(noteCreate.Body));

            var listedNote = _client.ForIssue(Initialize.UnitTestIssue.IssueId).ToList();
            Assert.That(listedNote.Count, Is.EqualTo(1));
            Assert.That(listedNote.First().Body, Is.EqualTo(noteCreate.Body));


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