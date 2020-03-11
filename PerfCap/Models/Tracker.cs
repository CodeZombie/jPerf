using Newtonsoft.Json;
using PerfCap.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jPerf
{
    public class Tracker
    {
        private Func<Double> captureSampleFunction { get; }
        public List<Sample> Samples { get; set; }
        public string Name { get; set; }
        public System.Drawing.Color DrawColor { get; }
        
        public Tracker(string name, System.Drawing.Color drawColor, Func<Double> captureSampleFunction, Log log)
        {
            log.AddLine("Creating Tracker");

            this.Name = name;
            this.DrawColor = drawColor;
            this.Samples = new List<Sample>() {};
            this.captureSampleFunction = captureSampleFunction;
        }

        public void CaptureSample(double time)
        {
            this.Samples.Add(new Sample(this.captureSampleFunction(), time));
        }

        public void AddSample(Sample sample)
        {
            this.Samples.Add(sample);
        }

        public Tracker Smooth(SmoothMode smoothMode, Log log)
        {
            log.AddLine("Smoothing Tracker");

            Tracker SmoothTracker = new Tracker(this.Name, this.DrawColor, null, log);

            double RunningValue = 0;
            double RunningTime = 0;
            int MergeSize = (smoothMode == SmoothMode.Moderate ? 4 : 8);

            for (int i = 0; i < this.Samples.Count(); i++)
            {
                if (i % MergeSize == 0)
                {
                    RunningValue = 0;
                    RunningTime = 0;
                }
                RunningTime += this.Samples[i].Time;
                RunningValue += this.Samples[i].Value;
                if (i % MergeSize == (MergeSize - 1) || i == (this.Samples.Count() - 1))
                {
                    SmoothTracker.Samples.Add(new Sample(RunningValue / ((i % MergeSize) + 1), RunningTime / ((i % MergeSize) + 1)));
                }
            }

            return SmoothTracker;
        }

        public object ToObject(Log log)
        {
            log.AddLine("Converting Tracker to generic object");

            List<Object> SampleObjects = new List<object>();
            foreach (Sample S in this.Samples)
            {
                SampleObjects.Add(S.ToObject());
            }
            return new
                {
                    Samples = SampleObjects,
                    Color_A = this.DrawColor.A,
                    Color_R = this.DrawColor.R,
                    Color_G = this.DrawColor.G,
                    Color_B = this.DrawColor.B,
                    Name = this.Name
                };
        }
    }
}
