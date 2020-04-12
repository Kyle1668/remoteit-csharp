using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Remoteit.Util;
using Remoteit.RestApi;

namespace Remoteit
{
    public class RemoteitClient : IRemoteitClient
    {
        public HttpClient HttpApiClient { get; }

        public IEnumerable<char> DeveloperKey { get; }

        private IEnumerable<char> _userName { get; }

        private IEnumerable<char> _userPassword { get; }

        private RemoteitApiSession _currentSession;

        private bool _invalidSession
        {
            get { return _currentSession == null || _currentSession.SessionHasExpired(); }
        }

        public RemoteitClient(IEnumerable<char> userName, IEnumerable<char> password, IEnumerable<char> developerKey, HttpClient requestClient = null)
        {
            _userName = userName;
            _userPassword = password;
            DeveloperKey = developerKey;

            if (requestClient == null)
            {
                HttpApiClient = new HttpClient() { BaseAddress = new System.Uri("https://api.remot3.it/apv/v27") };
            }
            else
            {
                HttpApiClient = requestClient;
            }

            // _currentSession = new RemoteitApiSession(new UnixTimeStampCalculator());
        }
    }
}