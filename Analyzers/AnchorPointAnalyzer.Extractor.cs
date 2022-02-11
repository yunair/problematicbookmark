namespace ProblematicBookmark.Extractors
{
    using System.Text.RegularExpressions;

    internal partial class AnchorPointAnalyzer
    {
        private static readonly string Pattern = @"(?<=\()[^\<\(\)]+(?=\))";

        private async Task Extract(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                await Task.CompletedTask;
            }

            await ExtractAnchorPointByRegex(content);
        }

        private async Task ExtractAnchorPointByRegex(string content)
        {
            var match = Regex.Match(content, Pattern);
            while (match.Success)
            {
                var anchorPoint = await GetAnchorPoint(match.Groups[0].Value);
                if (!string.IsNullOrEmpty(anchorPoint))
                {
                    Add(KeyValuePair.Create(anchorPoint, match.Groups[0].Value));
                }

                match = match.NextMatch();
            }
        }

        private async static Task<string> GetAnchorPoint(string str)
        {
            if (string.IsNullOrEmpty(str)
                || str.StartsWith("https")
                || str.StartsWith("http")
                || !await IsMatch(@"^([/][a-zA-Z]|[a-zA-Z].*[/][a-zA-Z]).*", str))
            {
                return string.Empty;
            }
           
            var startIndex = str.IndexOf("#");

            if (startIndex == -1)
            {
                return string.Empty;
            }

            var relativeUrl = str.Substring(0,startIndex);

            if (relativeUrl.Contains(".md")
                || relativeUrl.Contains(".png")
                || relativeUrl.Contains(".yml")
                || startIndex==str.Length)
            {
                return string.Empty;
            }
            
            var anchorPoint = str.Substring(startIndex+1);
            if (anchorPoint.IndexOf("&")!=-1)
            {
                anchorPoint = anchorPoint.Substring(0, anchorPoint.IndexOf("&"));
            }

            return await Task.FromResult(anchorPoint);
        }

        private async static Task<bool> IsMatch(string expresstion,string str)
        {
            Regex reg = new Regex(expresstion);
            if (string.IsNullOrEmpty(str))
            {
                return await Task.FromResult(false);
            }

            return await Task.FromResult(reg.IsMatch(str));
        }
    }
}
