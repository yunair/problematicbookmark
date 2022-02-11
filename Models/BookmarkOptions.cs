namespace ProblematicBookmark.Models
{
    using CommandLine;

    internal class BookmarkOptions
    {
        [Option('p', "contentRepoLocalPath", Required = true, HelpText = "Content Repo Local Path.")]
        public string ContentRepoLocalPath { get; set; }
    }
}
