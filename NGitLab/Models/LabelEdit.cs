using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

[EditorBrowsable(EditorBrowsableState.Never)]
public class LabelEdit
{
    [JsonIgnore]
    public long Id;

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
