using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AspNetCore_WebApi_HtmlAgilityPack_Sample.Services;
using AspNetCore_WebApi_HtmlAgilityPack_Sample.Tests.Helpers;
using Moq;
using Moq.Protected;
using Xunit;

namespace AspNetCore_WebApi_HtmlAgilityPack_Sample.Tests.Services
{
    public class HtmlParserServiceTests
    {
        public HtmlParserServiceTests()
        {
        }

        [Fact]
        public async void ReplacePeopleNames_WhenSelectorIsValid_ReturnsModifiedHtml()
        {
            //Arrange
            var expectedHtml = @"<html>
            <body>
                <div><h1>List of People Names</h1></div>
                <div class=""people-names"">
                <ul>
                    <li>Aaron</li>
                    <li>Adam</li>
                    <li>Bryan</li>
                    <li>Bob</li>
                    <li>Cameron</li>
                    <li>Cathie</li>
                    <li>John</li>
                    <li>Kelly</li>
                    <li>Tony</li>
                </ul>
                </div>
            </body>
            </html>";

            var httpMessageHandlerMock = TestingHelper.MockHttpMessageHandlerReturnsSuccessStatusCode(expectedHtml);
            var httpClient = new HttpClient(httpMessageHandlerMock)
            {
                BaseAddress = new Uri("http://nowebsite.com/")
            };

            var htmlParserService = new HtmlParserService(httpClient);

            //Act
            var actualHtml = await htmlParserService.ReplacePeopleNames("//div[@class=\"people-names\"]//ul//li");

            //Assert
            Assert.NotNull(actualHtml);
            Assert.NotEqual(expectedHtml.Trim(), actualHtml.Trim());
        }

        [Fact]
        public async void ReplacePeopleNames_WhenSelectorIsNotValid_ReturnsOriginalHtml()
        {
            //Arrange
            var expectedHtml = @"<html>
            <body>
                <div><h1>List of People Names</h1></div>
                <div class=""people-names"">
                <ul>
                    <li>Aaron</li>
                    <li>Adam</li>
                    <li>Bryan</li>
                    <li>Bob</li>
                    <li>Cameron</li>
                    <li>Cathie</li>
                    <li>John</li>
                    <li>Kelly</li>
                    <li>Tony</li>
                </ul>
                </div>
            </body>
            </html>";

            var httpMessageHandlerMock = TestingHelper.MockHttpMessageHandlerReturnsSuccessStatusCode(expectedHtml);
            var httpClient = new HttpClient(httpMessageHandlerMock)
            {
                BaseAddress = new Uri("http://nowebsite.com/")
            };

            var htmlParserService = new HtmlParserService(httpClient);

            //Act
            var actualHtml = await htmlParserService.ReplacePeopleNames("//div[@class='i-dont-exist']//ul//li");

            //Assert
            Assert.NotNull(actualHtml);
            Assert.Equal(expectedHtml.Trim(), actualHtml.Trim());
        }

        [Fact]
        public async void ReplacePeopleNames_WhenHttpClientRequestDoesNotSucceed_ReturnsEmptyString()
        {
            //Arrange
            var httpMessageHandlerMock = TestingHelper.MockHttpMessageHandlerReturnsErrorStatusCode();
            var httpClient = new HttpClient(httpMessageHandlerMock)
            {
                BaseAddress = new Uri("http://nowebsite.com/")
            };

            var htmlParserService = new HtmlParserService(httpClient);

            //Act
            var actualHtml = await htmlParserService.ReplacePeopleNames("//div//h1");

            //Assert
            Assert.True(string.IsNullOrEmpty(actualHtml));
        }
    }
}
