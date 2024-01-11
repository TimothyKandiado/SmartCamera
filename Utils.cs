//#define DEBUG_LOG_ENABLED

using FlaxEngine;

namespace Tmore.SmartCamera;

public static class Utils
{
    internal static void LogMessage(object message)
    {
#if DEBUG_LOG_ENABLED
        Debug.Log(message);
#endif
    }
}