using System.Net.Http;
using System.Threading.Tasks;

namespace Remoteit.RestApi
{
    internal interface IRemoteitApiRequest<T>
    {
        Task<T> SendAsync(HttpRequestMessage httpRequest);

        Task<T> SendAsync(HttpRequestMessage httpRequest, HttpClient httpApiClient);
    }
}