using Newtonsoft.Json;
using Remoteit.Models;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Security.Authentication;
using System.Text.Json;

namespace Remoteit
{
    public class RemoteitClient
    {
        private static readonly HttpClient _httpApiClient = new HttpClient()
        {
            BaseAddress = new System.Uri("https://api.remot3.it/apv/v27/")
        };

        public readonly IEnumerable<char> DeveloperKey;

        public readonly IEnumerable<char> UserName;

        private readonly IEnumerable<char> _userPassword;

        private RemoteitApiSession _currentSession;

        public RemoteitClient(IEnumerable<char> developerKey, IEnumerable<char> userName, IEnumerable<char> password)
        {
            DeveloperKey = developerKey;
            UserName = userName;
            _userPassword = password;
            _httpApiClient.DefaultRequestHeaders.Add("developerkey", developerKey.ToString());
        }

        private async Task<RemoteitApiSession> AuthenticateToApi()
        {
            var apiEndpoint = string.Concat(_httpApiClient.BaseAddress, "device/connect");

            var requestBody = new Dictionary<string, IEnumerable<char>>()
            {
                { "username", UserName },
                { "password", _userPassword }
            };

            var rawJsonRequestBody = new StringContent(JsonSerializer.Serialize(requestBody));

            try
            {
                HttpResponseMessage response = await _httpApiClient.PostAsync(apiEndpoint, rawJsonRequestBody);
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<RemoteitApiSession>(responseBody);
            }
            catch (HttpRequestException apiRequestError)
            {
                throw new AuthenticationException(apiRequestError.Message);
            }
        }
    }
}