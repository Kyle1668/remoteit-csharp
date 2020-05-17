using System.Text.Json.Serialization;

namespace Remoteit.Types
{
    internal class ConnectionTerminationEndpointResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }
    }
}