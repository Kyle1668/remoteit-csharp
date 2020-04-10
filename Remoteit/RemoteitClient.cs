using Remoteit.Models;
using System.Net.Http;
using System.Collections.Generic;

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

        public RemoteitClient(IEnumerable<char> userName, IEnumerable<char> password, IEnumerable<char> developerKey, HttpClient requestClient)
        {
            _userName = userName;
            _userPassword = password;

            DeveloperKey = developerKey;
            HttpApiClient = requestClient;
        }
    }
}