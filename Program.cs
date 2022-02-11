// See https://aka.ms/new-console-template for more information
using CommandLine;
using Microsoft.Extensions.Configuration;
using ProblematicBookmark;
using ProblematicBookmark.Models;
using ConfigurationProvider = ProblematicBookmark.ConfigurationProvider;

try
{
    TraceLog.WriteLineInfo($"Scan Start:{DateTime.UtcNow}");
    var configBuilder = new ConfigurationBuilder();
    var config = configBuilder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
    var configProvider = new ConfigurationProvider();
    config.Bind(configProvider);

    await Parser.Default.ParseArguments<BookmarkOptions>(args)
            .MapResult(
              async (BookmarkOptions opts) => await AnalyseBookMark(opts, configProvider),
              errs => Task.FromResult<int>(1));
    TraceLog.WriteLineInfo($"Scan finished:{DateTime.UtcNow}");
}
catch (Exception ex)
{
    TraceLog.WriteLineError("Exception" + ex.Message);
}

static async Task AnalyseBookMark(BookmarkOptions opts, ConfigurationProvider config)
{
    ServiceCollectionExtension.ServiceCollection.RegisterConfigurationProvider(config)
        .RegisterBookmarkOptions(opts).RegisterServices().StartUp();

    var filterResults = await CallerContext.BookmarkManager.Scan();
    Utilities.Save(filterResults);
}
