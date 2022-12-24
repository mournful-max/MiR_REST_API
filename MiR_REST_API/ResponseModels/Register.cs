using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MiR_REST_API.ResponseModels
{
    public sealed class Register
    {
        [JsonPropertyName("allowed_methods")]
        public List<string> AllowedMethods;
        [JsonPropertyName("id")]
        public int? Id;
        [JsonPropertyName("value")]
        public double? Value;
        [JsonPropertyName("label")]
        public string Label;
    }
}
