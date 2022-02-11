namespace ProblematicBookmark.Models
{
    using System.Collections.Generic;

    internal class FilterResult
    {
        public string MarkdownPath { get; set; }

        public List<string> Bookmarkurl { get; set; }
    }
}
