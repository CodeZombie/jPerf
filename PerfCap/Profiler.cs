using Newtonsoft.Json;
using PerfCap;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jPerf
{
    class Profiler
    {
        private List<Tracker> Trackers;
        private Stopwatch TimeKeeper;
        private List<Marker> Markers;
        private DateTime StartTime;
        private bool ShowMarkers;

        public Profiler()
        {
            Console.WriteLine("New Profiler Created");
            //instantiate trackers
            Trackers = new List<Tracker>();
            this.ShowMarkers = true;
            //create stopwatch
            TimeKeeper = new Stopwatch();
            ResetTimeKeeper();

            //Inisntantiate Markers
            Markers = new List<Marker>();
        }

        public void SetSmoothMode(int Mode)
        {
            foreach (Tracker Tracker in this.Trackers)
            {
                Tracker.SetSmoothMode(Mode);
            }
        }

        public static Profiler FromJson(string JsonString)
        {
            Profiler NewProfiler = new Profiler();

            dynamic data = JsonConvert.DeserializeObject<dynamic>(JsonString);

            NewProfiler.StartTime = data.StartTime;

            //Add Markers
            foreach (dynamic m in data.Markers)
            {
                NewProfiler.Markers.Add(new Marker((string)m.Name, (double)m.Time));
            }
            
            //create profilers:
            foreach (dynamic o in data.Trackers)
            {
                string ProfilerName = o.Name;
                List<Sample> Samples = Sample.FromDynamicList(o.Samples.ToObject<List<object>>());
                int Color_A = o.Color_A;
                int Color_R = o.Color_R;
                int Color_G = o.Color_G;
                int Color_B = o.Color_B;
                System.Drawing.Color Color = System.Drawing.Color.FromArgb(Color_A, Color_R, Color_G, Color_B);

                Tracker Tracker = new Tracker(ProfilerName, Color, () => { return 0.0; });
                Tracker.AddSamples(Samples);
                NewProfiler.AddTracker(Tracker);
            }

            return NewProfiler;
        }
        public void SetShowMarkers(bool ShowMarkers)
        {
            this.ShowMarkers = ShowMarkers;
        }
        private void ResetTimeKeeper()
        {
            TimeKeeper.Reset();
        }

        public void StartRecording()
        {
            this.StartTime = DateTime.Now;
            TimeKeeper.Start();
        }

        public DateTime GetStartTime()
        {
            return this.StartTime;
        }

        public void AddMarker(string Name)
        {
            Console.WriteLine("Added marker");
            this.Markers.Add(new Marker(Name, this.TimeKeeper.Elapsed.TotalMilliseconds));
        }

        public List<Marker> GetMarkers()
        {
            return this.ShowMarkers ? this.Markers : new List<Marker>() { };
        }

        public double GetElapsedTime()
        {
            return this.TimeKeeper.Elapsed.TotalMilliseconds;
        }

        public void StopRecording()
        {
            this.TimeKeeper.Stop();
        }

        public void AddTracker(Tracker T)
        {
            this.Trackers.Add(T);
        }

        public void Update()
        {
            if(this.TimeKeeper.IsRunning)
            {
                //all trackers mark new points.
                foreach (Tracker T in Trackers)
                {
                    T.Update(this.TimeKeeper.Elapsed.TotalMilliseconds);
                }
            }
        }

        public List<Tracker> GetTrackers()
        {
            return Trackers;
        }

        public double GetNumberOfSamples()
        {
            double SampleCount = 0;
            foreach(Tracker T in this.Trackers)
            {
                SampleCount += T.GetSamples().Count();
            }
            return SampleCount;
        }

        public string ToJson()
        {
            List<object> TrackerData = new List<object>();
            foreach (Tracker T in this.GetTrackers())
            {
                TrackerData.Add(T.ToObject());
            }

            return JsonConvert.SerializeObject(new
            {
                this.StartTime,
                this.Markers,
                Trackers = TrackerData
            });
        }

        public void AddMarkerFile(string JsonString)
        {
            Profiler NewProfiler = new Profiler();

            dynamic data = JsonConvert.DeserializeObject<dynamic>(JsonString);

            foreach(dynamic M in data)
            {
                DateTime Time = DateTime.ParseExact((string)M.time, "yyyy-MM-dd HH:mm:ss", new System.Globalization.CultureInfo("en-US"));
                //get offset, in millis, between the time this profiler was started, and the time in startTime
                Console.WriteLine("Adding Marker: " + (string)M.title + " || " + Time.ToLongTimeString());
                this.Markers.Add(new PerfCap.Marker((string)M.title, Time.Subtract(this.StartTime).TotalMilliseconds));
            }
        }

        ~Profiler()
        {
            //clean up..
            
        }
    }
}
