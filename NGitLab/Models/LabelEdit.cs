﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public class LabelEdit
    {
        public LabelEdit()
        {
        }

        public LabelEdit(int projectId, Label label)
        {
            Id = projectId;
            Name = label.Name;
        }

        [Required]
        public int Id;

        [Required]
        [JsonPropertyName("name")]
        public string Name;

        [JsonPropertyName("new_name")]
        public string NewName;

        [JsonPropertyName("color")]
        public string Color;

        [JsonPropertyName("description")]
        public string Description;
    }
}
