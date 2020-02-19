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

        public Tracker(string name, System.Drawing.Color Color, Func<Double> getNewSampleFunction)
        {
            Console.WriteLine("New Tracker Created");
            this.name = name;
            this.Color = Color;
            this.Samples = new List<Sample>() {};
            this.getNewSampleFunction = getNewSampleFunction;
        }

        public void Update(double time)
        {
            this.AddSample(new Sample(this.getNewSampleFunction(), time));
        }

        public List<Sample> GetSamples()
        {
            return this.Samples;
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
