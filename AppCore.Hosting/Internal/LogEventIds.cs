using AppCore.Logging;

namespace AppCore.Hosting
{
    internal class LogEventIds
    {
        public static readonly LogEventId ServiceStarting = new LogEventId(0, nameof(ServiceStarting));

        public static readonly LogEventId ServiceStarted = new LogEventId(1, nameof(ServiceStarted));

        public static readonly LogEventId ServiceStartFailed = new LogEventId(2, nameof(ServiceStartFailed));

        public static readonly LogEventId ServiceStopping = new LogEventId(0, nameof(ServiceStopping));

        public static readonly LogEventId ServiceStopped = new LogEventId(1, nameof(ServiceStopped));

        public static readonly LogEventId ServiceStopFailed = new LogEventId(2, nameof(ServiceStopFailed));
    }
}
