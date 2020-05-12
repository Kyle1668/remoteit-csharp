using Moq;
using Moq.Protected;
using Remoteit.Models;
using Remoteit.RestApi;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Remoteit.Test.RestApi
{
    public class RemoteitApiRequest
    {
        [Fact]
        public async Task TestHandlingIncorrectStatusCode()
        {
            // Set up Mock of HttpMessageHandler. This allows us to mock the response to the HTTP request.
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var mockHttpResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent("{ \"status\": \"false\", \"reason\": \"missing api token\", \"code\": \"GENERAL_ERROR\" }")
            };

            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>
            (
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockHttpResponse)
            .Verifiable();

            // Expected behavior of the HTTP request. Verified at the end of the test.
            var expectedHttpMethod = HttpMethod.Get;
            var expectedApiEndpointUri = new Uri("https://api.remot3.it/apv/v27/device/list/all");

            // Create a test HttpClient that uses the mocked HttpMessageHandler. Creates instance of SUT.
            var testHttpClient = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("https://api.remot3.it/apv/v27") };
            var httpRequest = new HttpRequestMessage()
            {
                Method = expectedHttpMethod,
                RequestUri = expectedApiEndpointUri
            };

            await Assert.ThrowsAsync<AuthenticationException>(async () =>
             {
                 var testRequest = new RemoteitApiRequest<DevicesListEndpointResponse>(testHttpClient);
                 await testRequest.SendAsync(httpRequest);
             });

            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == expectedHttpMethod && req.RequestUri == expectedApiEndpointUri),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public async Task TestHandlingHttpRequestException()
        {
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>
            (
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException())
            .Verifiable();

            var httpRequest = new HttpRequestMessage()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://api.remot3.it/apv/v27/device/list/all")
            };

            await Assert.ThrowsAsync<AuthenticationException>(async () =>
            {
                var testRequest = new RemoteitApiRequest<DevicesListEndpointResponse>();
                await testRequest.SendAsync(httpRequest);
            });
        }
    }
}