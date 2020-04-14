using Xunit;
using Moq;
using Remoteit.RestApi;
using Remoteit.Util;
using System.Net.Http;
using System;

namespace Remoteit.Test.RestApi
{
    public class RemoteitApiSessionManagerTest
    {
        private readonly string testDevKey = "XXXXXX";

        // Test ToDo: Throw an exception if the current session is null.
        [Fact]
        public void TestExceptionThrownWhenCheckingExpirationWithNoSession()
        {
            var testSession = new RemoteitApiSessionManager(new UnixTimeStampCalculator(), new HttpClient());

            Assert.Throws<InvalidOperationException>(() =>
            {
                testSession.SessionHasExpired();
            });
        }

        [Fact]
        public void TestSessionExpiredCalculation()
        {
            var mockTimer = new Mock<IUnixTimeStampCalculator>();
            mockTimer.Setup(x => x.Calculate()).Returns(1587257611);

            var testSession = new RemoteitApiSessionManager(mockTimer.Object, new HttpClient());
            testSession.CurrentSessionData.TokenExpirationDate = 1587100798;

            Assert.True(testSession.SessionHasExpired());
        }

        [Fact]
        public void TestSessionNotExpiredCalculation()
        {
            var mockTimer = new Mock<IUnixTimeStampCalculator>();
            mockTimer.Setup(x => x.Calculate()).Returns(1587100798);

            var testSession = new RemoteitApiSessionManager(mockTimer.Object, new HttpClient());
            testSession.CurrentSessionData.TokenExpirationDate = 1587257611;

            Assert.False(testSession.SessionHasExpired());
        }

        [Fact]
        public void TestCreatingANewApiSession()
        {
            // Test the following
            // 1. That the http request is sent once with the correct arguments.
            // 2. That CurrentSessionData has changed. Lazy way could be to check if the token is different?
        }

        [Fact]
        public void TestHandlingUnableToCreateNewSession()
        {
            // Test the following
            // 1. If the API returns a non-200 response, throw an Unable to authenticate exception.
        }
    }
}