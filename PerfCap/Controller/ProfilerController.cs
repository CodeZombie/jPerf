using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot.Annotations;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using PerfCap.Model;
using System.Diagnostics;
using System.Windows.Forms;
using jPerf;
using PerfCap.Model.HardwareMonitors;

namespace PerfCap.Controller
{
    public class ProfilerController
    {
        public Profiler Profiler { get; set; }
        public Timer updateTimer { get; set; }
        public List<HardwareMonitor> HardwareMonitors { get; set; }

        public ProfilerController()
        {
            HardwareMonitors = new List<HardwareMonitor>();
            HardwareMonitors.Add(new CPUHardwareMonitor());
            HardwareMonitors.Add(new GPUHardwareMonitor());
            HardwareMonitors.Add(new RAMHardwareMonitor());
            
            updateTimer = new Timer();
            updateTimer.Interval = 10;
            updateTimer.Tick += UpdateTimerTick;
            updateTimer.Enabled = true;

            NewProfiler();
        }

        private void UpdateTimerTick(object sender, EventArgs e)
        {
            this.Profiler.CaptureSamples();
        }

        public void NewProfiler()
        {
            Console.WriteLine("New");
            this.Profiler = new Profiler(ProfilerState.Ready);
            this.Profiler.Trackers.Add(new Tracker("CPU Load", System.Drawing.Color.Blue, () => HardwareMonitors[0].GetNextValue()));
            this.Profiler.Trackers.Add(new Tracker("GPU Load", System.Drawing.Color.Red, () => HardwareMonitors[1].GetNextValue()));
            this.Profiler.Trackers.Add(new Tracker("RAM Usage", System.Drawing.Color.Green, () => HardwareMonitors[2].GetNextValue()));
        }
        public void StartRecording()
        {
            Console.WriteLine("Start Recording");
            this.Profiler.StartRecording();
            this.updateTimer.Start();
        }
        public void StopRecording()
        {
            this.Profiler.StopRecording();
        }
        public void SetSmoothMode(SmoothMode smoothMode)
        {
            this.Profiler.SetSmoothMode(smoothMode);
        }
        public void ToggleMarkers()
        {
            this.Profiler.ShowMarkers = !this.Profiler.ShowMarkers;
        }
    }
}
