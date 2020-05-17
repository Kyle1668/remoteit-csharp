using System.Net.Http;
using System.Security.Authentication;
using System.Text.Json;
using System.Threading.Tasks;
using Remoteit.Exceptions;

namespace Remoteit.RestApi
{
    internal class RemoteitApiRequest<T> : IRemoteitApiRequest<T>
    {
        private HttpClient _httpApiClient;

        public RemoteitApiRequest(HttpClient httpApiClient = null)
        {
            _httpApiClient = httpApiClient ?? new HttpClient();
        }

        public async Task<T> SendAsync(HttpRequestMessage httpRequest)
        {
            try
            {
                HttpResponseMessage httpResponse = await _httpApiClient.SendAsync(httpRequest);
                var rawResponseBody = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    return JsonSerializer.Deserialize<T>(rawResponseBody);
                }

                throw new RemoteitException(rawResponseBody);
            }
            catch (HttpRequestException apiRequestError)
            {
                throw new RemoteitException(apiRequestError.Message);
            }
        }
    }
}