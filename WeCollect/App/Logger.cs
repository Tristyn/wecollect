using Microsoft.Extensions.Logging;

namespace WeCollect.App
{
    public static class Logger
    {
        public static ILoggerFactory loggerFactory = new LoggerFactory()
            .AddConsole()
            .AddDebug();

        public static ILogger<T> GetLogger<T>()
        {
            return loggerFactory.CreateLogger<T>();
        }

        public static ILogger GetLogger(string categoryName)
        {
            return loggerFactory.CreateLogger(categoryName);
        }
    }
}