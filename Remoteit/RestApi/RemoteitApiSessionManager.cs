using Remoteit.Models;
using Remoteit.Util;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Text.Json;

[assembly: InternalsVisibleTo("Remoteit.Test")]

namespace Remoteit.RestApi
{
    internal class RemoteitApiSessionManager
    {
        private IUnixTimeStampCalculator _timeCalculator;

        private HttpClient _httpApiClient { get; }

        public RemoteitApiSession CurrentSessionData { get; set; }

        public RemoteitApiSessionManager(IUnixTimeStampCalculator timeCalculator, HttpClient httpClient)
        {
            _timeCalculator = timeCalculator;
            _httpApiClient = httpClient;
        }

        public async void GenerateSession(string userName, string userPassword)
        {
            var apiEndpoint = string.Concat(_httpApiClient.BaseAddress, "device/connect");

            var requestBody = new Dictionary<string, IEnumerable<char>>()
            {
                { "username", userName },
                { "password", userPassword }
            };

            var rawJsonRequestBody = new StringContent(JsonSerializer.Serialize(requestBody));

            try
            {
                HttpResponseMessage response = await _httpApiClient.PostAsync(apiEndpoint, rawJsonRequestBody);
                var responseBody = await response.Content.ReadAsStringAsync();
                CurrentSessionData = JsonSerializer.Deserialize<RemoteitApiSession>(responseBody);
            }
            catch (HttpRequestException apiRequestError)
            {
                throw new AuthenticationException(apiRequestError.Message);
            }
        }

        public bool SessionHasExpired()
        {
            return CurrentSessionData.TokenExpirationDate <= _timeCalculator.Calculate();
        }
    }
}