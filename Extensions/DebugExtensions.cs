using System;
using RT.Util;

namespace VanillaRuleGenerator.Extensions
{
    //Faking the Debug calls to make direct copy/paste code much easier.
    public static class Debug
    {
	    public static LoggerBase Logger;

	    public static void Log(string message)
	    {
		    LogFormat(message);
	    }

	    public static void LogFormat(string format, params object[] args)
	    {
		    Logger?.Log(1, LogType.Info, string.Format(format, args));
	    }

		public static void LogException(Exception exception, string message = "An exception has occured:", LogType logType = LogType.Error, uint verbosity=1)
	    {
		    Logger?.Log(verbosity, logType, message);
		    Logger?.Exception(exception, verbosity, logType);
	    }

        
    }
}