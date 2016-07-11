using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace ULA.Quijano.ProcesoLegal.Commons
{
    public class UltimusLogs
    {

        public static Logger logger;

        private LogEventInfo vLogEventInfo;

        public string IDLogger { get; set; }
        public string METHODLogger { get; set; }
        public string LoggerName { get; set; }

        public string INITLogger
        {
            get { return string.Format("{0}|{1}", METHODLogger, IDLogger); }
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static string GetMyMethodName()
        {
            var st = new StackTrace(new StackFrame(1));
            return st.GetFrame(0).GetMethod().Name;
        }

        public UltimusLogs(string vLoggerName)
        {

            logger = LogManager.GetLogger(vLoggerName);
            LoggerName = vLoggerName;
        }

        public void Trace(string Message)
        {

            Message = Message.Replace(System.Environment.NewLine, " -> ");

            vLogEventInfo = new LogEventInfo(LogLevel.Trace, LoggerName, Message);
            vLogEventInfo.Properties["INITLogger"] = INITLogger;

            logger.Log(vLogEventInfo);

        }

        public void Error(string Message)
        {

            Message = Message.Replace(System.Environment.NewLine, " -> ");

            vLogEventInfo = new LogEventInfo(LogLevel.Error, LoggerName, Message);
            vLogEventInfo.Properties["INITLogger"] = INITLogger;

            logger.Log(vLogEventInfo);

        }
    }
}