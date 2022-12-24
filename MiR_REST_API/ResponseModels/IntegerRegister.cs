using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MiR_REST_API.ResponseModels
{
    public sealed class IntegerRegister
    {
        [JsonPropertyName("allowed_methods")]
        public List<string> AllowedMethods;
        [JsonPropertyName("id")]
        public int? Id;
        [JsonPropertyName("value")]
        public int? Value;
        [JsonPropertyName("label")]
        public string Label;

        public IntegerRegister() { }
        public IntegerRegister(Register register)
        {
            this.AllowedMethods = register.AllowedMethods;
            this.Id             = register.Id;
            this.Value          = (int?)register.Value;
            this.Label          = register.Label;
        }
    }
}
