using Moq;
using Moq.Protected;
using Remoteit.RestApi;
using Remoteit.Util;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Remoteit.Models;
using System.Security.Authentication;

namespace Remoteit.Test.RestApi
{
    public class RemoteitApiRequest
    {
        // Test Cases
        // 1. When IsSuccessStatusCode is false, the correct AuthenticationException is thrown.
        // 2. When an HttpRequestException us thrown, the correct AuthenticationException is thrown.

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

            // Execute the SUT: Attempt to create a new session.
            await Assert.ThrowsAsync<AuthenticationException>(async () =>
             {
                 var testRequest = new RemoteitApiRequest<DevicesListEndpointResponse>();
                 await testRequest.SendAsync(testHttpClient, httpRequest);
             });

            // Verify that the request made to the remote.it API was only made once and was the correct method.
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
            throw new NotImplementedException();
        }
    }
}