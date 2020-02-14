using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jPerf
{
    enum ProfilerState
    {
        Ready,
        Recording,
        Paused,
        Stopped
    }
    class Profiler
    {
        private List<Tracker> Trackers;
        private Stopwatch TimeKeeper;
        private ProfilerState State;
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
            this.State = ProfilerState.Ready;
        }

        private void ResetTimeKeeper()
        {
            TimeKeeper.Reset();
        }

        public void StartRecording()
        {
            if (this.State == ProfilerState.Ready)
            {
                TimeKeeper.Start();
                this.State = ProfilerState.Recording;
            }
            else if(this.State == ProfilerState.Paused)
            {
                this.State = ProfilerState.Recording;
            }
        }

        public void StopRecording()
        {
            this.State = ProfilerState.Stopped;
        }

        public void AddTracker(Tracker T)
        {
            this.Trackers.Add(T);
        }

        public void Update()
        {
            if(this.State == ProfilerState.Recording)
            {
                //all trackers mark new points.
                foreach (Tracker T in Trackers)
                {
                    T.Update(Convert.ToInt64(this.TimeKeeper.Elapsed.TotalMilliseconds));
                }
            }
        }

        public List<Tracker> GetTrackers()
        {
            return Trackers;
        }

        public void SetState(ProfilerState State)
        {
            this.State = State;
        }
        public ProfilerState GetState()
        {
            return this.State;
        }

        ~Profiler()
        {
            //clean up..
            
        }
    }
}
