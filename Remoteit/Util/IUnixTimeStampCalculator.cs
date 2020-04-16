using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Remoteit.Test")]
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]

namespace Remoteit.Util
{
    internal interface IUnixTimeStampCalculator
    {
        public long Calculate();
    }
}