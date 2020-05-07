using Remoteit.Models;
using Remoteit.RestApi;
using Remoteit.Util;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Remoteit.Test")]

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
        /// Required for authentication. Your developer key which can be found by logging into remote.it and going to your Account settings page.
        /// </summary>
        public string DeveloperKey { get; }

        private string _userName { get; }

        private string _userPassword { get; }

        internal IRemoteitApiSessionManager CurrentSession { get; set; }

        private bool _invalidSession
        {
            get { return CurrentSession == null || CurrentSession.SessionHasExpired(); }
        }

        public RemoteitClient(string userName, string password, string developerKey, HttpClient requestClient = null)
        {
            _userName = userName;
            _userPassword = password;

            DeveloperKey = developerKey;
            HttpApiClient = requestClient != null ? requestClient : new HttpClient() { BaseAddress = new Uri("https://api.remot3.it/apv/v27") };
            CurrentSession = new RemoteitApiSessionManager(new UnixTimeStampCalculator(), HttpApiClient);
        }

        /// <summary>
        /// Retrieves a list of the user's devices: https://docs.remote.it/api-reference/devices/list
        /// </summary>
        /// <returns>A Task object with the list of RemoteitDevices</returns>
        public async Task<List<RemoteitDevice>> GetDevices()
        {
            if (_invalidSession)
            {
                CurrentSession.CurrentSessionData = await CurrentSession.GenerateSession(_userName, _userPassword);
            }

            var httpRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(string.Concat(HttpApiClient.BaseAddress, "/device/list/all"))
            };

            httpRequest.Headers.Add("developerkey", DeveloperKey.ToString());
            httpRequest.Headers.Add("token", CurrentSession.CurrentSessionData.Token);

            try
            {
                HttpResponseMessage httpResponse = await HttpApiClient.SendAsync(httpRequest);
                var rawResponseBody = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    var deserializeResponseBody = JsonSerializer.Deserialize<DevicesListEndpointResponse>(rawResponseBody);
                    return deserializeResponseBody.Devices;
                }

                throw new AuthenticationException(rawResponseBody);
            }
            catch (HttpRequestException apiRequestError)
            {
                throw new AuthenticationException(apiRequestError.Message);
            }
        }

        public async Task<ServiceConnection> ConnectToService(string deviceAddress)
        {
            if (_invalidSession)
            {
                CurrentSession.CurrentSessionData = await CurrentSession.GenerateSession(_userName, _userPassword);
            }

            var requestBodyAttributed = new Dictionary<string, dynamic>()
            {
                {"deviceaddress",  deviceAddress},
                {"wait", true }
            };

            var httpRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(string.Concat(HttpApiClient.BaseAddress, "/device/connect")),
                Content = new StringContent(JsonSerializer.Serialize(requestBodyAttributed))
            };

            httpRequest.Headers.Add("developerkey", DeveloperKey.ToString());
            httpRequest.Headers.Add("token", CurrentSession.CurrentSessionData.Token);

            try
            {
                HttpResponseMessage httpResponse = await HttpApiClient.SendAsync(httpRequest);
                var rawResponseBody = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    var deserializeResponseBody = JsonSerializer.Deserialize<ServiceConnectionEndpointResponse>(rawResponseBody);
                    return deserializeResponseBody.Connection;
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
