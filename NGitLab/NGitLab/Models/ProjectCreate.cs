using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace NGitLab.Models {
    /// <summary>
    ///     Creates a new project owned by the authenticated user.
    /// </summary>
    [DataContract]
    public class ProjectCreate {
        /// <summary>
        ///     Gets or sets the name of the project.
        /// </summary>
        [Required]
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        ///     Gets or sets the namespace for the project.
        /// </summary>
        [DefaultValue(0)]
        [DataMember(Name = "namespace_id",EmitDefaultValue =false,IsRequired =false)]
        public int NamespaceId { get; set; }

        /// <summary>
        ///     Gets or sets the short project description.
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "path")]
        public string Path { get; set; }

        [DataMember(Name = "issues_enabled")]
        public bool IssuesEnabled { get; set; }

        [DataMember(Name = "wall_enabled")]
        public bool WallEnabled { get; set; }

        [DataMember(Name = "merge_requests_enabled")]
        public bool MergeRequestsEnabled { get; set; }

        [DataMember(Name = "snippets_enabled")]
        public bool SnippetsEnabled { get; set; }

        [DataMember(Name = "wiki_enabled")]
        public bool WikiEnabled { get; set; }

        [DataMember(Name = "jobs_enabled")]
        public bool JobsEnabled { get; set; }

        [DataMember(Name = "container_registry_enabled")]
        public bool ContainerRegistryEnabled { get; set; }

        [DataMember(Name = "shared_runners_enabled")]
        public bool SharedRunnersEnabled { get; set; }

        [DataMember(Name = "import_url")]
        public string ImportUrl { get; set; }

        [DataMember(Name = "visibility_level")]
        public VisibilityLevel VisibilityLevel { get; set; }

        [DataMember(Name = "public_jobs")]
        public bool PublicJobs { get; set; }

        [DataMember(Name = "only_allow_merge_if_pipeline_succeeds")]
        public bool OnlyAllowMergeIfPipelineSucceeds { get; set; }

        [DataMember(Name = "only_allow_merge_if_all_discussions_are_resolved")]
        public bool OnlyAllowMergeIfAllDiscussionsAreResolved { get; set; }

        [DataMember(Name = "lfs_enabled")]
        public bool LfsEnabled { get; set; }

        [DataMember(Name = "request_access_enabled")]
        public bool RequestAccessEnabled { get; set; }

        [DataMember(Name = "tag_list")]
        public string[] TagList { get; set; }
    }
}