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
    IEnumerable<Label> ForProject(int projectId);

    /// <summary>
    /// Return a list of labels for a group.
    /// </summary>
    /// <param name="groupId"></param>
    /// <returns></returns>
    IEnumerable<Label> ForGroup(int groupId);

    /// <summary>
    /// Return a specified label from the project or null;
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Label GetProjectLabel(int projectId, string name);

    [EditorBrowsable(EditorBrowsableState.Never)]
    Label GetLabel(int projectId, string name);

    /// <summary>
    /// Return a specified label from the group or null;
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    Label GetGroupLabel(int groupId, string name);

    /// <summary>
    /// Create a new label for a project.
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    Label CreateProjectLabel(int projectId, ProjectLabelCreate label);

    [EditorBrowsable(EditorBrowsableState.Never)]
    Label Create(LabelCreate label);

    /// <summary>
    /// Create a new label for a group.
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    Label CreateGroupLabel(int groupId, GroupLabelCreate label);

    [EditorBrowsable(EditorBrowsableState.Never)]
    Label CreateGroupLabel(LabelCreate label);

    /// <summary>
    /// Edit the contents of an existing project label.
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    Label EditProjectLabel(int projectId, ProjectLabelEdit label);

    [EditorBrowsable(EditorBrowsableState.Never)]
    Label Edit(LabelEdit label);

    /// <summary>
    /// Edit the contents of an existing label.
    /// </summary>
    /// <param name="groupId"></param>
    /// <param name="label"></param>
    /// <returns></returns>
    Label EditGroupLabel(int groupId, GroupLabelEdit label);

    [EditorBrowsable(EditorBrowsableState.Never)]
    Label EditGroupLabel(LabelEdit label);

    /// <summary>
    /// Delete a label from the project.
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="label"></param>
    /// <returns>True if "200", the success code for delete, was returned from the service.</returns>
    Label DeleteProjectLabel(int projectId, ProjectLabelDelete label);

    [EditorBrowsable(EditorBrowsableState.Never)]
    Label Delete(LabelDelete label);
}
