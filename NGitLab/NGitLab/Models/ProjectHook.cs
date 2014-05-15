using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class ProjectHook
    {
        [DataMember(Name = "id")]
        public int Id;

        [DataMember(Name = "url")]
        public Uri Url;

        [DataMember(Name = "project_id")]
        public int ProjectId; 
        
        [DataMember(Name = "push_events")]
        public bool PushEvents;
        
        [DataMember(Name = "merge_requests_events")]
        public bool MergeRequestsEvents;
        
        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;
    }
}