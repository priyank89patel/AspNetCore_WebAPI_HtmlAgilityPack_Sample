using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AspNetCore_WebApi_HtmlAgilityPack_Sample.Controllers;
using AspNetCore_WebApi_HtmlAgilityPack_Sample.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace AspNetCore_WebApi_HtmlAgilityPack_Sample.Tests.Controllers
{
    public class HtmlParserControllerTests
    {
        public HtmlParserControllerTests()
        {
        }

        [Fact]
        public async void Replace_WhenSelectorIsValid_ReturnsReplacedHtmlWith200StatusCode()
        {
            //Arrange
            var html = @"<body>
                <div>
                <p>John M</p>
                <p>Kelly T</p>
                <p>Ryan S</p>
                </div>
            </body>";

            var htmlParserService = new Mock<IHtmlParserService>();
            htmlParserService.Setup(s => s.ReplacePeopleNames(It.IsAny<string>())).ReturnsAsync(html);

            var htmlParserController = new HtmlParserController(htmlParserService.Object);

            //Act
            var response = await htmlParserController.ReplacePeopleNamesRandomly("//div//p");
            var contentResult = response as ContentResult;

            //Assert
            Assert.NotNull(contentResult);
            Assert.Equal(StatusCodes.Status200OK, contentResult.StatusCode);
        }

        [Fact]
        public async void Replace_WhenSelectorIsEmpty_Returns400BadRequest()
        {
            //Arrange
            var htmlParserService = new Mock<IHtmlParserService>();
            var htmlParserController = new HtmlParserController(htmlParserService.Object);

            //Act
            var response = await htmlParserController.ReplacePeopleNamesRandomly("");
            var badRequestResult = response as BadRequestResult;

            //Assert
            Assert.NotNull(badRequestResult);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }
    }
}
