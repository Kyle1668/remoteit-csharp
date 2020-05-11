using Remoteit.Models;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

[assembly: InternalsVisibleTo("Remoteit.Test")]

namespace Remoteit.RestApi
{
    internal interface IRemoteitApiSessionManager
    {
        public RemoteitApiSession CurrentSessionData { get; set; }

        public Task<RemoteitApiSession> GenerateSession(string userName, string userPassword, string developerKey);

        public bool SessionHasExpired();
    }
}