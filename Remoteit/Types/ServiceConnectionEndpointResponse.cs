using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("Remoteit.Test")]

namespace Remoteit.Types
{
    internal class ServiceConnectionEndpointResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("connection")]
        public ServiceConnection Connection { get; set; }

        [JsonPropertyName("wait")]
        public bool Wait { get; set; }

        [JsonPropertyName("connectionid")]
        public string ConnectionId { get; set; }
    }
}