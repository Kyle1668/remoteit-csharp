using Remoteit.Models;
using Remoteit.Util;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Remoteit.Test")]

namespace Remoteit.RestApi
{
    internal class RemoteitApiSessionManager : IRemoteitApiSessionManager
    {
        private IUnixTimeStampCalculator _timeCalculator;

        public RemoteitApiSessionManager(IUnixTimeStampCalculator timeCalculator = null, HttpClient httpClient = null)
        {
            _timeCalculator = timeCalculator ?? new UnixTimeStampCalculator();
            _httpApiClient = httpClient ?? new HttpClient() { BaseAddress = new Uri("https://api.remot3.it/apv/v27") };
        }

        public RemoteitApiSession CurrentSessionData { get; set; }

        private HttpClient _httpApiClient { get; set; }

        public async Task<RemoteitApiSession> GenerateSession(string userName, string userPassword, string developerKey)
        {
            string requestBodyJsonData = JsonSerializer.Serialize(new Dictionary<string, string>()
            {
                { "username", userName },
                { "password", userPassword }
            });

            var httpRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(string.Concat(_httpApiClient.BaseAddress, "/user/login")),
                Content = new StringContent(requestBodyJsonData)
            };
            httpRequest.Headers.Add("developerkey", developerKey);

            var sessionData = await new RemoteitApiRequest<RemoteitApiSession>(_httpApiClient).SendAsync(httpRequest);
            return sessionData;
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