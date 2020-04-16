using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using Remoteit.Util;
using Remoteit.RestApi;

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
        public HttpClient HttpApiClient { get; }

        /// <summary>
        /// Required for authentication.  Your developer key which can be found by logging into remote.it and going to your Account settings page.
        /// </summary>
        public IEnumerable<char> DeveloperKey { get; }

        private IEnumerable<char> _userName { get; }

        private IEnumerable<char> _userPassword { get; }

        private RemoteitApiSessionManager _currentSession;

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

            _currentSession = new RemoteitApiSessionManager(new UnixTimeStampCalculator(), HttpApiClient);
        }
    }
}