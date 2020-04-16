﻿using Remoteit.Models;
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

        public RemoteitApiSession CurrentSessionData { get; set; }

        public RemoteitApiSessionManager(IUnixTimeStampCalculator timeCalculator, HttpClient httpClient)
        {
            _timeCalculator = timeCalculator;
            _httpApiClient = httpClient;
        }

        public async Task<RemoteitApiSession> GenerateSession(string userName, string userPassword)
        {
            var apiEndpoint = new Uri(string.Concat(_httpApiClient.BaseAddress.OriginalString, "/device/connect"));

            var requestBody = new Dictionary<string, IEnumerable<char>>()
            {
                { "username", userName },
                { "password", userPassword }
            };

            var rawJsonRequestBody = new StringContent(JsonSerializer.Serialize(requestBody));

            try
            {
                var request = new HttpRequestMessage()
                {
                    Content = rawJsonRequestBody,
                    Method = HttpMethod.Post,
                    RequestUri = apiEndpoint
                };

                HttpResponseMessage response = await _httpApiClient.SendAsync(request);

                //HttpResponseMessage response = await _httpApiClient.PostAsync(apiEndpoint, rawJsonRequestBody);

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