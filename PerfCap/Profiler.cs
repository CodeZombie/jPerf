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
        private int StepInterval;

        public Profiler(int StepInterval)
        {
            Console.WriteLine("New Profiler Created");
            //instantiate trackers
            Trackers = new List<Tracker>();
            this.StepInterval = StepInterval;

            //create stopwatch
            TimeKeeper = new Stopwatch();
            ResetTimeKeeper();
        }

        private void ResetTimeKeeper()
        {
            TimeKeeper.Reset();
        }

        public void StartRecording()
        {
            TimeKeeper.Start();
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

        ~Profiler()
        {
            //clean up..
            
        }
    }
}
