using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using NGitLab.Models;

namespace NGitLab.Mock
{
    public sealed class Release : GitLabObject
    {
        public Release()
        {
        }

        public Project Project => (Project)Parent;

        public string TagName;

        public string Description;

        public string Name;

        public DateTime CreatedAt = DateTime.UtcNow;

        public DateTime ReleasedAt = DateTime.UtcNow;

        public UserRef Author;

        public Commit Commit;

        public string CommitPath;

        public string TagPath;

        internal Models.ReleaseInfo ToReleaseClient()
        {
            return new Models.ReleaseInfo
            {
                TagName = TagName,
                Name = Name,
                Description = Description,
                CreatedAt = CreatedAt,
                ReleasedAt = ReleasedAt,
                Author = Author.ToClientAuthor(),
                Commit = Commit,
                CommitPath = CommitPath,
                TagPath = TagPath,
            };
        }
    }
}
