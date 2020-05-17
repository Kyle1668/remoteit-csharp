using System.Collections;
using System.Text.Json.Serialization;

namespace Remoteit.Types
{
    internal class RemoteitApiSession
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("token_index")]
        public string TokenIndex { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("guid")]
        public string Guid { get; set; }

        [JsonPropertyName("service_token")]
        public string ServiceToken { get; set; }

        [JsonPropertyName("service_level")]
        public string ServiceLevel { get; set; }

        [JsonPropertyName("storage_plan")]
        public string StoragePlan { get; set; }

        [JsonPropertyName("secondary_auth")]
        public string SecondaryAuth { get; set; }

        [JsonPropertyName("auth_token")]
        public string AuthToken { get; set; }

        [JsonPropertyName("auth_expiration")]
        public long TokenExpirationDate { get; set; }

        [JsonPropertyName("service_authhash")]
        public string ServiceAuthHash { get; set; }

        [JsonPropertyName("commerical_setting")]
        public string CommericalSetting { get; set; }

        [JsonPropertyName("apikey")]
        public string ApiKey { get; set; }

        [JsonPropertyName("developer_key")]
        public string DeveloperKey { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        [JsonPropertyName("developer_plan")]
        public string DeveloperPlan { get; set; }

        [JsonPropertyName("portal_plan")]
        public string PortalPlan { get; set; }

        [JsonPropertyName("portal_plan_expires")]
        public string PortalPlanExpiresDate { get; set; }

        [JsonPropertyName("service_features")]
        public string ServiceFeatures { get; set; }

        [JsonPropertyName("announcements")]
        public IEnumerable Announcements { get; set; }

        [JsonPropertyName("member_since")]
        public string MemberSince { get; set; }

        [JsonPropertyName("index")]
        public string Index { get; set; }

        [JsonPropertyName("pubsub_channel")]
        public string PubsubChannel { get; set; }

        [JsonPropertyName("aws_identity")]
        public string AwsIdentity { get; set; }
    }
}