using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Remoteit.RestApi
{
    internal interface IRemoteitApiRequest<T>
    {
        Task<T> SendAsync(HttpClient httpApiClient, HttpRequestMessage httpRequest);
    }
}