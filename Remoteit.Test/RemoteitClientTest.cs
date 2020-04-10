using Xunit;
using System.Security.Authentication;

namespace Remoteit.Test
{
    public class RemoteitClientTest
    {
        [Fact]
        public void AuthenticateSuccessfully()
        {
            var testClient = new RemoteitClient("c9xNsPKT7Yo5xHNj", "kyle1668", "foo");

            Assert.Equal("c9xNsPKT7Yo5xHNj", testClient.DeveloperKey);
            Assert.Equal("kyle1668", testClient.UserName);
        }
    }
}