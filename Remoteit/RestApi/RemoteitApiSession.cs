using System.Text.Json.Serialization;
using System.Collections;
using System;

namespace Remoteit.RestApi
{
    public class RemoteitApiSession
    {
        [JsonPropertyName("status")]
        public bool Status { get; }

        [JsonPropertyName("token")]
        public string Token { get; }

        [JsonPropertyName("token_index")]
        public string TokenIndex { get; }

        [JsonPropertyName("email")]
        public string Email { get; }

        [JsonPropertyName("guid")]
        public string Guid { get; }

        [JsonPropertyName("service_token")]
        public string ServiceToken { get; }

        [JsonPropertyName("service_level")]
        public string ServiceLevel { get; }

        [JsonPropertyName("storage_plan")]
        public string StoragePlan { get; }

        [JsonPropertyName("secondary_auth")]
        public string SecondaryAuth { get; }

        [JsonPropertyName("auth_token")]
        public string AuthToken { get; }

        [JsonPropertyName("auth_expiration")]
        public long TokenExpirationDate { get; }

        [JsonPropertyName("service_authhash")]
        public string ServiceAuthHash { get; }

        [JsonPropertyName("commerical_setting")]
        public string CommericalSetting { get; }

        [JsonPropertyName("apikey")]
        public string ApiKey { get; }

        [JsonPropertyName("developer_key")]
        public string DeveloperKey { get; }

        [JsonPropertyName("language")]
        public string Language { get; }

        [JsonPropertyName("developer_plan")]
        public string DeveloperPlan { get; }

        [JsonPropertyName("portal_plan")]
        public string PortalPlan { get; }

        [JsonPropertyName("portal_plan_expires")]
        public string PortalPlanExpiresDate { get; }

        [JsonPropertyName("service_features")]
        public string ServiceFeatures { get; }

        [JsonPropertyName("announcements")]
        public IEnumerable Announcements { get; }

        [JsonPropertyName("member_since")]
        public string MemberSince { get; }

        [JsonPropertyName("index")]
        public bool Index { get; }

        [JsonPropertyName("pubsub_channel")]
        public string PubsubChannel { get; }

        [JsonPropertyName("aws_identity")]
        public string AwsIdentity { get; }

        public bool SessionHasExpired()
        {
            return TokenExpirationDate <= DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }
}