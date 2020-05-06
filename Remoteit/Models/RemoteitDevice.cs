using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace Remoteit.Models
{
    /*
     * {
      "deviceaddress": "80:00:01:13:A7:00:0F:92",
      "devicealias": "wordpress-gec2",
      "ownerusername": "kyle@remot3.it",
      "devicetype": "00:23:00:00:00:04:00:05:04:60:FF:FF:00:01:00:00",
      "devicestate": "inactive",
      "devicelastip": "13.59.119.206",
      "lastinternalip": "172.31.16.244",
      "servicetitle": "Bulk Service",
      "webenabled": "1",
      "weburi": [
        null
      ],
      "localurl": "bm9uZQ==",
      "georegion": "NorthAmerica",
      "webviewerurl": [
        null
      ],
      "lastcontacted": "2019-04-16T14:36:21.71+00:00",
      "createdate": "2019-02-06T18:22:36.273+00:00",
      "shared": "shared-from",
      "scripting": false
    },
    */

    public class RemoteitDevice
    {
        [JsonPropertyName("deviceaddress")]
        public string DeviceAddress { get; set; }

        [JsonPropertyName("devicealias")]
        public string DeviceAlias { get; set; }

        [JsonPropertyName("ownerusername")]
        public string OwnerUserName { get; set; }

        [JsonPropertyName("devicetype")]
        public string DeviceType { get; set; }

        [JsonPropertyName("devicestate")]
        public string DeviceState { get; set; }

        [JsonPropertyName("devicelastip")]
        public string DeviceLastIP { get; set; }

        [JsonPropertyName("lastinternalip")]
        public string LastInternalIP { get; set; }

        [JsonPropertyName("servicetitle")]
        public string ServiceTitle { get; set; }

        [JsonPropertyName("webenabled")]
        public string WebEnabled { get; set; }

        /*
        [JsonPropertyName("weburi")]
        public string WebUri { get; set; }
        */

        [JsonPropertyName("localurl")]
        public string LocalUrl { get; set; }

        [JsonPropertyName("georegion")]
        public string GeoRegion { get; set; }

        [JsonPropertyName("webviewerurl")]
        public List<string> WebViewUrl { get; set; }

        [JsonPropertyName("lastcontacted")]
        public string LastContacted { get; set; }

        [JsonPropertyName("createdate")]
        public string CreatedDate { get; set; }

        [JsonPropertyName("shared")]
        public string Shared { get; set; }

        [JsonPropertyName("scripting")]
        public bool Scripting { get; set; }
    }
}