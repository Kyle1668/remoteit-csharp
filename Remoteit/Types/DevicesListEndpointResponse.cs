using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

[assembly: InternalsVisibleTo("Remoteit.Test")]

namespace Remoteit.Types
{
    internal class DevicesListEndpointResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("cache_expires")]
        public int CacheExpires { get; set; }

        [JsonPropertyName("length")]
        public int Length { get; set; }

        [JsonPropertyName("devices")]
        public List<RemoteitDevice> Devices { get; set; }
    }
}