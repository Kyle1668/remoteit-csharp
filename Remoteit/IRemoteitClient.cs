using System.Net.Http;

namespace Remoteit
{
    public interface IRemoteitClient
    {
        HttpClient HttpApiClient { get; }

        string DeveloperKey { get; }
    }
}