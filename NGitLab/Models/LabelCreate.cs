using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NGitLab.Models;

[EditorBrowsable(EditorBrowsableState.Never)]
public class LabelCreate
{
    [JsonIgnore]
    public int Id;

    [Required]
    [JsonPropertyName("name")]
    public string Name;

    [Required]
    [JsonPropertyName("color")]
    public string Color;

    [JsonPropertyName("description")]
    public string Description;
}
