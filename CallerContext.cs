namespace ProblematicBookmark
{
    using ProblematicBookmark.Analyzers;
    using ProblematicBookmark.Managers;
    using ProblematicBookmark.Models;
    using System;

    internal class CallerContext : CallerContextBase
    {
        public static BookmarkOptions BookmarkOptions => Resolve<BookmarkOptions>();
        public static ConfigurationProvider ConfigurationProvider => Resolve<ConfigurationProvider>();

        public static AnchorPointAnalyzer AnchorPointAnalyzer => Resolve<AnchorPointAnalyzer>();
        public static WebCrawlerAnalyzer WebCrawlerAnalyzer => Resolve<WebCrawlerAnalyzer>();

        public static BookmarkManager BookmarkManager => Resolve<BookmarkManager>();

    }

    internal class CallerContextBase
    {
        public static T Resolve<T>() where T : class
        {
            return Container.GetService(typeof(T)) as T;
        }

        static AsyncLocal<IServiceProvider> AsyncLocal { get; } = new AsyncLocal<IServiceProvider>();

        public static IServiceProvider Container
        {
            get => AsyncLocal.Value;
            set
            {
                AsyncLocal.Value = value;
            }
        }
    }
}