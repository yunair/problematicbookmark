namespace ProblematicBookmark
{
    using System.Collections.Generic;

    internal class Constants
    {
        public static IReadOnlyDictionary<string, string> BookMarkUrlTemplate { get; } = new Dictionary<string, string>
        {
            { "PubDev",  "https://review.docs.microsoft.com/en-us" },
            { "Public",  "https://docs.microsoft.com/en-us/" }
        };
    }
}