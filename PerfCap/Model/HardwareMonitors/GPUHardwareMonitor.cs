using LibreHardwareMonitor.Hardware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfCap.Model
{
    public class GPUHardwareMonitor : HardwareMonitor
    {
        private Computer computer;
        private ISensor GpuLoad;
        public GPUHardwareMonitor()
        {
            computer = new Computer();
            computer.Open();
            computer.IsGpuEnabled = true;
            computer.IsCpuEnabled = true;

            GpuLoad = computer.Hardware
                  .First(h => h.HardwareType == HardwareType.GpuAmd || h.HardwareType == HardwareType.GpuNvidia)
                  .Sensors.First(s => s.SensorType == SensorType.Load);
        }

        public override double GetNextValue()
        {
            GpuLoad.Hardware.Update();
            return (double)GpuLoad.Value;
        }
    }
}
