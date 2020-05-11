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
        private HttpClient _httpApiClient { get; }

        /// <summary>
        /// Required for authentication. Your developer key which can be found by logging into remote.it and going to your Account settings page.
        /// </summary>
        public string DeveloperKey { get; }

        private string _userName { get; }

        private string _userPassword { get; }

        internal IRemoteitApiSessionManager CurrentSession { get; set; }

        private bool _isInvalidSession
        {
            get { return CurrentSession == null || CurrentSession.SessionHasExpired(); }
        }

        public RemoteitClient(string userName, string password, string developerKey, HttpClient requestClient = null)
        {
            _userName = userName;
            _userPassword = password;

            DeveloperKey = developerKey;
            _httpApiClient = requestClient != null ? requestClient : new HttpClient() { BaseAddress = new Uri("https://api.remot3.it/apv/v27") };
            CurrentSession = new RemoteitApiSessionManager(new UnixTimeStampCalculator(), _httpApiClient);
        }

        public async Task<List<RemoteitDevice>> GetDevices()
        {
            if (_isInvalidSession)
            {
                CurrentSession.CurrentSessionData = await CurrentSession.GenerateSession(_userName, _userPassword, DeveloperKey);
            }

            var httpRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(string.Concat(_httpApiClient.BaseAddress, "/device/list/all"))
            };
            httpRequest.Headers.Add("developerkey", DeveloperKey.ToString());
            httpRequest.Headers.Add("token", CurrentSession.CurrentSessionData.Token);

            var apiRequestSender = new RemoteitApiRequest<DevicesListEndpointResponse>();
            DevicesListEndpointResponse results = await apiRequestSender.SendAsync(_httpApiClient, httpRequest);
            return results.Devices;
        }

        public async Task<ServiceConnection> ConnectToService(string deviceAddress)
        {
            if (_isInvalidSession)
            {
                CurrentSession.CurrentSessionData = await CurrentSession.GenerateSession(_userName, _userPassword, DeveloperKey);
            }

            var requestBodyAttributes = new Dictionary<string, dynamic>()
            {
                {"deviceaddress",  deviceAddress},
                {"wait", true }
            };

            var httpRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(string.Concat(_httpApiClient.BaseAddress, "/device/connect")),
                Content = new StringContent(JsonSerializer.Serialize(requestBodyAttributes))
            };
            httpRequest.Headers.Add("developerkey", DeveloperKey);
            httpRequest.Headers.Add("token", CurrentSession.CurrentSessionData.Token);

            var apiRequestSender = new RemoteitApiRequest<ServiceConnectionEndpointResponse>();
            ServiceConnectionEndpointResponse results = await apiRequestSender.SendAsync(_httpApiClient, httpRequest);
            return results.Connection;
        }
    }
}