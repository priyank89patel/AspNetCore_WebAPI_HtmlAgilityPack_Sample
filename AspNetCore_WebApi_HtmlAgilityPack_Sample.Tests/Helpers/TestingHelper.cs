using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;

namespace AspNetCore_WebApi_HtmlAgilityPack_Sample.Tests.Helpers
{
    public static class TestingHelper
    {
        public static HttpMessageHandler MockHttpMessageHandlerReturnsSuccessStatusCode(string response)
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(response)
                });

            return httpMessageHandlerMock.Object;
        }

        public static HttpMessageHandler MockHttpMessageHandlerReturnsErrorStatusCode()
        {
            var httpMessageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(
                new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("Internal Server Error")
                });

            return httpMessageHandlerMock.Object;
        }
    }
}
