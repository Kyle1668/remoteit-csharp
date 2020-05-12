using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Remoteit.Models
{
    public class RemoteitDevice
    {
        [JsonPropertyName("createdate")]
        public string CreatedDate { get; set; }

        [JsonPropertyName("deviceaddress")]
        public string DeviceAddress { get; set; }

        [JsonPropertyName("devicealias")]
        public string DeviceAlias { get; set; }

        [JsonPropertyName("devicelastip")]
        public string DeviceLastIP { get; set; }

        [JsonPropertyName("devicestate")]
        public string DeviceState { get; set; }

        [JsonPropertyName("devicetype")]
        public string DeviceType { get; set; }

        [JsonPropertyName("georegion")]
        public string GeoRegion { get; set; }

        [JsonPropertyName("lastcontacted")]
        public string LastContacted { get; set; }

        [JsonPropertyName("lastinternalip")]
        public string LastInternalIP { get; set; }

        [JsonPropertyName("localurl")]
        public string LocalUrl { get; set; }

        [JsonPropertyName("ownerusername")]
        public string OwnerUserName { get; set; }

        [JsonPropertyName("scripting")]
        public bool Scripting { get; set; }

        [JsonPropertyName("servicetitle")]
        public string ServiceTitle { get; set; }

        [JsonPropertyName("shared")]
        public string Shared { get; set; }

        [JsonPropertyName("webenabled")]
        public string WebEnabled { get; set; }

        [JsonPropertyName("webviewerurl")]
        public List<string> WebViewUrl { get; set; }
    }
}