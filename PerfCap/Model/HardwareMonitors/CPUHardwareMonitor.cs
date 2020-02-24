using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfCap.Model
{
    public class CPUHardwareMonitor : HardwareMonitor
    {
        private PerformanceCounter counter;
        public CPUHardwareMonitor()
        {
            counter = new PerformanceCounter("Processor", "% Idle Time", "_Total");
            counter.NextValue();
        }

        public override double GetNextValue()
        {
            return (double)counter.NextValue();
        }
    }
}
