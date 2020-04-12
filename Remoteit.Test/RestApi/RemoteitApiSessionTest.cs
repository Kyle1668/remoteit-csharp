using Xunit;
using Moq;
using Remoteit.RestApi;
using Remoteit.Util;

namespace Remoteit.Test.RestApi
{
    public class RemoteitApiSessionTest
    {
        [Fact]
        public void TestSessionExpiredCalculation()
        {
            var mockTimer = new Mock<IUnixTimeStampCalculator>();
            mockTimer.Setup(x => x.Calculate()).Returns(1587257611);

            var testSession = new RemoteitApiSession(mockTimer.Object);
            testSession.TokenExpirationDate = 1587100798;

            Assert.True(testSession.SessionHasExpired());
        }

        [Fact]
        public void TestSessionNotxpiredCalculation()
        {
            var mockTimer = new Mock<IUnixTimeStampCalculator>();
            mockTimer.Setup(x => x.Calculate()).Returns(1587100798);

            var testSession = new RemoteitApiSession(mockTimer.Object);
            testSession.TokenExpirationDate = 1587257611;

            Assert.False(testSession.SessionHasExpired());
        }
    }
}