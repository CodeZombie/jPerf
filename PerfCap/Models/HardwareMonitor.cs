using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfCap.Model
{
    public abstract class HardwareMonitor
    {
        public HardwareMonitor() { }

        public abstract double GetNextValue();
    }
}
