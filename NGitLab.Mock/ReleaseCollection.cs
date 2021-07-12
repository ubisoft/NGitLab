using System;
using System.Linq;

namespace NGitLab.Mock
{
    public class ReleaseCollection : Collection<ReleaseInfo>
    {
        private Project Project => (Project)Parent;

        public ReleaseCollection(GitLabObject parent)
            : base(parent)
        {
        }

        public ReleaseInfo GetByTagName(string tagName)
        {
            return this.FirstOrDefault(r => r.TagName.Equals(tagName));
        }

        public override void Add(ReleaseInfo release)
        {
            if (release is null)
                throw new ArgumentNullException(nameof(release));

            if (release.TagName == default || Project.Repository.GetTags().FirstOrDefault(t => t.FriendlyName.Equals(release.TagName, StringComparison.Ordinal)) == null)
            {
                throw new ArgumentException($"Tag '{release.TagName}' invalid or not found", nameof(release));
            }

            if (release.Name == default)
            {
                release.Name = release.TagName;
            }

            if (GetByTagName(release.TagName) != null)
            {
                throw new GitLabException("Release already exists");
            }

            base.Add(release);
        }

        public ReleaseInfo Add(string tagName, string name, string description, User user)
        {
            if (tagName is null)
                throw new ArgumentNullException(nameof(tagName));
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            var release = new ReleaseInfo
            {
                TagName = tagName,
                Name = name,
                Description = description,
                Author = user,
            };

            Add(release);
            return release;
        }
    }
}
