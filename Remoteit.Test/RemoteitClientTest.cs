using Xunit;
using System.Security.Authentication;
using System.Net.Http;

namespace Remoteit.Test
{
    public class RemoteitClientTest
    {
        private readonly HttpClient testHttpClient = new HttpClient();

        [Fact]
        public void TestPublicMemberInitWithProvidedHttpClient()
        {
            var testClient = new RemoteitClient("kyle1668", "foo", "c9xNsPKT7Yo5xHNj", testHttpClient);

            Assert.Equal("c9xNsPKT7Yo5xHNj", testClient.DeveloperKey);
            Assert.Equal(testHttpClient, testClient.HttpApiClient);
        }

        [Fact]
        public void TestPublicMemberInitWithoutProvidedHttpClient()
        {
            var testClient = new RemoteitClient("kyle1668", "foo", "c9xNsPKT7Yo5xHNj");
            var internalDefaultHttpClient = new HttpClient() { BaseAddress = new System.Uri("https://api.remot3.it/apv/v27") };

            Assert.Equal("c9xNsPKT7Yo5xHNj", testClient.DeveloperKey);
            Assert.Equal(internalDefaultHttpClient.ToString(), testClient.HttpApiClient.ToString());
        }
    }
}