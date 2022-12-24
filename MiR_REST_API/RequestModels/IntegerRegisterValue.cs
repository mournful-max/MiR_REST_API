using System.Text.Json.Serialization;

namespace MiR_REST_API.RequestModels
{
    public sealed class IntegerRegisterValue
    {
        [JsonPropertyName("value")]
        public int Value;

        public IntegerRegisterValue() { }
        public IntegerRegisterValue(int value)
        {
            Value = value;
        }
    }
}
