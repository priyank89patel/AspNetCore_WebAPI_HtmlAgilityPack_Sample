using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore_WebApi_HtmlAgilityPack_Sample.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCore_WebApi_HtmlAgilityPack_Sample.Controllers
{
    [Route("api/parse-html")]
    [ApiController]
    public class HtmlParserController : ControllerBase
    {
        private readonly IHtmlParserService _htmlParserService;
        public HtmlParserController(IHtmlParserService htmlParserService)
        {
            _htmlParserService = htmlParserService;
        }

        [HttpPost("replace-people-names/{*selector}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ReplacePeopleNamesRandomly(string selector)
        {
            if (string.IsNullOrEmpty(selector))
                return BadRequest();

            var replacedHtml = await _htmlParserService.ReplacePeopleNames(selector);

            return new ContentResult
            {
                StatusCode = StatusCodes.Status200OK,
                ContentType = "text/html",
                Content = replacedHtml
            };
        }
    }
}