using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Impl;

public class LabelClient : ILabelClient
{
    public const string ProjectLabelUrl = "/projects/{0}/labels";
    public const string GroupLabelUrl = "/groups/{0}/labels";

    private readonly API _api;

    public LabelClient(API api)
    {
        _api = api;
    }

    public IEnumerable<Label> ForProject(long projectId)
    {
        return ForProject(projectId, query: null);
    }

    public IEnumerable<Label> ForProject(long projectId, LabelQuery query)
    {
        string url = AddLabelParameterQuery(string.Format(CultureInfo.InvariantCulture, ProjectLabelUrl, projectId), query);

        return _api.Get().GetAll<Label>(url);
    }

    public IEnumerable<Label> ForGroup(long groupId)
    {
        return ForGroup(groupId, query: null);
    }

    public IEnumerable<Label> ForGroup(long groupId, LabelQuery query)
    {
        string url = AddLabelParameterQuery(string.Format(CultureInfo.InvariantCulture, GroupLabelUrl, groupId), query);

        return _api.Get().GetAll<Label>(url);
    }

    public Label GetProjectLabel(long projectId, string name)
    {
        return ForProject(projectId, new LabelQuery() { Search = name }).FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.Ordinal));
    }

    [Obsolete("Use GetProjectLabel instead")]
    public Label GetLabel(long projectId, string name)
    {
        return GetProjectLabel(projectId, name);
    }

    public Label GetGroupLabel(long groupId, string name)
    {
        return ForGroup(groupId, new LabelQuery() { Search = name }).FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.Ordinal));
    }

    public Label CreateProjectLabel(long projectId, ProjectLabelCreate label)
    {
        return _api.Post().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, ProjectLabelUrl, projectId));
    }

    [Obsolete("Use CreateProjectLabel instead")]
    public Label Create(LabelCreate label)
    {
        return CreateProjectLabel(label.Id, new ProjectLabelCreate
        {
            Name = label.Name,
            Color = label.Color,
            Description = label.Description,
        });
    }

    public Label CreateGroupLabel(long groupId, GroupLabelCreate label)
    {
        return _api.Post().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, GroupLabelUrl, groupId));
    }

    [Obsolete("Use other CreateGroupLabel instead")]
    public Label CreateGroupLabel(LabelCreate label)
    {
        return CreateGroupLabel(label.Id, new GroupLabelCreate
        {
            Name = label.Name,
            Color = label.Color,
            Description = label.Description,
        });
    }

    public Label EditProjectLabel(long projectId, ProjectLabelEdit label)
    {
        return _api.Put().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, ProjectLabelUrl, projectId));
    }

    [Obsolete("Use EditProjectLabel instead")]
    public Label Edit(LabelEdit label)
    {
        return EditProjectLabel(label.Id, new ProjectLabelEdit
        {
            Name = label.Name,
            NewName = label.NewName,
            Color = label.Color,
            Description = label.Description,
        });
    }

    public Label EditGroupLabel(long groupId, GroupLabelEdit label)
    {
        return _api.Put().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, GroupLabelUrl, groupId));
    }

    [Obsolete("Use other EditGroupLabel instead")]
    public Label EditGroupLabel(LabelEdit label)
    {
        return EditGroupLabel(label.Id, new GroupLabelEdit
        {
            Name = label.Name,
            NewName = label.NewName,
            Color = label.Color,
            Description = label.Description,
        });
    }

    public Label DeleteProjectLabel(long projectId, ProjectLabelDelete label)
    {
        return _api.Delete().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, ProjectLabelUrl, projectId));
    }

    [Obsolete("Use DeleteProjectLabel instead")]
    public Label Delete(LabelDelete label)
    {
        return DeleteProjectLabel(label.Id, new ProjectLabelDelete
        {
            Id = label.Id,
            Name = label.Name,
        });
    }

    private static string AddLabelParameterQuery(string url, LabelQuery query)
    {
        if (query == null)
        {
            return url;
        }

        url = Utils.AddParameter(url, "with_counts", query.WithCounts);
        url = Utils.AddParameter(url, "per_page", query.PerPage);
        url = Utils.AddParameter(url, "search", query.Search);
        url = Utils.AddParameter(url, "include_ancestor_groups", query.IncludeAncestorGroups);

        return url;
    }
}
