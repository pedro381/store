using System.Text.Json.Serialization;

namespace Store.Domain.Base
{
    public class ResponseBase
    {
        [JsonPropertyName("has_errors")]
        public bool HasErrors => Errors?.Count > 0;

        [JsonPropertyName("errors")]
        public List<Error> Errors { get; set; } = [];

        public void AddErro(string message)
        {
            Errors ??= [];
            Errors.Add(new Error { Message = message });
        }
    }
}
