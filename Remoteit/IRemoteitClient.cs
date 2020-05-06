using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Remoteit
{
    public interface IRemoteitClient
    {
        HttpClient HttpApiClient { get; }

        string DeveloperKey { get; }
    }
}