namespace ProblematicBookmark.Analyzers
{
    using System.Collections.Generic;

    internal partial class AnchorPointAnalyzer : List<KeyValuePair<string, string>>
    {
        public async Task<List<KeyValuePair<string, string>>> Analysis(string filePath)
        {
            await LoadFile(filePath);
            return this;
        }

        private async Task LoadFile(string filePath)
        {
            await OpenFile(filePath);
        }

        private async Task OpenFile(string filePath)
        {
            string text = await File.ReadAllTextAsync(filePath);
            await Extract(text);
        }
    }
}