using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NGitLab.Impl;
using NGitLab.Mock.Clients;
using NGitLab.Models;

namespace NGitLab.Mock
{
    internal sealed class MilestoneClient : ClientBase, IMilestoneClient
    {
        private readonly int _resourceId;
        private readonly MilestoneScope _scope;

        public MilestoneClient(ClientContext context, int id, MilestoneScope scope)
            : base(context)
        {
            _resourceId = id;
            _scope = scope;
        }

        public Models.Milestone this[int id]
        {
            get
            {
                using (Context.BeginOperationScope())
                {
                    var project = GetProject(_resourceId, ProjectPermission.View);
                    return FindMilestone(id, project)?.ToClientMilestone();
                }
            }
        }

        public IEnumerable<Models.Milestone> All => Get(new MilestoneQuery());

        public MilestoneScope Scope => throw new NotImplementedException();

        public Models.Milestone Activate(int milestoneId)
        {
            using (Context.BeginOperationScope())
            {
                var milestone = new Milestone();

                if (_scope == MilestoneScope.Groups)
                {
                    var group = GetGroup(_resourceId, GroupPermission.Edit);
                    milestone = FindMilestone(milestoneId, group) ?? throw new GitLabNotFoundException($"Cannot find milestone with ID {milestoneId}");
                }
                else if (_scope == MilestoneScope.Projects)
                {
                    var project = GetProject(_resourceId, ProjectPermission.Edit);
                    milestone = FindMilestone(milestoneId, project) ?? throw new GitLabNotFoundException($"Cannot find milestone with ID {milestoneId}");
                }

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
                var milestone = new Milestone();

                if (_scope == MilestoneScope.Groups)
                {
                    var group = GetGroup(_resourceId, GroupPermission.Edit);
                    milestone = FindMilestone(milestoneId, group) ?? throw new GitLabNotFoundException($"Cannot find milestone with ID {milestoneId}");
                }
                else if (_scope == MilestoneScope.Projects)
                {
                    var project = GetProject(_resourceId, ProjectPermission.Edit);
                    milestone = FindMilestone(milestoneId, project) ?? throw new GitLabNotFoundException($"Cannot find milestone with ID {milestoneId}");
                }

                milestone.State = MilestoneState.closed;
                return milestone.ToClientMilestone();
            }
        }

        public Models.Milestone Create(MilestoneCreate milestone)
        {
            using (Context.BeginOperationScope())
            {
                var ms = new Milestone
                {
                    Title = milestone.Title,
                    Description = milestone.Description,
                    DueDate = string.IsNullOrEmpty(milestone.DueDate) ? DateTimeOffset.UtcNow : DateTimeOffset.Parse(milestone.DueDate),
                    StartDate = string.IsNullOrEmpty(milestone.StartDate) ? DateTimeOffset.UtcNow : DateTimeOffset.Parse(milestone.StartDate),
                };

                if (_scope == MilestoneScope.Groups)
                {
                    var group = GetGroup(_resourceId, GroupPermission.Edit);
                    group.Milestones.Add(ms);
                }
                else if (_scope == MilestoneScope.Projects)
                {
                    var project = GetProject(_resourceId, ProjectPermission.Edit);
                    project.Milestones.Add(ms);
                }

                return ms.ToClientMilestone();
            }
        }

        public void Delete(int milestoneId)
        {
            using (Context.BeginOperationScope())
            {
                if (_scope == MilestoneScope.Groups)
                {
                    var group = GetGroup(_resourceId, GroupPermission.Edit);
                    var milestone = FindMilestone(milestoneId, group) ?? throw new GitLabNotFoundException($"Cannot find milestone with ID {milestoneId}");
                    group.Milestones.Remove(milestone);
                }
                else if (_scope == MilestoneScope.Projects)
                {
                    var project = GetProject(_resourceId, ProjectPermission.Edit);
                    var milestone = FindMilestone(milestoneId, project) ?? throw new GitLabNotFoundException($"Cannot find milestone with ID {milestoneId}");
                    project.Milestones.Remove(milestone);
                }
            }
        }

        public IEnumerable<Models.Milestone> Get(MilestoneQuery query)
        {
            using (Context.BeginOperationScope())
            {
                IEnumerable<Milestone> milestones = new List<Milestone>();

                if (_scope == MilestoneScope.Groups)
                {
                    var group = GetGroup(_resourceId, GroupPermission.Edit);
                    milestones.Concat(group.Milestones);
                }
                else if (_scope == MilestoneScope.Projects)
                {
                    var project = GetProject(_resourceId, ProjectPermission.Edit);
                    milestones.Concat(project.Milestones);
                }

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
                var ms = new Milestone();

                if (_scope == MilestoneScope.Groups)
                {
                    var group = GetGroup(_resourceId, GroupPermission.Edit);
                    ms = FindMilestone(milestoneId, group) ?? throw new GitLabNotFoundException($"Cannot find milestone with ID {milestoneId}");
                }
                else if (_scope == MilestoneScope.Projects)
                {
                    var project = GetProject(_resourceId, ProjectPermission.Edit);
                    ms = FindMilestone(milestoneId, project) ?? throw new GitLabNotFoundException($"Cannot find milestone with ID {milestoneId}");
                }

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
                    ms.DueDate = DateTimeOffset.Parse(milestone.DueDate, CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(milestone.StartDate))
                {
                    ms.StartDate = DateTimeOffset.Parse(milestone.StartDate, CultureInfo.InvariantCulture);
                }

                return ms.ToClientMilestone();
            }
        }

        private static Milestone FindMilestone(int id, Project project)
        {
            return project.Milestones.FirstOrDefault(x => x.Id == id);
        }

        private static Milestone FindMilestone(int id, Group group)
        {
            return group.Milestones.FirstOrDefault(x => x.Id == id);
        }
    }
}
