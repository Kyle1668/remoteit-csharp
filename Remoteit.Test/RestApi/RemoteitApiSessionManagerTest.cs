﻿using Moq;
using Moq.Protected;
using Remoteit.Exceptions;
using Remoteit.RestApi;
using Remoteit.Types;
using Remoteit.Util;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Remoteit.Test.RestApi
{
    public class RemoteitApiSessionManagerTest
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        [Fact]
        public async Task TestCreatingANewApiSessionAsync()
        {
            var mockHttpResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{  \"status\": \"true\",  \"token\": \"4c7aa09820a05364487c1300a5887f89\",  \"token_index\": \"83E9FCDE-2F3E-6D55-1B81-97B08404DA82\",  \"email\": \"XXXX@gmail.com\",  \"guid\": \"XXXXXX-DDF8-4609-930D-FFD32AE3D224\",  \"service_token\": \"EBE7EBE7E4A69989AABF8695B1E489BBBFA8BBBA8C9B8D8A9F8E97E8A78AB695A9E7E4E8EDE9ECECECEAE7E8EFEEE4EFEEF0ECEEF0EFF0EFE9E8E4B5A7B2BBBABBA8B7B0B1BCACB7BBB0EF9EB9B3BFB7B2F0BDB1B3E49C9F8D979DE4ECEBE4ECE6E6EEEEE4EFEEEEEEEEE4ECEEEEE4EEE4EFEEEEE4BB9886A7AE878CBD\",  \"service_level\": \"BASIC\",  \"storage_plan\": \"FREE\",  \"secondary_auth\": \"l1Ws9YZRjFW7j2bSjEqEAaQeaIg=\",  \"auth_token\": \"4c7aa09820a05364487c1300a5887f89\",  \"auth_expiration\": 1587257611,  \"service_authhash\": \"XYXYXYXYXYXYXY\",  \"commerical_setting\": \"FREE\",  \"apikey\": \"XYXYXYXYXYXYXY\",  \"developer_key\": \"XYXYXYXYXYXYXY\",  \"language\": \"en\",  \"developer_plan\": \"RDZDNDI2NjAtRjNBRi01QjVGLTAwNjUtRkU3QjM5NTIwN0ZB\",  \"portal_plan\": \"free_plan\",  \"portal_plan_expires\": \"2038-01-01 00:00:00\",  \"service_features\": \"YWRzPTAsc2hhcmU9MjAwLGNvbmN1cnJlbnQ9MTAwLHAycGR1cmF0aW9uPTI4ODAwLHAycGRhaWx5PTEwMDAwLGd1ZXN0PTA=\",  \"announcements\": [],  \"member_since\": \"2018-09-10 XYXYXYXYXYXYXY\",  \"index\": \"true\",  \"pubsub_channel\": \"2F8610ED-DDF8-4609-930D-FFD32AE3D224\",  \"aws_identity\": \"us-east-1:c33b2655-3779-4625-a7e8-XYXYXYXYXYXYXY\"}")
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>
            (
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockHttpResponse)
            .Verifiable();

            var expectedHttpMethod = HttpMethod.Post;
            var expectedApiEndpointUri = new Uri("https://api.remot3.it/apv/v27/user/login");

            var testHttpClient = new HttpClient(_mockHttpMessageHandler.Object) { BaseAddress = new Uri("https://api.remot3.it/apv/v27") };
            var testSession = new RemoteitApiSessionManager(new UnixTimeStampCalculator(), testHttpClient);

            testSession.CurrentSessionData = await testSession.GenerateSession("kyle", "some_developer_key", "some_dev_key");

            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == expectedHttpMethod && req.RequestUri == expectedApiEndpointUri),
                ItExpr.IsAny<CancellationToken>()
            );

            Assert.Equal("4c7aa09820a05364487c1300a5887f89", testSession.CurrentSessionData.Token);
            Assert.Equal("1587257611", testSession.CurrentSessionData.TokenExpirationDate.ToString());
        }

        [Fact]
        public void TestExceptionThrownWhenCheckingExpirationWithNoSession()
        {
            var testSession = new RemoteitApiSessionManager(new UnixTimeStampCalculator(), new HttpClient());

            Assert.Throws<RemoteitClientOperationException>(() =>
            {
                testSession.SessionHasExpired();
            });
        }

        [Fact]
        public async Task TestHandlingUnableToCreateNewSession()
        {
            var mockHttpResponse = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent("{  \"status\": \"false\",  \"reason\": \"api key failed validation\",  \"code\": \"GENERAL_ERROR\"}")
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>
            (
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(mockHttpResponse)
            .Verifiable();

            var testHttpClient = new HttpClient(_mockHttpMessageHandler.Object) { BaseAddress = new Uri("https://api.remot3.it/apv/v27") };
            var testSession = new RemoteitApiSessionManager(new UnixTimeStampCalculator(), testHttpClient);

            var expectedHttpMethod = HttpMethod.Post;
            var expectedApiEndpointUri = new Uri("https://api.remot3.it/apv/v27/user/login");

            await Assert.ThrowsAsync<RemoteitException>(() => testSession.GenerateSession("kyle", "incorrect_password", "some_dev_key"));

            _mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == expectedHttpMethod && req.RequestUri == expectedApiEndpointUri),
                ItExpr.IsAny<CancellationToken>()
            );
        }

        [Fact]
        public void TestSessionExpiredCalculation()
        {
            var mockTimer = new Mock<IUnixTimeStampCalculator>();
            mockTimer.Setup(x => x.Calculate()).Returns(1587257611);

            var testSession = new RemoteitApiSessionManager(mockTimer.Object, new HttpClient())
            {
                CurrentSessionData = new RemoteitApiSession()
                {
                    TokenExpirationDate = 1587100798
                }
            };

            Assert.True(testSession.SessionHasExpired());
        }

        [Fact]
        public void TestSessionNotExpiredCalculation()
        {
            var mockTimer = new Mock<IUnixTimeStampCalculator>();
            mockTimer.Setup(x => x.Calculate()).Returns(1587100798);

            var testSession = new RemoteitApiSessionManager(mockTimer.Object, new HttpClient())
            {
                CurrentSessionData = new RemoteitApiSession()
                {
                    TokenExpirationDate = 1587257611
                }
            };

            Assert.False(testSession.SessionHasExpired());
        }
    }
}