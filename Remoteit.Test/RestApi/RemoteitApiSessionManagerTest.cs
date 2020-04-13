using Xunit;
using Moq;
using Remoteit.RestApi;
using Remoteit.Util;

namespace Remoteit.Test.RestApi
{
    public class RemoteitApiSessionManagerTest
    {
        private readonly string testDevKey = "XXXXXX";

        [Fact]
        public void TestSessionExpiredCalculation()
        {
            var mockTimer = new Mock<IUnixTimeStampCalculator>();
            mockTimer.Setup(x => x.Calculate()).Returns(1587257611);

            var testSession = new RemoteitApiSessionManager(mockTimer.Object);
            testSession.CurrentSessionData.TokenExpirationDate = 1587100798;

            Assert.True(testSession.SessionHasExpired());
        }

        [Fact]
        public void TestSessionNotExpiredCalculation()
        {
            var mockTimer = new Mock<IUnixTimeStampCalculator>();
            mockTimer.Setup(x => x.Calculate()).Returns(1587100798);

            var testSession = new RemoteitApiSessionManager(mockTimer.Object);
            testSession.CurrentSessionData.TokenExpirationDate = 1587257611;

            Assert.False(testSession.SessionHasExpired());
        }
    }
}