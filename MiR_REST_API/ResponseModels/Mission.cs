using System.Text.Json.Serialization;

namespace MiR_REST_API.ResponseModels
{
    public sealed class Mission
    {
        [JsonPropertyName("id")]
        public int? Id;
        [JsonPropertyName("state")]
        public string State;
        [JsonPropertyName("url")]
        public string Url;
    }
}
