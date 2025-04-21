using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using NGitLab.Models;

namespace NGitLab;

public interface ILabelClient
{
    /// <summary>
    /// Return a list of labels for a project.
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    IEnumerable<Label> ForProject(long projectId);

    /// <summary>
    /// Return a list of labels for a project.
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
    IEnumerable<Label> ForProject(long projectId, LabelQuery labelQuery);

    /// <summary>
    /// Return a list of labels for a group.
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    IEnumerable<Label> ForGroup(long groupId);

    /// <summary>
    /// Return a list of labels for a group.
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    IEnumerable<Label> ForGroup(long groupId, LabelQuery labelQuery);

    /// <summary>
    /// Return a specified label from the project or null;
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Label GetProjectLabel(long projectId, string name);

    [Obsolete("Use GetProjectLabel instead")]
    Label GetLabel(long projectId, string name);

    /// <summary>
    /// Return a specified label from the group or null;
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Label GetGroupLabel(long groupId, string name);

    /// <summary>
    /// Create a new label for a project.
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    Label CreateProjectLabel(long projectId, ProjectLabelCreate label);

    [Obsolete("Use CreateProjectLabel instead")]
    Label Create(LabelCreate label);

    /// <summary>
    /// Create a new label for a group.
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    Label CreateGroupLabel(long groupId, GroupLabelCreate label);

    [Obsolete("Use other CreateGroupLabel instead")]
    Label CreateGroupLabel(LabelCreate label);

    /// <summary>
    /// Edit the contents of an existing project label.
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    Label EditProjectLabel(long projectId, ProjectLabelEdit label);

    [Obsolete("Use EditProjectLabel instead")]
    Label Edit(LabelEdit label);

    /// <summary>
    /// Edit the contents of an existing label.
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    Label EditGroupLabel(long groupId, GroupLabelEdit label);

    [Obsolete("Use other EditGroupLabel instead")]
    Label EditGroupLabel(LabelEdit label);

    [Obsolete("Use DeleteProjectLabelAsync instead")]
    Label DeleteProjectLabel(long projectId, ProjectLabelDelete label);

    /// <summary>
    /// Delete a project label using its ID.
    /// </summary>
    /// <param name="projectId">Project ID</param>
    /// <param name="labelId">Label ID</param>
    [SuppressMessage("ApiDesign", "RS0026:Do not add multiple public overloads with optional parameters", Justification = "Internal requirement to have the CancellationToken optional")]
    Task DeleteProjectLabelAsync(long projectId, long labelId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Delete a project label using its name.
    /// </summary>
    /// <param name="projectId">Project ID</param>
    /// <param name="labelName">Label Name</param>
    [SuppressMessage("ApiDesign", "RS0026:Do not add multiple public overloads with optional parameters", Justification = "Internal requirement to have the CancellationToken optional")]
    Task DeleteProjectLabelAsync(long projectId, string labelName, CancellationToken cancellationToken = default);

    [Obsolete("Use DeleteProjectLabelAsync instead")]
    Label Delete(LabelDelete label);
}
