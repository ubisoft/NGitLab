using System.Text.Json.Serialization;

namespace NGitLab.Models;

public class RunnerId
{
    public RunnerId()
    {
    }

    public RunnerId(long id)
    {
        Id = id;
    }

    [JsonPropertyName("runner_id")]
    public long Id { get; set; }
}
