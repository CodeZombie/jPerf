using jPerf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;

namespace PerfCap.Model
{
    public enum ProfilerState
    {
        Ready,
        Recording,
        Stopped
    }

    public enum SmoothMode
    {
        None,
        Moderate,
        Heavy
    }

    public class Profiler
    {
        public List<Tracker> Trackers;
        public List<Marker> Markers { get; set; }
        public Stopwatch Stopwatch { get; set; }            //Keeps track of how many milliseconds have passed while recording
        private double lastSampleCaptureTime;
        public DateTime StartTime { get; set; }             //The exact moment the profiler began recording
        private Timer updateTimer;                          //Triggers the UpdateLoop() function at a regular interval
        public int CaptureInterval { get; set; }            //Roughly how many milliseconds between each capture.
        public ProfilerState State { get; set; }            //Ready, Recording, Stopped.
        private Log log;

        public Profiler(ProfilerState profilerState, Log log)
        {
            this.log = log;
            this.log.AddLine("New Profiler Created");
            Trackers = new List<Tracker>();
            Markers = new List<Marker>();
            Stopwatch = new Stopwatch();
            State = profilerState;
            Stopwatch.Reset();
            CaptureInterval = 500;

            updateTimer = new Timer();
            updateTimer.Interval = 10;
            updateTimer.Tick += UpdateLoop;
            updateTimer.Enabled = true;
        }

        public int GetSampleCount()
        {
            int count = 0;
            foreach(Tracker tracker in Trackers)
            {
                count += tracker.Samples.Count();
            }
            return count;
        }

        public void UpdateLoop(object s, EventArgs e)
        {
            
            double elapsedTime = this.Stopwatch.Elapsed.TotalMilliseconds;
            if (this.State == ProfilerState.Recording && elapsedTime - lastSampleCaptureTime > CaptureInterval)
            {
                this.log.AddLine("Capturing Samples");
                foreach (Tracker tracker in Trackers)
                {
                    tracker.CaptureSample(this.Stopwatch.Elapsed.TotalMilliseconds);
                }
                this.lastSampleCaptureTime = elapsedTime;
            }
        }

        public void StartRecording()
        {
            this.log.AddLine("Started Recording...");
            if (this.State != ProfilerState.Ready)
            {
                this.log.AddLine("WARNING: Cannot start recording - Profiler not ready");
                return;
            }

            this.State = ProfilerState.Recording;
            this.Stopwatch.Start();
            this.StartTime = DateTime.Now;
            this.updateTimer.Start();
        }

        public void StopRecording()
        {
            this.log.AddLine("Stopping Recording...");
            if (this.State != ProfilerState.Recording)
            {
                this.log.AddLine("WARNING: Cannot stop recording - Recording not started.");
                return;
            }

            this.State = ProfilerState.Stopped;
            this.Stopwatch.Stop();
        }
    }
}
