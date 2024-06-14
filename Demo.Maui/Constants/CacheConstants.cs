using System;
namespace Demo.Maui.Constants;

public static class CacheConstants
{
    public static class Cache
    {
        public const string Name = "Movie.Demo.Cache";

        /// <summary>
        /// 1 hour
        /// </summary>
        public const int DefaultInvalidationTime = 60;

        /// <summary>
        /// 5 Days
        /// </summary>
        public const int StaticInvalidationTime = 7200;
    }

    public static class Keys
    {
        public static string ImageConfiguration = nameof(ImageConfiguration);
        public static string Genres = nameof(Genres);
        public static string RecentPopularFilms = nameof(RecentPopularFilms);
    }
}

