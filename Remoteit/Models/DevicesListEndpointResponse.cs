using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Remoteit.Models
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