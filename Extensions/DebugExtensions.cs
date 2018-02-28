using System;

namespace VanillaRuleGenerator.Extensions
{
    //Faking the Debug calls to make direct copy/paste code much easier.
    public static class Debug
    {
		public delegate void LogMessageDelegate(string message);
	    public static LogMessageDelegate LogMessageHandler;



		//public static string log = string.Empty;

		public static void Log(string message)
        {
	        LogMessageHandler?.Invoke(message);
        }

        public static void Log(string message, Object context)
        {
	        LogMessageHandler?.Invoke(string.Format(message, context));
        }

	    public static void LogError(object message){ }
        public static void LogError(object message, Object context){}

        public static void LogErrorFormat(string format, params object[] args){}
        public static void LogErrorFormat(Object context, string format, params object[] args){}

	    public delegate void LogExceptionDelegate(Exception exception, string message);
	    public static LogExceptionDelegate LogExceptionHandler;

		public static void LogException(Exception exception)
	    {
		    LogExceptionHandler?.Invoke(exception, "An exception has occured:");
	    }

	    public static void LogException(Exception exception, Object context)
	    {
		    if (context is string message)
		    {
			    LogExceptionHandler?.Invoke(exception, message);
		    }
		    else
		    {
			    LogException(exception);
		    }
	    }

        public static void LogFormat(string format, params object[] args)
        {
			LogMessageHandler?.Invoke(string.Format(format, args));
		}

	    public static void LogFormat(Object context, string format, params object[] args)
	    {
		    LogFormat(format, args);
	    }

		public static void LogWarning(object message){}
        public static void LogWarning(object message, Object context){}

        public static void LogWarningFormat(string format, params object[] args){}
        public static void LogWarningFormat(Object context, string format, params object[] args){}

        public static void LogAssertionFormat(string format, params object[] args){}
        public static void LogAssertionFormat(Object context, string format, params object[] args){}

        public static void LogAssertion(object message){}
        public static void LogAssertion(object message, Object context){}
    }
}