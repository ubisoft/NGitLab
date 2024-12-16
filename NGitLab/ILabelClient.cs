using System.Collections.Generic;
using System.ComponentModel;
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
    /// Return a list of labels for a group.
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    IEnumerable<Label> ForGroup(long groupId);

    /// <summary>
    /// Return a specified label from the project or null;
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Label GetProjectLabel(long projectId, string name);

    [EditorBrowsable(EditorBrowsableState.Never)]
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

    [EditorBrowsable(EditorBrowsableState.Never)]
    Label Create(LabelCreate label);

    /// <summary>
    /// Create a new label for a group.
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    Label CreateGroupLabel(long groupId, GroupLabelCreate label);

    [EditorBrowsable(EditorBrowsableState.Never)]
    Label CreateGroupLabel(LabelCreate label);

    /// <summary>
    /// Edit the contents of an existing project label.
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    Label EditProjectLabel(long projectId, ProjectLabelEdit label);

    [EditorBrowsable(EditorBrowsableState.Never)]
    Label Edit(LabelEdit label);

    /// <summary>
    /// Edit the contents of an existing label.
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    Label EditGroupLabel(long groupId, GroupLabelEdit label);

    [EditorBrowsable(EditorBrowsableState.Never)]
    Label EditGroupLabel(LabelEdit label);

    /// <summary>
    /// Delete a label from the project.
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="label"></param>
    /// <returns>True if "200", the success code for delete, was returned from the service.</returns>
    Label DeleteProjectLabel(long projectId, ProjectLabelDelete label);

    [EditorBrowsable(EditorBrowsableState.Never)]
    Label Delete(LabelDelete label);
}
