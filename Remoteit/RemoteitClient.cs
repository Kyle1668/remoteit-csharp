using Remoteit.Models;
using Remoteit.RestApi;
using Remoteit.Util;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.CompilerServices;
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
        ///
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="developerKey">Your developer key which can be found by logging into remote.it and going to your "Account" settings page.</param>
        /// <param name="requestClient"></param>
        public RemoteitClient(string userName, string password, string developerKey, HttpClient requestClient = null)
        {
            _userName = userName;
            _userPassword = password;

            DeveloperKey = developerKey;
            _httpApiClient = requestClient != null ? requestClient : new HttpClient() { BaseAddress = new Uri("https://api.remot3.it/apv/v27") };
            CurrentSession = new RemoteitApiSessionManager(new UnixTimeStampCalculator(), _httpApiClient);
        }

        public string DeveloperKey { get; }

        internal IRemoteitApiSessionManager CurrentSession { get; set; }

        private HttpClient _httpApiClient { get; }

        private bool _isInvalidSession
        {
            get { return CurrentSession == null || CurrentSession.SessionHasExpired(); }
        }

        private string _userName { get; }

        private string _userPassword { get; }

        /// <summary>
        /// Generate a connection to a remote.it device/service. API Documentation: https://docs.remote.it/api-reference/devices/connect
        /// </summary>
        /// <param name="deviceAddress">The service address (e.g. service ID) for the device you'd like to connect to.</param>
        public async Task<ServiceConnection> ConnectToService(string deviceAddress)
        {
            if (_isInvalidSession)
            {
                CurrentSession.CurrentSessionData = await CurrentSession.GenerateSession(_userName, _userPassword, DeveloperKey);
            }

            string requestBodyJsonData = JsonSerializer.Serialize(new Dictionary<string, dynamic>()
            {
                {"deviceaddress",  deviceAddress},
                {"wait", true }
            });

            var httpRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(string.Concat(_httpApiClient.BaseAddress, "/device/connect")),
                Content = new StringContent(requestBodyJsonData)
            };
            httpRequest.Headers.Add("developerkey", DeveloperKey);
            httpRequest.Headers.Add("token", CurrentSession.CurrentSessionData.Token);

            var apiRequestSender = new RemoteitApiRequest<ServiceConnectionEndpointResponse>(_httpApiClient);
            ServiceConnectionEndpointResponse results = await apiRequestSender.SendAsync(httpRequest);
            return results.Connection;
        }

        /// <summary>
        /// Get a  list of your remote.it devices and data. API Documentation: https://docs.remote.it/api-reference/devices/list
        /// </summary>
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
            httpRequest.Headers.Add("developerkey", DeveloperKey);
            httpRequest.Headers.Add("token", CurrentSession.CurrentSessionData.Token);

            var apiRequestSender = new RemoteitApiRequest<DevicesListEndpointResponse>(_httpApiClient);
            DevicesListEndpointResponse results = await apiRequestSender.SendAsync(httpRequest);
            return results.Devices;
        }
    }
}