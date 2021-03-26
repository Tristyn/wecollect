using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;

namespace WeCollect.App
{
    public class Log : ILogger
    {
        public static ILoggerFactory loggerFactory = new LoggerFactory()
            .AddConsole()
            .AddDebug();
        private static ILogger logger = loggerFactory.CreateLogger("default");

        private readonly ILogger _next;


        private Log(ILogger next)
        {
            _next = next;
        }

        void ILogger.Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
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

        public static ILogger<T> GetLogger<T>()
        {
            return loggerFactory.CreateLogger<T>();
        }

        public static ILogger GetLogger(string categoryName)
        {
            return loggerFactory.CreateLogger(categoryName);
        }

        public static void LogError(Exception ex)
        {
            logger.LogError(ex);
            Debug.Fail(ex.ToString() + Environment.NewLine + ex.StackTrace);
        }

        public static void LogError(Exception ex, string message)
        {
            logger.LogError(ex, message);
            Debug.Fail(ex.ToString() + Environment.NewLine + ex.StackTrace);
        }

        public static void LogError(string message)
        {
            logger.LogError(message);
            Debug.Fail(message);
        }

        public static void LogWarn(string message)
        {
            logger.LogWarning(message);
        }

        public static void LogInfo(string message)
        {
            logger.LogInformation(message);
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