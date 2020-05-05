using Remoteit.Models;
using Remoteit.Util;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Remoteit.Test")]

namespace Remoteit.RestApi
{
    internal class RemoteitApiSessionManager
    {
        private IUnixTimeStampCalculator _timeCalculator;

        private HttpClient _httpApiClient { get; }

        /// <summary>
        /// The data for the current API session. Includes the access token used for request authentication.
        /// </summary>
        public RemoteitApiSession CurrentSessionData { get; set; }

        public RemoteitApiSessionManager(IUnixTimeStampCalculator timeCalculator, HttpClient httpClient)
        {
            _timeCalculator = timeCalculator;
            _httpApiClient = httpClient;
        }

        /// <summary>
        /// Creates a API session by retreiving a new access token from the "/device/connect" API endpoint.
        /// https://docs.remote.it/api-reference/authentication
        /// </summary>
        /// <param name="userName">E-mail for remote.it(or for legacy users, your username)</param>
        /// <param name="userPassword">Password for remote.it</param>
        /// <returns>A new RemoteitApiSession instance</returns>
        public async Task<RemoteitApiSession> GenerateSession(IEnumerable<char> userName, IEnumerable<char> userPassword)
        {
            var apiEndpoint = new Uri(string.Concat(_httpApiClient.BaseAddress, "/device/connect"));

            var requestBody = new Dictionary<string, IEnumerable<char>>()
            {
                { "username", userName },
                { "password", userPassword }
            };

            var rawJsonRequestBody = new StringContent(JsonSerializer.Serialize(requestBody));

            try
            {
                HttpResponseMessage response = await _httpApiClient.PostAsync(apiEndpoint, rawJsonRequestBody);

                if (response.StatusCode != HttpStatusCode.OK)
                {
                    throw new AuthenticationException();
                }

                var responseBody = await response.Content.ReadAsStringAsync();
                CurrentSessionData = JsonSerializer.Deserialize<RemoteitApiSession>(responseBody);
                return CurrentSessionData;
            }
            catch (HttpRequestException apiRequestError)
            {
                throw new AuthenticationException(apiRequestError.Message);
            }
        }

        /// <summary>
        /// Compares the current Unix time and the session expiration date to determine wether the current token/session has expired.
        /// </summary>
        /// <returns>Wether the session has expires or not.</returns>
        public bool SessionHasExpired()
        {
            if (CurrentSessionData == null)
            {
                throw new InvalidOperationException("Unable to check is the session has expired since there is no current session.");
            }

            return CurrentSessionData.TokenExpirationDate <= _timeCalculator.Calculate();
        }
    }
}
