using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOG
{
    public class Logger : ILogger
    {
        private  log4net.ILog _log;

        public Logger(Type t)
        {
            _log = LogManager.GetLogger(t);
            log4net.Config.XmlConfigurator.Configure();
        }
        public void Debug(string message)
        {
           _log.Debug(message);
        }

        public void Info(string message)
        {
            _log.Info(message);
        }
    }
}
