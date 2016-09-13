using NGitLab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NGitLab
{
    public interface ILabelClient
    {
        /// <summary>
        /// Return a list of labels for a project.
        /// </summary>
        /// <param name="projectId"></param>
        /// <returns></returns>
        IEnumerable<Label> ForProject(int projectId);

        /// <summary>
        /// Return a specified label from the project or null;
        /// 
        /// </summary>
        /// <param name="projectId"></param>
        /// <param name="Name"></param>
        /// <returns></returns>
        Label GetLabel(int projectId, string Name);

        /// <summary>
        /// Create a new label for a project.
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        Label Create(LabelCreate label);

        /// <summary>
        /// Edit the contents of an existing label.
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        Label Edit(LabelEdit label);

        /// <summary>
        /// Delete a label from the project.
        /// 
        /// </summary>
        /// <param name="label"></param>
        /// <param name="result"></param>
        /// <returns>True if "200", the success code for delete, was returned from the service.</returns>
        Label Delete(LabelDelete label);
    }
}
