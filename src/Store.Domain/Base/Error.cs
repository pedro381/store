using System.Text.Json.Serialization;

namespace Store.Domain.Base
{
    public class Error
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}
