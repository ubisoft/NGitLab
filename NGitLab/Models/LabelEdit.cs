using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

[EditorBrowsable(EditorBrowsableState.Never)]
public class LabelEdit
{
    [JsonIgnore]
    public long Id { get; set; }

    [Required]
    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("new_name")]
    public string NewName { get; set; }

    [JsonPropertyName("color")]
    public string Color { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }
}
