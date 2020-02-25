using PerfCap.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfCap
{
    public interface ILogSubscriber
    {
        void UpdateLog(Log log);
    }
}
