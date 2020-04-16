using System;

namespace Remoteit.Util
{
    internal class UnixTimeStampCalculator : IUnixTimeStampCalculator
    {
        public long Calculate()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }
}