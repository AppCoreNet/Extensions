using System;
using AppCore.Logging;

namespace AppCore.Hosting
{
    internal static class LoggerExtensions
    {
        private static readonly LoggerEventDelegate<string> _serviceStarting =
            LoggerEvent.Define<string>(
                LogLevel.Trace,
                LogEventIds.ServiceStarting,
                "Starting service {serviceType} ...");

        private static readonly LoggerEventDelegate<string, long> _serviceStarted =
            LoggerEvent.Define<string, long>(
                LogLevel.Info,
                LogEventIds.ServiceStarted,
                "Started service {serviceType} in {elapsedTime} ms.");

        private static readonly LoggerEventDelegate<string, long> _serviceStartFailed =
            LoggerEvent.Define<string, long>(
                LogLevel.Error,
                LogEventIds.ServiceStartFailed,
                "Failed to start service {serviceType} after {elapsedTime} ms.");

        private static readonly LoggerEventDelegate<string> _serviceStopping =
            LoggerEvent.Define<string>(
                LogLevel.Trace,
                LogEventIds.ServiceStopping,
                "Stopping service {serviceType} ...");

        private static readonly LoggerEventDelegate<string, long> _serviceStopped =
            LoggerEvent.Define<string, long>(
                LogLevel.Info,
                LogEventIds.ServiceStopped,
                "Stopped service {serviceType} in {elapsedTime} ms.");

        private static readonly LoggerEventDelegate<string, long> _serviceStopFailed =
            LoggerEvent.Define<string, long>(
                LogLevel.Error,
                LogEventIds.ServiceStopFailed,
                "Failed to stop service {serviceType} after {elapsedTime} ms.");

        public static void ServiceStarting(this ILogger logger, IBackgroundService service)
        {
            _serviceStarting(logger, service.GetType().GetDisplayName());
        }

        public static void ServiceStarted(this ILogger logger, IBackgroundService service, TimeSpan elapsed)
        {
            _serviceStarted(
                logger,
                service.GetType().GetDisplayName(),
                (long) elapsed.TotalMilliseconds);
        }

        public static void ServiceStartFailed(this ILogger logger, IBackgroundService service, TimeSpan elapsed, Exception exception)
        {
            _serviceStartFailed(
                logger,
                service.GetType().GetDisplayName(),
                (long) elapsed.TotalMilliseconds,
                exception: exception);
        }

        public static void ServiceStopping(this ILogger logger, IBackgroundService service)
        {
            _serviceStopping(logger, service.GetType().GetDisplayName());
        }

        public static void ServiceStopped(this ILogger logger, IBackgroundService service, TimeSpan elapsed)
        {
            _serviceStopped(
                logger,
                service.GetType().GetDisplayName(),
                (long) elapsed.TotalMilliseconds);
        }

        public static void ServiceStopFailed(this ILogger logger, IBackgroundService service, TimeSpan elapsed, Exception exception)
        {
            _serviceStopFailed(
                logger,
                service.GetType().GetDisplayName(),
                (long) elapsed.TotalMilliseconds,
                exception: exception);
        }
    }
}
