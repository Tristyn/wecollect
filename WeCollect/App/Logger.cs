using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace WeCollect.App
{
    public class Logger : ILogger
    {

        private readonly ILogger _next;


        private Logger(ILogger next)
        {
            _next = next;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (logLevel >= LogLevel.Warning)
            {
                Debug.Fail(exception.Message);
            }
            _next.Log(logLevel, eventId, state, exception, formatter);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return _next.IsEnabled(logLevel);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return _next.BeginScope(state);
        }        

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

    public static class ILoggerExtensions
    {
        public static void LogError(this ILogger log, Exception exception)
        {
            log.LogError(exception, exception.Message);
        }
    }
}