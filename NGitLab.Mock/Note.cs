using System;

namespace NGitLab.Mock
{
    public abstract class Note : GitLabObject
    {
        protected Note()
        {
            CreatedAt = DateTimeOffset.UtcNow;
            UpdatedAt = DateTimeOffset.UtcNow;
        }

        public long Id { get; set; }

        public string Body { get; set; }

        public UserRef Author { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public bool System { get; set; }

        public abstract int NoticableId { get; }

        public abstract int NoticableIid { get; }

        public abstract string NoteableType { get; }

        public bool Resolvable { get; set; }

        public bool Resolved { get; set; }
    }
}
