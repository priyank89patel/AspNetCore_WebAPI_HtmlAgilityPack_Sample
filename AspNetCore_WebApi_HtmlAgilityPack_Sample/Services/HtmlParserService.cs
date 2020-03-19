using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace AspNetCore_WebApi_HtmlAgilityPack_Sample.Services
{
    public class HtmlParserService : IHtmlParserService
    {
        private readonly HttpClient _httpClient;
        private static Random _random = new Random();
        public HtmlParserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> ReplacePeopleNames(string selector)
        {
            string replacedHtml = string.Empty;
            HtmlDocument htmlDocument = null;

            var request = new HttpRequestMessage(HttpMethod.Get, "List_of_people_by_name");
            var html = await GetHttpResponse(request);

            if (!string.IsNullOrEmpty(html))
            {
                htmlDocument = LoadHtmlFromString(html);

                if (htmlDocument.DocumentNode != null)
                {
                    var htmlNodes = htmlDocument.DocumentNode.SelectNodes(selector);

                    if (htmlNodes != null && htmlNodes.Any())
                    {
                        var names = htmlNodes.Select(n => n.InnerText).ToList();

                        //Randomly change names
                        foreach (var element in htmlNodes)
                        {
                            int index = _random.Next(names.Count);
                            element.InnerHtml = names[index];
                        }
                    }

                    replacedHtml = htmlDocument.DocumentNode.OuterHtml;
                }
            }

            return replacedHtml;
        }

        private async Task<string> GetHttpResponse(HttpRequestMessage request)
        {
            string html = string.Empty;
            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                html = await response.Content.ReadAsStringAsync();

            return html;
        }
        private HtmlDocument LoadHtmlFromString(string htmlContent)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);

            return htmlDocument;
        }
    }
}
