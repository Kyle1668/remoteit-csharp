using Remoteit.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

[assembly: InternalsVisibleTo("Remoteit.Test")]

namespace Remoteit.RestApi
{
    internal class RemoteitApiSessionManager
    {
        private IUnixTimeStampCalculator _timeCalculator;

        public RemoteitApiSession CurrentSessionData { get; set; }

        public RemoteitApiSessionManager(IUnixTimeStampCalculator timeCalculator)
        {
            _timeCalculator = timeCalculator;
        }

        public bool SessionHasExpired()
        {
            //return TokenExpirationDate <= DateTimeOffset.Now.ToUnixTimeMilliseconds();
            return CurrentSessionData.TokenExpirationDate <= _timeCalculator.Calculate();
        }
    }
}