using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Remoteit
{
    public interface IRemoteitClient
    {
        HttpClient HttpApiClient { get; }

        IEnumerable<char> DeveloperKey { get; }
    }
}