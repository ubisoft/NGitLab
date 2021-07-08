using System;
using System.Collections.Generic;
using System.Linq;
using NGitLab.Mock.Clients;
using NGitLab.Models;

namespace NGitLab.Mock
{
    internal sealed class MilestoneClient : ClientBase, IMilestoneClient
    {
        private readonly int _projectId;

        public MilestoneClient(ClientContext context, int projectId)
            : base(context)
        {
            _projectId = projectId;
        }

        public Models.Milestone this[int id]
        {
            get
            {
                using (Context.BeginOperationScope())
                {
                    var project = GetProject(_projectId, ProjectPermission.View);
                    return FindMilestone(id, project)?.ToClientMilestone();
                }
            }
        }

        public IEnumerable<Models.Milestone> All => Get(new MilestoneQuery());

        public Models.Milestone Activate(int milestoneId)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Edit);
                var milestone = FindMilestone(milestoneId, project) ?? throw new GitLabNotFoundException($"Cannot find milestone with ID {milestoneId}");
                milestone.State = MilestoneState.active;
                return milestone.ToClientMilestone();
            }
        }

        public IEnumerable<Models.Milestone> AllInState(Models.MilestoneState state)
        {
            return Get(new MilestoneQuery { State = state });
        }

        public Models.Milestone Close(int milestoneId)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Edit);
                var milestone = FindMilestone(milestoneId, project) ?? throw new GitLabNotFoundException($"Cannot find milestone with ID {milestoneId}");
                milestone.State = MilestoneState.closed;
                return milestone.ToClientMilestone();
            }
        }

        public Models.Milestone Create(MilestoneCreate milestone)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Edit);
                var ms = new Milestone
                {
                    Title = milestone.Title,
                    Description = milestone.Description,
                    DueDate = string.IsNullOrEmpty(milestone.DueDate) ? DateTimeOffset.UtcNow : DateTimeOffset.Parse(milestone.DueDate),
                    StartDate = string.IsNullOrEmpty(milestone.StartDate) ? DateTimeOffset.Now : DateTimeOffset.Parse(milestone.StartDate),
                };
                project.Milestones.Add(ms);
                return ms.ToClientMilestone();
            }
        }

        public void Delete(int milestoneId)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Edit);
                var milestone = FindMilestone(milestoneId, project) ?? throw new GitLabNotFoundException($"Cannot find milestone with ID {milestoneId}");
                project.Milestones.Remove(milestone);
            }
        }

        public IEnumerable<Models.Milestone> Get(MilestoneQuery query)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.View);
                IEnumerable<Milestone> milestones = project.Milestones;
                if (query.State != null)
                {
                    milestones = milestones.Where(m => (int)m.State == (int)query.State);
                }

                if (!string.IsNullOrEmpty(query.Search))
                {
                    milestones = milestones.Where(m => m.Title.Contains(query.Search, StringComparison.OrdinalIgnoreCase));
                }

                return milestones.Select(m => m.ToClientMilestone());
            }
        }

        public Models.Milestone Update(int milestoneId, MilestoneUpdate milestone)
        {
            using (Context.BeginOperationScope())
            {
                var project = GetProject(_projectId, ProjectPermission.Edit);
                var ms = FindMilestone(milestoneId, project) ?? throw new GitLabNotFoundException($"Cannot find milestone with ID {milestoneId}");
                if (!string.IsNullOrEmpty(milestone.Title))
                {
                    ms.Title = milestone.Title;
                }

                if (milestone.Description != null)
                {
                    ms.Description = milestone.Description;
                }

                if (!string.IsNullOrEmpty(milestone.DueDate))
                {
                    ms.DueDate = DateTimeOffset.Parse(milestone.DueDate);
                }

                if (!string.IsNullOrEmpty(milestone.StartDate))
                {
                    ms.StartDate = DateTimeOffset.Parse(milestone.StartDate);
                }

                return ms.ToClientMilestone();
            }
        }

        private static Milestone FindMilestone(int id, Project project)
        {
            return project.Milestones.FirstOrDefault(x => x.Id == id);
        }
    }
}
