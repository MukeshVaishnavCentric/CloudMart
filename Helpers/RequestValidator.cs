using System.Text.Json;

namespace CloudMart.Helpers
{
    public class RequestValidator
    {
        public static T ValidateBody<T>(string? body)
        {
            if (string.IsNullOrEmpty(body))
                throw new ArgumentException("Request body is required");

            return JsonSerializer.Deserialize<T>(body)!
                ?? throw new ArgumentException("Invalid JSON body");
        }
    }
}
