using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfCap.Model.HardwareMonitors
{

    public class RAMHardwareMonitor : HardwareMonitor
    {
        private PerformanceCounter counter;

        public RAMHardwareMonitor()
        {
            counter = new PerformanceCounter("Memory", "% Committed Bytes In Use");
            counter.NextValue();
        }

        public override double GetNextValue()
        {
            return (double)counter.NextValue();
        }
    }


}
