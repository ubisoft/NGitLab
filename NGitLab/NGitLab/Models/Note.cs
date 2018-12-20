using System;
using System.Runtime.Serialization;

namespace NGitLab.Models
{
    [DataContract]
    public class Note
    {
        [DataMember(Name = "id")]
        public int NoteId;

        [DataMember(Name = "body")]
        public string Body;

        [DataMember(Name = "attachment")]
        public string Attachment;

        [DataMember(Name = "author")]
        public Author Author;

        [DataMember(Name = "created_at")]
        public DateTime CreatedAt;

        [DataMember(Name = "updated_at")]
        public DateTime UpdatedAt;

        [DataMember(Name = "system")]
        public bool System;

        [DataMember(Name = "noteable_id")]
        public int NoteableId;

        [DataMember(Name = "noteable_type")]
        public string NoteableType;

        [DataMember(Name = "noteable_iid")]
        public int Noteable_Iid;

        [DataMember(Name = "resolvable")]
        public bool Resolvable;
    }
}
