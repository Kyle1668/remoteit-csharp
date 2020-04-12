using System;

namespace Remoteit.Util
{
    public class UnixTimeStampCalculator : IUnixTimeStampCalculator
    {
        public long Calculate()
        {
            return DateTimeOffset.Now.ToUnixTimeMilliseconds();
        }
    }
}