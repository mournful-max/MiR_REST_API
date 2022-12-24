using System.Text.Json.Serialization;

namespace MiR_REST_API.RequestModels
{
    public sealed class PostMission
    {
        [JsonPropertyName("mission_id")]
        public string MissionGuid;
    }
}
