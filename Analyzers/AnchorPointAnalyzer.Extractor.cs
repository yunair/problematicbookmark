namespace ProblematicBookmark.Analyzers
{
    using System.Text.RegularExpressions;

    internal partial class AnchorPointAnalyzer
    {
        private static readonly string Pattern = @"\]\((.*?)\)";

        private void Extract(string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return;
            }

            ExtractAnchorPointByRegex(content);
        }

        private void ExtractAnchorPointByRegex(string content)
        {
            var match = Regex.Match(content, Pattern);
            while (match.Success)
            {
                var anchorPoint = GetAnchorPoint(match.Groups[1].Value);
                if (!string.IsNullOrEmpty(anchorPoint))
                {
                    Add(KeyValuePair.Create(anchorPoint, match.Groups[1].Value));
                }

                match = match.NextMatch();
            }
        }


        private string GetAnchorPoint(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            if (str.StartsWith("https://docs.microsoft.com"))
            {
                return str;
            }

            if (!str.StartsWith('/'))
            {
                return string.Empty;
            }

            var startIndex = str.IndexOf("#");
            if (startIndex == -1 || startIndex == str.Length-1)
            {
                return string.Empty;
            }

            var anchorPoint = str.Substring(startIndex + 1);
            if (anchorPoint.IndexOf("&") != -1)
            {
                anchorPoint = anchorPoint.Substring(0, anchorPoint.IndexOf("&"));
            }

            return anchorPoint;
        }
    }
}