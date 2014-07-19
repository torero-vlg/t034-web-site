using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using NLog;

namespace Db.Tools
{
    static public class MonitorLog
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public enum typelog : int
        {
            Error = EventLogEntryType.Error,
            Info = EventLogEntryType.Information,
            Warn = EventLogEntryType.Warning
        }

        static public void WriteLog(string mes, typelog t, bool to_imp)
        {
            switch (t)
            {
                case typelog.Error:
                    logger.Error(mes + Environment.StackTrace + Environment.NewLine);
                    break;
                case typelog.Warn:
                    logger.Warn(mes + Environment.NewLine);
                    break;
                case typelog.Info:
                    logger.Info(mes);
                    break;
            }
        }

        static public void WriteLog(string mes, typelog t, int eventID, short category, bool to_imp)
        {
            WriteLog(mes, t, to_imp);
        }

        static public void StartLog(string source_name, string mes)
        {
            WriteLog(mes, typelog.Info, true);
        }

        static public void Close(string mes)
        {
            WriteLog(mes, typelog.Info, true);
        }

    }
}
