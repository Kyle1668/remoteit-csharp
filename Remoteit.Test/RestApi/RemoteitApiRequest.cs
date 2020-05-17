using Moq;
using Moq.Protected;
using Remoteit.Exceptions;
using Remoteit.RestApi;
using Remoteit.Types;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Remoteit.Test.RestApi
{
    public class RemoteitApiRequest
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        [Fact]
        public async Task TestHandlingHttpRequestException()
        {
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>
            (
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ThrowsAsync(new HttpRequestException())
            .Verifiable();

            await Assert.ThrowsAsync<RemoteitException>(async () =>
            {
                var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri("https://api.remot3.it/apv/v27/device/list/all"));
                var remoteitRequest = new RemoteitApiRequest<DevicesListEndpointResponse>();
                await remoteitRequest.SendAsync(httpRequest);
            });
        }

        [Fact]
        public async Task TestHandlingIncorrectStatusCode()
        {
            var mockHttpResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent("{ \"status\": \"false\", \"reason\": \"missing api token\", \"code\": \"GENERAL_ERROR\" }")
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>
            (
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockHttpResponse)
            .Verifiable();

            var expectedHttpMethod = HttpMethod.Get;
            var expectedApiEndpointUri = new Uri("https://api.remot3.it/apv/v27/device/list/all");

            var testHttpClient = new HttpClient(_mockHttpMessageHandler.Object) { BaseAddress = new Uri("https://api.remot3.it/apv/v27") };
            var httpRequest = new HttpRequestMessage(expectedHttpMethod, expectedApiEndpointUri);

            await Assert.ThrowsAsync<RemoteitException>(async () =>
             {
                 var testRequest = new RemoteitApiRequest<DevicesListEndpointResponse>(testHttpClient);
                 await testRequest.SendAsync(httpRequest);
             });

            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == expectedHttpMethod && req.RequestUri == expectedApiEndpointUri),
                ItExpr.IsAny<CancellationToken>()
            );
        }
    }
}