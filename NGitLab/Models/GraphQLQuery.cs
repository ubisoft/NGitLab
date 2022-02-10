using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NGitLab.Models
{
    public sealed class GraphQLQuery
    {
        [JsonPropertyName("query")]
        public string Query { get; set; }

        [JsonPropertyName("operationName")]
        public string OperationName { get; set; }

        [JsonPropertyName("variables")]
        public IDictionary<string, object> Variables { get; } = new Dictionary<string, object>(StringComparer.Ordinal);
    }
}
