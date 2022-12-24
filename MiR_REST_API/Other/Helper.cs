using System.Text.Json;

namespace MiR_REST_API.Other
{
    internal static class Helper
    {
        public static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
        {
            IncludeFields = true,
            WriteIndented = true
        };
    }
}
