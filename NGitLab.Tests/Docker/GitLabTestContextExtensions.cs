using System;
using System.IO;
using NGitLab.Models;

namespace NGitLab.Tests.Docker;

public static class GitLabTestContextExtensions
{
    public static Group CreateSubgroup(this GitLabTestContext context, int parentGroupId, Action<GroupCreate> config) =>
        context.CreateGroup(g =>
        {
            g.ParentId = parentGroupId;
            config(g);
        });

    public static Group CreateSubgroup(this GitLabTestContext context, int parentGroupId, string name, string path = null) =>
        context.CreateSubgroup(parentGroupId, g =>
        {
            g.Name = name ?? path ?? g.Name;
            g.Path = path ?? name ?? g.Path;
        });

    public static Project CreateProject(this GitLabTestContext context, int parentGroupId, Action<ProjectCreate> config)
    {
        var name = $"project_{Guid.NewGuid()}";
        var projectCreate = new ProjectCreate()
        {
            NamespaceId = new GroupId(parentGroupId).ValueAsString(),
            Name = name,
            Path = name,
        };
        config?.Invoke(projectCreate);
        return context.Client.Projects.Create(projectCreate);
    }

    public static Project CreateProject(this GitLabTestContext context, int parentGroupId, string name, string path = null) =>
        context.CreateProject(parentGroupId, p =>
        {
            p.Name = name ?? path ?? p.Name;
            p.Path = path ?? name ?? p.Path;
        });
}
