using System.Runtime.Serialization;

namespace NGitLab.Models {
    /// <summary>
    ///     Class for updating a project hook.
    /// </summary>
    [DataContract]
    public class ProjectHookUpdate : ProjectHookInsert {
        [DataMember(Name = "hook_id")]
        public int HookId { get; set; }
    }
}