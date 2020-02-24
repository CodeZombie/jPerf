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
        public List<Tracker> SmoothTrackers { get; set; }
        public List<Marker> Markers { get; set; }
        public bool ShowMarkers { get; set; }
        public Stopwatch Stopwatch { get; set; }
        public double lastSampleCaptureTime {get; set;}
        public DateTime StartTime { get; set; }
        public ProfilerState State { get; set; }
        public int CaptureInterval { get; set; } //How many milliseconds in between sample captures

        public SmoothMode SmoothMode;

        public Profiler(ProfilerState profilerState)
        {
            Trackers = new List<Tracker>();
            SmoothTrackers = new List<Tracker>();
            Markers = new List<Marker>();
            Stopwatch = new Stopwatch();
            ShowMarkers = true;
            SetSmoothMode(SmoothMode.None);
            State = profilerState;
            Stopwatch.Reset();
            CaptureInterval = 500;
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

        public List<Tracker> GetTrackers()
        {
            if(this.SmoothMode == SmoothMode.None)
            {
                return this.Trackers;
            }
            else
            {
                return this.SmoothTrackers;
            }
        }

        public void SetSmoothMode(SmoothMode smoothMode)
        {
            this.SmoothMode = smoothMode;

            SmoothTrackers.Clear();
            foreach(Tracker tracker in Trackers)
            {
                SmoothTrackers.Add(tracker.Smooth(this.SmoothMode));
            }
        }

        public void CaptureSamples()
        {
            double elapsedTime = this.Stopwatch.Elapsed.TotalMilliseconds;
            if (this.State == ProfilerState.Recording && elapsedTime - lastSampleCaptureTime > CaptureInterval)
            {
                foreach (Tracker tracker in Trackers)
                {
                    tracker.CaptureSample(this.Stopwatch.Elapsed.Milliseconds);
                }
                this.lastSampleCaptureTime = elapsedTime;
            }
        }

        public void StartRecording()
        {
            if(this.State != ProfilerState.Ready)
            {
                Console.WriteLine("MESSAGE: Cannot start recording - Profiler not ready.");
                return;
            }

            this.State = ProfilerState.Recording;
            this.Stopwatch.Start();
            this.StartTime = DateTime.Now;
        }

        public void StopRecording()
        {
            if(this.State != ProfilerState.Recording)
            {
                Console.WriteLine("MESSAGE: Cannot stop recording - Recording not started.");
                return;
            }

            this.State = ProfilerState.Stopped;
            this.Stopwatch.Stop();
        }
    }
}
