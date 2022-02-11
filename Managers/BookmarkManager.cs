namespace ProblematicBookmark.Managers
{
    using Microsoft.Extensions.DependencyInjection;
    using ProblematicBookmark.Models;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    internal class BookmarkManager : List<FilterResult>
    {
        public async Task<List<FilterResult>> Scan()
        {
            string informationalString = $"[info] Scan {CallerContext.BookmarkOptions.ContentRepoLocalPath}";
            TraceLog.WriteLineInfo(informationalString);
            //Console.WriteLine(informationalString);
            await ScanFile(CallerContext.BookmarkOptions.ContentRepoLocalPath);
            return this;
        }

        private async Task ScanFile(string path,int dep=1)
        {
            var filePaths = Directory.GetFiles(path, "*.md");
            
            foreach (var filePath in filePaths)
            {
                using (var trace = new TraceLog())
                {
                    try
                    {
                        var bookmarkurl = await GetProblematicBookmark(filePath);
                        if (bookmarkurl != null && bookmarkurl.Count > 0)
                        {
                            var str = Regex.Replace(@filePath, @"\\", @"\\"); 
                            Add(new FilterResult()
                            {
                                MarkdownPath = str,
                                Bookmarkurl = await GetProblematicBookmark(filePath)
                            });
                        }

                        var count = bookmarkurl == null ? 0 : bookmarkurl?.Count;
                        TraceLog.WriteLineInfo(GenarateDepStr(dep) + "|___" + Path.GetFileName(filePath) + $" count:{count}");
                    }
                    catch (Exception ex)
                    {
                        TraceLog.WriteLineError(GenarateDepStr(dep) + "|___" + Path.GetFileName(filePath) + "  failed");
                        trace.LogLineError("File:" + filePath)
                             .LogLineError("Detailed:" + ex.Message);
                    }
                }
            }

            var dirs = Directory.GetDirectories(path);
            foreach (var dir in dirs)
            {
                TraceLog.WriteLineInfo(GenarateDepStr(dep) + "|___" +dir.Replace(path,""));
                await ScanFile(dir,dep+1);
            }
        }

        private string GenarateDepStr(int dep=1)
        {
            StringBuilder sb = new StringBuilder();
            for(var i=0;i<dep;i++)
                sb.Append("   ");
            return sb.ToString();
        }

        private async Task<List<string>> GetProblematicBookmark(string file)
        {
            var problemmaticBookmarks = new List<string>();
            var bookmarks = await CallerContext.AnchorPointAnalyzer.Analysis(file);
            foreach (var bookmark in bookmarks)
                    {
                        var correct = await CallerContext.WebCrawlerAnalyzer.Validate(bookmark);
                        if (!correct)
                        {
                            problemmaticBookmarks.Add(bookmark.Value);
                        }
                    }
              
            return await Task.FromResult(problemmaticBookmarks);
        }
    }
}
