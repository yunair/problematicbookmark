﻿namespace ProblematicBookmark.Analyzers
{
    using Flurl.Http;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class WebCrawlerAnalyzer
    {
        public async Task<bool> Validate(KeyValuePair<string, string> keyValuePair)
        {
            var bookmarkUrl = InferWebPageUrl(keyValuePair.Value);
            var content = await CrawlContent(bookmarkUrl);
            return Exist(keyValuePair.Key, content);
        }

        private string InferWebPageUrl(string bookmarkUrl)
        {
            if (string.IsNullOrEmpty(bookmarkUrl))
                return string.Empty;

            if (bookmarkUrl.StartsWith("https://docs.microsoft.com"))
            {
                return bookmarkUrl;
            }

            if (!bookmarkUrl.StartsWith("/"))
            {
                bookmarkUrl = "/" + bookmarkUrl;
            }

            return Constants.BookMarkUrlTemplate[CallerContext.ConfigurationProvider.Environment].TrimEnd('/') + bookmarkUrl;
        }

        private async Task<string> CrawlContent(string url)
        {
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                return string.Empty;
            }

            try
            {
                var htmlString = await url.GetStringAsync();
                return htmlString;
            }
            catch
            {
                throw;
            }
        }

        private bool Exist(string anchorPoint, string content)
        {
            if (!string.IsNullOrEmpty(content) &&
                (content.Contains("id=" + '"' + anchorPoint + '"')
                 || content.Contains("name=" + '"' + anchorPoint + '"')
                ))
            {
                return true;
            }

            return false;
        }
    }
}