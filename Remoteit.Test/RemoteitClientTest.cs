﻿using Moq;
using Moq.Protected;
using Remoteit.Models;
using Remoteit.RestApi;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Remoteit.Test
{
    public class RemoteitClientTest
    {
        private readonly HttpClient testHttpClient = new HttpClient();

        /// <summary>
        /// Tests initialization of public members without dependency injection.
        /// </summary>
        [Fact]
        public void TestPublicMemberInitWithProvidedHttpClient()
        {
            var testClient = new RemoteitClient("kyle1668", "foo", "c9xNsPKT7Yo5xHNj", testHttpClient);

            Assert.Equal("c9xNsPKT7Yo5xHNj", testClient.DeveloperKey);
            Assert.Equal(testHttpClient, testClient.HttpApiClient);
        }

        /// <summary>
        /// Tests initialization of public members with dependency injection.
        /// </summary>
        [Fact]
        public void TestPublicMemberInitWithoutProvidingHttpClient()
        {
            var testClient = new RemoteitClient("kyle1668", "foo", "c9xNsPKT7Yo5xHNj");
            var internalDefaultHttpClient = new HttpClient() { BaseAddress = new System.Uri("https://api.remot3.it/apv/v27") };

            Assert.Equal("c9xNsPKT7Yo5xHNj", testClient.DeveloperKey);
            Assert.Equal(internalDefaultHttpClient.ToString(), testClient.HttpApiClient.ToString());
        }

        /// <summary>
        /// Test returning the user's devices
        /// </summary>
        [Fact]
        public async Task TestGetAllDevicesAsync()
        {
            // Set up Mock of HttpMessageHandler. This allows us to mock the response to the HTTP request.
            var mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>
            (
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri == new Uri("https://api.remot3.it/apv/v27/device/list/all")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("{ \"status\": \"true\", \"cache_expires\": 2, \"length\": 2607, \"devices\": [ { \"deviceaddress\": \"80:00:01:13:XX:XX:XX:YY\", \"devicealias\": \"ec2\", \"ownerusername\": \"kyle@remote.it\", \"devicetype\": \"00:23:00:00:00:04:00:05:04:60:FF:YY:UU:CC:XX\", \"devicestate\": \"inactive\", \"devicelastip\": \"13.59.YYY.206\", \"lastinternalip\": \"172.31.16.244\", \"servicetitle\": \"Bulk Service\", \"webenabled\": \"1\", \"weburi\": [ null ], \"localurl\": \"bm9uZQ==\", \"georegion\": \"NorthAmerica\", \"webviewerurl\": [ null ], \"lastcontacted\": \"2019-04-16T14:36:21.71+00:00\", \"createdate\": \"2019-02-06T18:22:36.273+00:00\", \"shared\": \"shared-from\", \"scripting\": false }, { \"deviceaddress\": \"80:00:01:13:A7:XX:XX:XX\", \"devicealias\": \"wordpress\", \"ownerusername\": \"foo@remot3.it\", \"devicetype\": \"00:07:00:00:00:04:00:05:04:60:00:50:00:XX:XX:XX\", \"devicestate\": \"inactive\", \"devicelastip\": \"13.59.119.206\", \"lastinternalip\": \"172.31.16.244\", \"servicetitle\": \"HTTP\", \"webenabled\": \"1\", \"weburi\": [ null ], \"localurl\": \"bm9uZQ==\", \"georegion\": \"NorthAmerica\", \"webviewerurl\": [ null ], \"lastcontacted\": \"2019-04-16T14:26:20.5+00:00\", \"createdate\": \"2019-02-06T18:28:56.017+00:00\", \"shared\": \"shared-from\", \"scripting\": false }, { \"deviceaddress\": \"80:00:01:13:CC:XX:XX:XX\", \"devicealias\": \"https\", \"ownerusername\": \"foo@remote.it\", \"devicetype\": \"00:07:00:XX:XX:04:00:05:04:60:01:B1:00:01:00:00\", \"devicestate\": \"inactive\", \"devicelastip\": \"13.59.YYY:XXX\", \"lastinternalip\": \"172.31.16.244\", \"servicetitle\": \"HTTP\", \"webenabled\": \"1\", \"weburi\": [ null ], \"localurl\": \"bm9uZQ==\", \"georegion\": \"NorthAmerica\", \"webviewerurl\": [ null ], \"lastcontacted\": \"2019-04-16T14:20:57.763+00:00\", \"createdate\": \"2019-02-06T23:07:10.143+00:00\", \"shared\": \"shared-from\", \"scripting\": false }, { \"deviceaddress\": \"80:00:01:13:AA:XX:XX:XX\", \"devicealias\": \"azure_vm_1\", \"ownerusername\": \"kyledevinobrien1@gmail.com\", \"devicetype\": \"00:23:00:00:00:04:00:06:04:60:FF:FF:00:01:00:00\", \"devicestate\": \"active\", \"devicelastip\": \"23.96.205.250\", \"lastinternalip\": \"10.0.0.4\", \"servicetitle\": \"Bulk Service\", \"webenabled\": \"1\", \"weburi\": [ null ], \"localurl\": \"bm9uZQ==\", \"georegion\": \"NorthAmerica\", \"webviewerurl\": [ null ], \"lastcontacted\": \"2020-04-17T19:15:00.497+00:00\", \"createdate\": \"2020-04-09T15:15:11.813+00:00\", \"shared\": \"not-shared\", \"scripting\": true }, { \"deviceaddress\": \"80:00:00:00:ZZ:ZZ:ZZ:ZZ\", \"devicealias\": \"ssh\", \"ownerusername\": \"kyledevinobrien1@gmail.com\", \"devicetype\": \"00:1C:00:00:00:04:00:06:04:60:00:16:00:01:00:00\", \"devicestate\": \"active\", \"devicelastip\": \"23.96.205.250\", \"lastinternalip\": \"10.0.0.4\", \"servicetitle\": \"SSH\", \"webenabled\": \"1\", \"weburi\": \"\\/ssh\\/index.php\", \"localurl\": \"bm9uZQ==\", \"georegion\": \"NorthAmerica\", \"webviewerurl\": [ null ], \"lastcontacted\": \"2020-04-17T19:15:39.153+00:00\", \"createdate\": \"2020-04-09T15:16:01.71+00:00\", \"shared\": \"not-shared\", \"scripting\": true } ] }")
            })
            .Verifiable();

            // Set up mock of the session data. This will prevent autorefreshing the sesison token which is outside the scope of this test.
            var mockSessionManager = new Mock<IRemoteitApiSessionManager>();
            mockSessionManager.Setup(session => session.SessionHasExpired()).Returns(false).Verifiable();
            mockSessionManager.Setup(session => session.CurrentSessionData).Returns(new RemoteitApiSession() { Token = "f5cce83b0a20d66a4c6710a8327e213d" }).Verifiable();

            // Create a test HttpClient that uses the mocked HttpMessageHandler. Creates instance of SUT.
            var testHttpClient = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("https://api.remot3.it/apv/v27") };
            var testRemoteitClient = new RemoteitClient("foo@remote.it", "pass123", "X12345", testHttpClient) { CurrentSession = mockSessionManager.Object };

            // Execute the SUT: Attempt to get devices
            var devices = await testRemoteitClient.GetDevices();

            // Verify that the request made to the remote.it API was only made once and with the correct HTTP method and headers.
            mockHttpMessageHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(
                    req => req.Method == HttpMethod.Get &&
                           req.RequestUri == new Uri("https://api.remot3.it/apv/v27/device/list/all") &&
                           req.Headers.GetValues("developerkey").FirstOrDefault() == "X12345" &&
                           req.Headers.GetValues("token").FirstOrDefault() == "f5cce83b0a20d66a4c6710a8327e213d"),
                ItExpr.IsAny<CancellationToken>()
            );

            // Verify that the current session was checked to ensure that a token refresh didn't occur.
            mockSessionManager.Verify(session => session.SessionHasExpired(), Times.Once());
            mockSessionManager.Verify(session => session.CurrentSessionData, Times.Exactly(2));

            // Ensure that the correct devices are returned
            Assert.True(devices.Count == 5);
            Assert.True(devices[0].DeviceAddress == "80:00:01:13:XX:XX:XX:YY");
            Assert.True(devices[1].DeviceAddress == "80:00:01:13:A7:XX:XX:XX");
            Assert.True(devices[2].DeviceAddress == "80:00:01:13:CC:XX:XX:XX");
            Assert.True(devices[3].DeviceAddress == "80:00:01:13:AA:XX:XX:XX");
            Assert.True(devices[4].DeviceAddress == "80:00:00:00:ZZ:ZZ:ZZ:ZZ");
        }
    }
}