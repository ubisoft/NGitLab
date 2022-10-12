using System;
using NGitLab.Models;

namespace NGitLab.Mock
{
    public sealed class ResourceLabelEvent : GitLabObject
    {
        public int Id { get; set; }

        public Author User { get; set; }

        public DateTime CreatedAt { get; set; }

        public int ResourceId { get; set; }

        public string ResourceType { get; set; }

        public Label Label { get; set; }

        public ResourceLabelEventAction Action { get; set; }

        public Models.ResourceLabelEvent ToClientResourceLabelEvent()
        {
            return new Models.ResourceLabelEvent()
            {
                Id = Id,
                User = User,
                CreatedAt = CreatedAt,
                ResourceId = ResourceId,
                ResourceType = ResourceType,
                Label = Label.ToClientLabel(),
                Action = Action,
            };
        }
    }
}
