namespace ProblematicBookmark.Analyzers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    internal class WebCrawlerAnalyzer
    {
        private static Lazy<HttpClient> _httpClient = new Lazy<HttpClient>();
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
                var htmlString = await _httpClient.Value.GetStringAsync(url);
                return htmlString;
            }
            catch (Exception ex)
            {
                throw new Exception(String.Format("url:{0} => Exception Message:{1}", url, ex.ToString()));
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