using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace LOG
{
    public interface ILogger
    {
        void Info(string message);
        void Debug(string message);
    }
}
