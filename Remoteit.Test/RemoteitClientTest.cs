using Xunit;
using System.Security.Authentication;

namespace Remoteit.Test
{
    public class RemoteitClientTest
    {
        public RemoteitClientTest()
        {
        }

        [Fact]
        public void AuthenticateSuccessfully()
        {
            // Mock 200 Response from API. Should not throw excpetion.
            var r3Client = new RemoteitClient("c9xNsPKT7Yo5xHNj", "kyle1668", "foo");

            Assert.Equal(RemoteitClient.DeveloperKey, "c9xNsPKT7Yo5xHNj");
            Assert.Equal(RemoteitClient.UserName, "kyle1668");
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