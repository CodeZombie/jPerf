using jPerf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Windows.Forms;
using Newtonsoft.Json;

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
        public string name { get; set; }

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
            name = "*";
        }
        public int GetRecordingLength()
        {
            Console.WriteLine(Trackers.Count());
            if (Trackers.Count() == 0)
            {
                return 0;
            }

            if (Trackers[0].Samples.Count() == 0)
            {
                return 0;
            }

            return (int)Math.Floor(Trackers[0].Samples.Last().Time);
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
            if (this.State != ProfilerState.Recording)
            {
                return;
            }

            this.log.AddLine("Stopping Recording...");
            this.State = ProfilerState.Stopped;
            this.Stopwatch.Stop();
        }

        public void AddMarkerFile(string JsonString, Log log)
        {
            log.AddLine("Parsing jPerf Marker File");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(JsonString);

            foreach (dynamic marker in data)
            {
                DateTime time = DateTime.ParseExact((string)marker.time, "yyyy-MM-dd HH:mm:ss", new System.Globalization.CultureInfo("en-US"));
                this.Markers.Add(new Marker( (string)marker.title, time.Subtract(this.StartTime).TotalMilliseconds, log));
            }
        }

        public static Profiler FromJson(string JsonString, Log log)
        {
            log.AddLine("Parsing jPerf Capture File");
            Profiler newProfiler = new Profiler(ProfilerState.Stopped, log);

            dynamic data = JsonConvert.DeserializeObject<dynamic>(JsonString);

            newProfiler.StartTime = data.StartTime;

            //Add Markers
            foreach (dynamic m in data.Markers)
            {
                newProfiler.Markers.Add(new Marker((string)m.Name, (double)m.Time, log));
            }

            //create profilers:
            foreach (dynamic o in data.Trackers)
            {
                string profilerName = o.Name;
                List<Sample> Samples = Sample.FromDynamicList(o.Samples.ToObject<List<object>>(), log);
                int Color_A = o.Color_A;
                int Color_R = o.Color_R;
                int Color_G = o.Color_G;
                int Color_B = o.Color_B;
                System.Drawing.Color Color = System.Drawing.Color.FromArgb(Color_A, Color_R, Color_G, Color_B);

                Tracker tracker = new Tracker(profilerName, Color, () => { return 0.0; }, log);
                tracker.Samples.AddRange(Samples);
                newProfiler.Trackers.Add(tracker);
            }
            return newProfiler;
        }

        public string ToJson(Log log)
        {
            log.AddLine("Converting Profiler object to JSON");
            List<object> trackerData = new List<object>();
            foreach (Tracker tracker in Trackers)
            {
                trackerData.Add(tracker.ToObject(log));
            }

            return JsonConvert.SerializeObject(new
            {
                this.StartTime,
                this.Markers,
                Trackers = trackerData
            });
        }

    }
}
