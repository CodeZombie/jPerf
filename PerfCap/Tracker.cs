using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jPerf
{
    class Tracker
    {
        private string name;
        private Func<Double> getNewSampleFunction;
        private List<Sample> Samples;
        private System.Drawing.Color Color;
        private int SmoothMode;

        private List<Sample> SmoothSamples;
        public Tracker(string name, System.Drawing.Color Color, Func<Double> getNewSampleFunction)
        {
            Console.WriteLine("New Tracker Created");
            this.SmoothMode = 0;
            this.name = name;
            this.Color = Color;
            this.Samples = new List<Sample>() {};
            this.SmoothSamples = new List<Sample>() { };
            this.getNewSampleFunction = getNewSampleFunction;
        }

        public void SetSmoothMode(int SmoothMode)
        {
            this.SmoothMode = SmoothMode;
            double RunningValue = 0;
            double RunningTime = 0;
            int MergeSize = (SmoothMode == 1 ? 4 : 8);
            //process results;

            this.SmoothSamples.Clear();
            for (int i = 0; i < this.Samples.Count(); i++)
            {
                if (i % MergeSize == 0)
                {
                    RunningValue = 0;
                    RunningTime = 0;
                }
                RunningTime += this.Samples[i].GetTime();
                RunningValue += this.Samples[i].GetValue();
                if (i % MergeSize == (MergeSize - 1) || i == (this.Samples.Count() - 1))
                {
                    this.SmoothSamples.Add(new Sample(RunningValue / ((i % MergeSize) + 1), RunningTime / ((i % MergeSize) + 1)));
                }
            }
        }

        public void Update(double time)
        {
            this.AddSample(new Sample(this.getNewSampleFunction(), time));
        }

        public List<Sample> GetSamples()
        {
            if(this.SmoothMode == 0)
            {
                return this.Samples;
            }
            else
            {
                return this.SmoothSamples;
            }
            
        }

        public void AddSample(Sample Sample)
        {
            this.Samples.Add(Sample);
        }
        
        public void AddSamples(List<Sample> Samples)
        {
            this.Samples.AddRange(Samples);
        }

        public string GetName()
        {
            return this.name;
        }

        public System.Drawing.Color GetColor()
        {
            return Color;
        }

        public void Clear()
        {
            this.Samples.Clear();
        }

        public object ToObject()
        {
            List<Object> SampleObjects = new List<object>();
            foreach (Sample S in this.Samples)
            {
                SampleObjects.Add(S.ToObject());
            }
            return new
                {
                    Samples = SampleObjects,
                    Color_A = this.Color.A,
                    Color_R = this.Color.R,
                    Color_G = this.Color.G,
                    Color_B = this.Color.B,
                    Name = this.name
                };
        }
    }
}
