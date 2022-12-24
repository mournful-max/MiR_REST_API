using System.Text.Json.Serialization;

namespace MiR_REST_API.RequestModels
{
    public sealed class StateValue
    {
        [JsonPropertyName("state_id")]
        public int StateId;

        public StateValue() { }
        public StateValue(int stateId)
        {
            StateId = stateId;
        }

        public const int READY          = 3;
        public const int PAUSE          = 4;
        public const int MANUAL_CONTROL = 11;
    }
}
