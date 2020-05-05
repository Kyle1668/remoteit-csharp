using Remoteit.Models;
using Remoteit.RestApi;
using Remoteit.Util;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;

namespace Remoteit
{
    /// <summary>
    /// Provides an interface to programmatically list and connect to your remote.it devices.
    /// </summary>
    public class RemoteitClient : IRemoteitClient
    {
        /// <summary>
        /// The Http client used to facilitate requests to the remote.it REST API.
        /// </summary>
        public HttpClient HttpApiClient { get; }

        /// <summary>
        /// Required for authentication.  Your developer key which can be found by logging into remote.it and going to your Account settings page.
        /// </summary>
        public IEnumerable<char> DeveloperKey { get; }

        private IEnumerable<char> _userName { get; }

        private IEnumerable<char> _userPassword { get; }

        private RemoteitApiSessionManager _currentSession;

        private bool _invalidSession
        {
            get { return _currentSession == null || _currentSession.SessionHasExpired(); }
        }

        public RemoteitClient(IEnumerable<char> userName, IEnumerable<char> password, IEnumerable<char> developerKey, HttpClient requestClient = null)
        {
            _userName = userName;
            _userPassword = password;
            DeveloperKey = developerKey;

            if (requestClient == null)
            {
                HttpApiClient = new HttpClient() { BaseAddress = new System.Uri("https://api.remot3.it/apv/v27") };
            }
            else
            {
                HttpApiClient = requestClient;
            }

            _currentSession = new RemoteitApiSessionManager(new UnixTimeStampCalculator(), HttpApiClient);
        }

        public async Task<List<RemoteitDevice>> GetDevices()
        {
            if (_currentSession.CurrentSessionData == null || _currentSession.SessionHasExpired())
            {
                _currentSession.CurrentSessionData = await _currentSession.GenerateSession(_userName, _userPassword);
            }

            var httpRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(string.Concat(HttpApiClient.BaseAddress, "/device/list/all"))
            };

            httpRequest.Headers.Add("developerkey", DeveloperKey.ToString());
            httpRequest.Headers.Add("token", _currentSession.CurrentSessionData.Token);

            try
            {
                HttpResponseMessage httpResponse = await HttpApiClient.SendAsync(httpRequest);
                var rawResponseBody = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    var deserializeResponseBody = JsonSerializer.Deserialize<DevicesListApiResponse>(rawResponseBody);
                    return deserializeResponseBody.Devices;
                }

                throw new AuthenticationException(rawResponseBody);
            }
            catch (HttpRequestException apiRequestError)
            {
                throw new AuthenticationException(apiRequestError.Message);
            }
        }
    }
}
