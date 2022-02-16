namespace ProblematicBookmark.Analyzers
{
    using System.Text.RegularExpressions;

    internal partial class AnchorPointAnalyzer
    {
        private static readonly string Pattern = @"\]\((.*?)\)";

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
                var anchorPoint = await GetAnchorPoint(match.Groups[1].Value);
                if (!string.IsNullOrEmpty(anchorPoint))
                {
                    Add(KeyValuePair.Create(anchorPoint, match.Groups[1].Value));
                }

                match = match.NextMatch();
            }
        }

        private async static Task<string> GetAnchorPoint(string str)
        {

            if (string.IsNullOrEmpty(str) || !str.StartsWith("/"))
            {
                return string.Empty;
            }

            if (str.StartsWith("https://docs.microsoft.com"))
            {
                return str;
            }

            var startIndex = str.IndexOf("#");
            if (startIndex == -1 || startIndex == str.Length)
            {
                return string.Empty;
            }

            var anchorPoint = str.Substring(startIndex + 1);
            if (anchorPoint.IndexOf("&") != -1)
            {
                anchorPoint = anchorPoint.Substring(0, anchorPoint.IndexOf("&"));
            }

            return await Task.FromResult(anchorPoint);
        }
    }
}