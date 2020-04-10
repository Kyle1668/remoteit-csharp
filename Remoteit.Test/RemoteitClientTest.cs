using Xunit;
using System.Security.Authentication;

namespace Remoteit.Test
{
    public class RemoteitClientTest
    {
        [Fact]
        public void AuthenticateSuccessfully()
        {
            // Mock 200 Response from API. Should not throw excpetion.
            var testClient = new RemoteitClient("c9xNsPKT7Yo5xHNj", "kyle1668", "foo");

            Assert.Equal("c9xNsPKT7Yo5xHNj", testClient.DeveloperKey);
            Assert.Equal("kyle1668", testClient.UserName);
        }

        [Fact]
        public void ThrowExcpetionWhenAuthenticationFails()
        {
            Assert.Throws<AuthenticationException>(() =>
            {
                // Mock 401 Response from API
                new RemoteitClient("foo", "foo", "foo");
            });
        }

        [Fact]
        public void ThrowExceptionWhenUnableToReachAPI()
        {
            Assert.Throws<AuthenticationException>(() =>
            {
                // Mock 401 Response from API
                new RemoteitClient("foo", "foo", "foo");
            });
        }
    }
}