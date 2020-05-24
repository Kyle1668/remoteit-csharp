using Moq;
using Moq.Protected;
using Remoteit.RestApi;
using Remoteit.Types;
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
        private readonly Mock<HttpMessageHandler> mockHttpMessageHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        private readonly Mock<IRemoteitApiSessionManager> mockSessionManager = new Mock<IRemoteitApiSessionManager>();

        [Fact]
        public async Task TestAutomaticSessionGeneration()
        {
            var mockLoginResponse = "{  \"status\": \"true\",  \"token\": \"4c7aa09820a05364487c1300a5887f89\",  \"token_index\": \"83E9FCDE-2F3E-6D55-1B81-97B08404DA82\",  \"email\": \"XXXX@gmail.com\",  \"guid\": \"XXXXXX-DDF8-4609-930D-FFD32AE3D224\",  \"service_token\": \"EBE7EBE7E4A69989AABF8695B1E489BBBFA8BBBA8C9B8D8A9F8E97E8A78AB695A9E7E4E8EDE9ECECECEAE7E8EFEEE4EFEEF0ECEEF0EFF0EFE9E8E4B5A7B2BBBABBA8B7B0B1BCACB7BBB0EF9EB9B3BFB7B2F0BDB1B3E49C9F8D979DE4ECEBE4ECE6E6EEEEE4EFEEEEEEEEE4ECEEEEE4EEE4EFEEEEE4BB9886A7AE878CBD\",  \"service_level\": \"BASIC\",  \"storage_plan\": \"FREE\",  \"secondary_auth\": \"l1Ws9YZRjFW7j2bSjEqEAaQeaIg=\",  \"auth_token\": \"4c7aa09820a05364487c1300a5887f89\",  \"auth_expiration\": 1587257611,  \"service_authhash\": \"XYXYXYXYXYXYXY\",  \"commerical_setting\": \"FREE\",  \"apikey\": \"XYXYXYXYXYXYXY\",  \"developer_key\": \"XYXYXYXYXYXYXY\",  \"language\": \"en\",  \"developer_plan\": \"RDZDNDI2NjAtRjNBRi01QjVGLTAwNjUtRkU3QjM5NTIwN0ZB\",  \"portal_plan\": \"free_plan\",  \"portal_plan_expires\": \"2038-01-01 00:00:00\",  \"service_features\": \"YWRzPTAsc2hhcmU9MjAwLGNvbmN1cnJlbnQ9MTAwLHAycGR1cmF0aW9uPTI4ODAwLHAycGRhaWx5PTEwMDAwLGd1ZXN0PTA=\",  \"announcements\": [],  \"member_since\": \"2018-09-10 XYXYXYXYXYXYXY\",  \"index\": \"true\",  \"pubsub_channel\": \"2F8610ED-DDF8-4609-930D-FFD32AE3D224\",  \"aws_identity\": \"us-east-1:c33b2655-3779-4625-a7e8-XYXYXYXYXYXYXY\"}";
            var mockDeviceListResponse = "{ \"status\": \"true\", \"cache_expires\": 2, \"length\": 2607, \"devices\": [ { \"deviceaddress\": \"80:00:01:13:XX:XX:XX:YY\", \"devicealias\": \"ec2\", \"ownerusername\": \"kyle@remote.it\", \"devicetype\": \"00:23:00:00:00:04:00:05:04:60:FF:YY:UU:CC:XX\", \"devicestate\": \"inactive\", \"devicelastip\": \"13.59.YYY.206\", \"lastinternalip\": \"172.31.16.244\", \"servicetitle\": \"Bulk Service\", \"webenabled\": \"1\", \"weburi\": [ null ], \"localurl\": \"bm9uZQ==\", \"georegion\": \"NorthAmerica\", \"webviewerurl\": [ null ], \"lastcontacted\": \"2019-04-16T14:36:21.71+00:00\", \"createdate\": \"2019-02-06T18:22:36.273+00:00\", \"shared\": \"shared-from\", \"scripting\": false }, { \"deviceaddress\": \"80:00:01:13:A7:XX:XX:XX\", \"devicealias\": \"wordpress\", \"ownerusername\": \"foo@remot3.it\", \"devicetype\": \"00:07:00:00:00:04:00:05:04:60:00:50:00:XX:XX:XX\", \"devicestate\": \"inactive\", \"devicelastip\": \"13.59.119.206\", \"lastinternalip\": \"172.31.16.244\", \"servicetitle\": \"HTTP\", \"webenabled\": \"1\", \"weburi\": [ null ], \"localurl\": \"bm9uZQ==\", \"georegion\": \"NorthAmerica\", \"webviewerurl\": [ null ], \"lastcontacted\": \"2019-04-16T14:26:20.5+00:00\", \"createdate\": \"2019-02-06T18:28:56.017+00:00\", \"shared\": \"shared-from\", \"scripting\": false }, { \"deviceaddress\": \"80:00:01:13:CC:XX:XX:XX\", \"devicealias\": \"https\", \"ownerusername\": \"foo@remote.it\", \"devicetype\": \"00:07:00:XX:XX:04:00:05:04:60:01:B1:00:01:00:00\", \"devicestate\": \"inactive\", \"devicelastip\": \"13.59.YYY:XXX\", \"lastinternalip\": \"172.31.16.244\", \"servicetitle\": \"HTTP\", \"webenabled\": \"1\", \"weburi\": [ null ], \"localurl\": \"bm9uZQ==\", \"georegion\": \"NorthAmerica\", \"webviewerurl\": [ null ], \"lastcontacted\": \"2019-04-16T14:20:57.763+00:00\", \"createdate\": \"2019-02-06T23:07:10.143+00:00\", \"shared\": \"shared-from\", \"scripting\": false }, { \"deviceaddress\": \"80:00:01:13:AA:XX:XX:XX\", \"devicealias\": \"azure_vm_1\", \"ownerusername\": \"kyledevinobrien1@gmail.com\", \"devicetype\": \"00:23:00:00:00:04:00:06:04:60:FF:FF:00:01:00:00\", \"devicestate\": \"active\", \"devicelastip\": \"23.96.205.250\", \"lastinternalip\": \"10.0.0.4\", \"servicetitle\": \"Bulk Service\", \"webenabled\": \"1\", \"weburi\": [ null ], \"localurl\": \"bm9uZQ==\", \"georegion\": \"NorthAmerica\", \"webviewerurl\": [ null ], \"lastcontacted\": \"2020-04-17T19:15:00.497+00:00\", \"createdate\": \"2020-04-09T15:15:11.813+00:00\", \"shared\": \"not-shared\", \"scripting\": true }, { \"deviceaddress\": \"80:00:00:00:ZZ:ZZ:ZZ:ZZ\", \"devicealias\": \"ssh\", \"ownerusername\": \"kyledevinobrien1@gmail.com\", \"devicetype\": \"00:1C:00:00:00:04:00:06:04:60:00:16:00:01:00:00\", \"devicestate\": \"active\", \"devicelastip\": \"23.96.205.250\", \"lastinternalip\": \"10.0.0.4\", \"servicetitle\": \"SSH\", \"webenabled\": \"1\", \"weburi\": \"\\/ssh\\/index.php\", \"localurl\": \"bm9uZQ==\", \"georegion\": \"NorthAmerica\", \"webviewerurl\": [ null ], \"lastcontacted\": \"2020-04-17T19:15:39.153+00:00\", \"createdate\": \"2020-04-09T15:16:01.71+00:00\", \"shared\": \"not-shared\", \"scripting\": true } ] }";

            SetupSendAsyncMock(HttpMethod.Post, "https://api.remot3.it/apv/v27/user/login", HttpStatusCode.OK, mockLoginResponse);
            SetupSendAsyncMock(HttpMethod.Get, "https://api.remot3.it/apv/v27/device/list/all", HttpStatusCode.OK, mockDeviceListResponse);

            var testHttpClient = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("https://api.remot3.it/apv/v27") };
            var testRemoteitClient = new RemoteitClient("foo@remote.it", "pass123", "X12345", testHttpClient); ;

            // Test run to trigger session generation
            await testRemoteitClient.GetDevices();

            Assert.NotNull(testRemoteitClient.CurrentSession.CurrentSessionData);
        }

        [Fact]
        public async Task TestConnectToService()
        {
            var mockResponse = "{ \"status\": \"true\", \"connection\": { \"connectionid\": \"972ac13a-7169-476b-8fd4-5cd9cdd9924a\", \"connectionOverridden\": \"true\", \"deviceaddress\": \"80:00:00:00:01:XX:YY:ZZ\", \"expirationsec\": \"28796\", \"imageintervalms\": \"1000\", \"previousConnection\": \"http:\\/\\/proxy17.rt3.io:32192\", \"proxy\": \"http:\\/\\/proxy2.remot3.it:37993\", \"proxyport\": \"37993\", \"proxyserver\": \"proxy2.remot3.it\", \"requested\": \"5\\/6\\/2020T8:59 PM\", \"status\": \"http:\\/\\/proxy2.remot3.it:37993\", \"streamscheme\": [ null ], \"streamuri\": [ null ], \"url\": [ null ], \"requestedAt\": \"2020-05-07T00:59:00+00:00\" }, \"wait\": true, \"connectionid\": \"972ac13a-7169-476b-8fd4-5cd9cdd9924a\" }";
            SetupSendAsyncMock(HttpMethod.Post, "https://api.remot3.it/apv/v27/device/connect", HttpStatusCode.OK, mockResponse);

            mockSessionManager.Setup(session => session.SessionHasExpired()).Returns(false).Verifiable();
            mockSessionManager.Setup(session => session.CurrentSessionData).Returns(new RemoteitApiSession() { Token = "f5cce83b0a20d66a4c6710a8327e213d" }).Verifiable();

            var testHttpClient = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("https://api.remot3.it/apv/v27") };
            var testRemoteitClient = new RemoteitClient("foo@remote.it", "pass123", "X12345", testHttpClient) { CurrentSession = mockSessionManager.Object };

            var fakeDeviceAddress = "80:00:00:00:01:XX:YY:ZZ";
            var testConnectionData = await testRemoteitClient.ConnectToService(fakeDeviceAddress);

            VerifySendAsyncMock(HttpMethod.Post, "https://api.remot3.it/apv/v27/device/connect", Times.AtMostOnce(), "X12345", "f5cce83b0a20d66a4c6710a8327e213d");

            Assert.True(testConnectionData.ProxyPort == "37993");
            Assert.True(testConnectionData.ProxyServer == "proxy2.remot3.it");
            Assert.True(testConnectionData.DeviceAddress == fakeDeviceAddress);
            Assert.True(testConnectionData.ConnectionId == "972ac13a-7169-476b-8fd4-5cd9cdd9924a");
        }

        [Fact]
        public async Task TestGetAllDevicesAsync()
        {
            var mockResponse = "{ \"status\": \"true\", \"cache_expires\": 2, \"length\": 2607, \"devices\": [ { \"deviceaddress\": \"80:00:01:13:XX:XX:XX:YY\", \"devicealias\": \"ec2\", \"ownerusername\": \"kyle@remote.it\", \"devicetype\": \"00:23:00:00:00:04:00:05:04:60:FF:YY:UU:CC:XX\", \"devicestate\": \"inactive\", \"devicelastip\": \"13.59.YYY.206\", \"lastinternalip\": \"172.31.16.244\", \"servicetitle\": \"Bulk Service\", \"webenabled\": \"1\", \"weburi\": [ null ], \"localurl\": \"bm9uZQ==\", \"georegion\": \"NorthAmerica\", \"webviewerurl\": [ null ], \"lastcontacted\": \"2019-04-16T14:36:21.71+00:00\", \"createdate\": \"2019-02-06T18:22:36.273+00:00\", \"shared\": \"shared-from\", \"scripting\": false }, { \"deviceaddress\": \"80:00:01:13:A7:XX:XX:XX\", \"devicealias\": \"wordpress\", \"ownerusername\": \"foo@remot3.it\", \"devicetype\": \"00:07:00:00:00:04:00:05:04:60:00:50:00:XX:XX:XX\", \"devicestate\": \"inactive\", \"devicelastip\": \"13.59.119.206\", \"lastinternalip\": \"172.31.16.244\", \"servicetitle\": \"HTTP\", \"webenabled\": \"1\", \"weburi\": [ null ], \"localurl\": \"bm9uZQ==\", \"georegion\": \"NorthAmerica\", \"webviewerurl\": [ null ], \"lastcontacted\": \"2019-04-16T14:26:20.5+00:00\", \"createdate\": \"2019-02-06T18:28:56.017+00:00\", \"shared\": \"shared-from\", \"scripting\": false }, { \"deviceaddress\": \"80:00:01:13:CC:XX:XX:XX\", \"devicealias\": \"https\", \"ownerusername\": \"foo@remote.it\", \"devicetype\": \"00:07:00:XX:XX:04:00:05:04:60:01:B1:00:01:00:00\", \"devicestate\": \"inactive\", \"devicelastip\": \"13.59.YYY:XXX\", \"lastinternalip\": \"172.31.16.244\", \"servicetitle\": \"HTTP\", \"webenabled\": \"1\", \"weburi\": [ null ], \"localurl\": \"bm9uZQ==\", \"georegion\": \"NorthAmerica\", \"webviewerurl\": [ null ], \"lastcontacted\": \"2019-04-16T14:20:57.763+00:00\", \"createdate\": \"2019-02-06T23:07:10.143+00:00\", \"shared\": \"shared-from\", \"scripting\": false }, { \"deviceaddress\": \"80:00:01:13:AA:XX:XX:XX\", \"devicealias\": \"azure_vm_1\", \"ownerusername\": \"kyledevinobrien1@gmail.com\", \"devicetype\": \"00:23:00:00:00:04:00:06:04:60:FF:FF:00:01:00:00\", \"devicestate\": \"active\", \"devicelastip\": \"23.96.205.250\", \"lastinternalip\": \"10.0.0.4\", \"servicetitle\": \"Bulk Service\", \"webenabled\": \"1\", \"weburi\": [ null ], \"localurl\": \"bm9uZQ==\", \"georegion\": \"NorthAmerica\", \"webviewerurl\": [ null ], \"lastcontacted\": \"2020-04-17T19:15:00.497+00:00\", \"createdate\": \"2020-04-09T15:15:11.813+00:00\", \"shared\": \"not-shared\", \"scripting\": true }, { \"deviceaddress\": \"80:00:00:00:ZZ:ZZ:ZZ:ZZ\", \"devicealias\": \"ssh\", \"ownerusername\": \"kyledevinobrien1@gmail.com\", \"devicetype\": \"00:1C:00:00:00:04:00:06:04:60:00:16:00:01:00:00\", \"devicestate\": \"active\", \"devicelastip\": \"23.96.205.250\", \"lastinternalip\": \"10.0.0.4\", \"servicetitle\": \"SSH\", \"webenabled\": \"1\", \"weburi\": \"\\/ssh\\/index.php\", \"localurl\": \"bm9uZQ==\", \"georegion\": \"NorthAmerica\", \"webviewerurl\": [ null ], \"lastcontacted\": \"2020-04-17T19:15:39.153+00:00\", \"createdate\": \"2020-04-09T15:16:01.71+00:00\", \"shared\": \"not-shared\", \"scripting\": true } ] }";
            SetupSendAsyncMock(HttpMethod.Get, "https://api.remot3.it/apv/v27/device/list/all", HttpStatusCode.OK, mockResponse);

            mockSessionManager.Setup(session => session.SessionHasExpired()).Returns(false).Verifiable();
            mockSessionManager.Setup(session => session.CurrentSessionData).Returns(new RemoteitApiSession() { Token = "f5cce83b0a20d66a4c6710a8327e213d" }).Verifiable();

            var testHttpClient = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("https://api.remot3.it/apv/v27") };
            var testRemoteitClient = new RemoteitClient("foo@remote.it", "pass123", "X12345", testHttpClient) { CurrentSession = mockSessionManager.Object };

            var devices = await testRemoteitClient.GetDevices();

            VerifySendAsyncMock(HttpMethod.Get, "https://api.remot3.it/apv/v27/device/list/all", Times.AtMostOnce(), "X12345", "f5cce83b0a20d66a4c6710a8327e213d");
            mockSessionManager.Verify(session => session.SessionHasExpired(), Times.Once());
            mockSessionManager.Verify(session => session.CurrentSessionData, Times.Exactly(2));

            Assert.True(devices.Count == 5);
            Assert.True(devices[0].DeviceAddress == "80:00:01:13:XX:XX:XX:YY");
            Assert.True(devices[1].DeviceAddress == "80:00:01:13:A7:XX:XX:XX");
            Assert.True(devices[2].DeviceAddress == "80:00:01:13:CC:XX:XX:XX");
            Assert.True(devices[3].DeviceAddress == "80:00:01:13:AA:XX:XX:XX");
            Assert.True(devices[4].DeviceAddress == "80:00:00:00:ZZ:ZZ:ZZ:ZZ");
        }

        [Fact]
        public async Task TestTerminateDeviceConnection()
        {
            var mockResponse = "{ \"status\": \"true\", \"id\": \"b01363de-8d20-40c8-XYXYXY-1acfb0f1303d\" }";
            SetupSendAsyncMock(HttpMethod.Post, "https://api.remot3.it/apv/v27/device/connect/stop", HttpStatusCode.OK, mockResponse);

            mockSessionManager.Setup(session => session.SessionHasExpired()).Returns(false).Verifiable();
            mockSessionManager.Setup(session => session.CurrentSessionData).Returns(new RemoteitApiSession() { Token = "f5cce83b0a20d66a4c6710a8327e213d" }).Verifiable();

            var testHttpClient = new HttpClient(mockHttpMessageHandler.Object) { BaseAddress = new Uri("https://api.remot3.it/apv/v27") };
            var testRemoteitClient = new RemoteitClient("foo@remote.it", "pass123", "X12345", testHttpClient) { CurrentSession = mockSessionManager.Object };

            string fakeDeviceAddress = "80:00:01:13:XX:XX:XX:YY";
            string fakeConnectionId = "972ac13a-7169-476b-8fd4-5cd9cddXXXXXX";
            await testRemoteitClient.TerminateDeviceConnection(fakeDeviceAddress, fakeConnectionId);

            VerifySendAsyncMock(HttpMethod.Post, "https://api.remot3.it/apv/v27/device/connect/stop", Times.AtMostOnce(), "X12345", "f5cce83b0a20d66a4c6710a8327e213d");
        }

        private void SetupSendAsyncMock(HttpMethod requestMethod, string requestUrl, HttpStatusCode responseCode, string responseData)
        {
            mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>
            (
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req => req.Method == requestMethod && req.RequestUri == new Uri(requestUrl)),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = responseCode,
                Content = new StringContent(responseData)
            })
            .Verifiable();
        }

        private void VerifySendAsyncMock(HttpMethod requestMethod, string requestUrl, Times timesSent, string developerKey, string token)
        {
            mockHttpMessageHandler.Protected().Verify(
                   "SendAsync",
                   timesSent,
                   ItExpr.Is<HttpRequestMessage>(
                       req => req.Method == requestMethod &&
                              req.RequestUri == new Uri(requestUrl) &&
                              req.Headers.GetValues("developerkey").FirstOrDefault() == developerKey &&
                              req.Headers.GetValues("token").FirstOrDefault() == token),
                   ItExpr.IsAny<CancellationToken>()
               );
        }

        [Fact]
        public async void DocsTest()
        {
            var remoteitUsername = "kyledevinobrien1@gmail.com";
            var remoteitPassword = "kf4krH7kkB9bkRDxSDsNLbfpjFJi";
            var remoteitDevKey = "MkY4NjEwRUQtRERGOC00NjA5LTkzMEQtRkZEMzJBRTNEMjI0";

            var remoteitClient = new RemoteitClient(remoteitUsername, remoteitPassword, remoteitDevKey);
            string deviceAddress = "80:00:00:00:01:04:80:05";


            // Create a device connection
            ServiceConnection connectionData = await remoteitClient.ConnectToService(deviceAddress);

            // Get the connection's ID from the service connection object.
            string connectionId = connectionData.ConnectionId;

            // Terminate the connection. An exception is thrown if the termination is unsucesful. 
            await remoteitClient.TerminateDeviceConnection(deviceAddress, connectionId);
        }
    }
}
