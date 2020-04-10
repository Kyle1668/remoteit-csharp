using Xunit;
using System.Security.Authentication;
using System.Net.Http;

namespace Remoteit.Test
{
    public class RemoteitClientTest
    {
        private readonly HttpClient testHttpClient = new HttpClient();

        [Fact]
        public void TestPublicMemberInit()
        {
            var testClient = new RemoteitClient("kyle1668", "foo", "c9xNsPKT7Yo5xHNj", testHttpClient);

            Assert.Equal("c9xNsPKT7Yo5xHNj", testClient.DeveloperKey);
            Assert.Equal(testHttpClient, testClient.HttpApiClient);
        }
    }
}