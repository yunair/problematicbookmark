namespace ProblematicBookmark
{
    using Microsoft.Extensions.DependencyInjection;
    using ProblematicBookmark.Extractors;
    using ProblematicBookmark.Extractors.WebCrawler;
    using ProblematicBookmark.Managers;
    using ProblematicBookmark.Models;

    internal static class ServiceCollectionExtension
    {
        private static IServiceCollection _serviceProvider = new ServiceCollection();
        public static IServiceCollection ServiceCollection
        {
            get
            {
                return _serviceProvider;
            }
        }

        public static void StartUp(this IServiceCollection service)
        {
            CallerContextBase.Container = service.BuildServiceProvider();
        }

        public static IServiceCollection RegisterConfigurationProvider(this IServiceCollection service, ConfigurationProvider configurationProvider)
        {
            service.AddSingleton(configurationProvider);
            return service;
        }

        public static IServiceCollection RegisterBookmarkOptions(this IServiceCollection service, BookmarkOptions BookmarkOptions)
        {
            service.AddSingleton(BookmarkOptions);
            return service;
        }

        public static IServiceCollection RegisterServices(this IServiceCollection service)
        {
            service.AddHttpClient()
                .AddTransient<AnchorPointAnalyzer, AnchorPointAnalyzer>()
                .AddSingleton<WebCrawlerAnalyzer, WebCrawlerAnalyzer>()
                .AddSingleton<BookmarkManager, BookmarkManager>();
            return service;
        }
    }
}
