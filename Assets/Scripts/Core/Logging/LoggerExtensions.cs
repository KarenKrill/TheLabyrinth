using UnityEngine;

namespace KarenKrill.Core.Logging
{
    internal static class LoggerExtensions
    {
        public static void LogWarning(this ILogger logger, object message)
        {
            logger.LogWarning(string.Empty, message);
        }
    }
}
