using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public Label GetGroupLabel(long groupId, string name)
    {
        return ForGroup(groupId, new LabelQuery() { Search = name }).FirstOrDefault(x => string.Equals(x.Name, name, StringComparison.Ordinal));
    }

    public Label CreateProjectLabel(long projectId, ProjectLabelCreate label)
    {
        return _api.Post().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, ProjectLabelUrl, projectId));
    }

    public Label CreateGroupLabel(long groupId, GroupLabelCreate label)
    {
        return _api.Post().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, GroupLabelUrl, groupId));
    }

    public Label EditProjectLabel(long projectId, ProjectLabelEdit label)
    {
        return _api.Put().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, ProjectLabelUrl, projectId));
    }

    public Label EditGroupLabel(long groupId, GroupLabelEdit label)
    {
        return _api.Put().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, GroupLabelUrl, groupId));
    }

    public Label DeleteProjectLabel(long projectId, ProjectLabelDelete label)
    {
        return _api.Delete().With(label).To<Label>(string.Format(CultureInfo.InvariantCulture, ProjectLabelUrl, projectId));
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
