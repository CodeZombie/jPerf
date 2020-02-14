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
        private List<Double> SampleTimes;
        private List<Double> SampleValues;
        private System.Drawing.Color Color;

        public Tracker(string name, System.Drawing.Color Color, Func<Double> getNewSampleFunction)
        {
            Console.WriteLine("New Tracker Created");
            this.name = name;
            this.Color = Color;
            this.SampleTimes = new List<Double>() {};
            this.SampleValues = new List<Double>() {};
            this.getNewSampleFunction = getNewSampleFunction;
        }

        public void Update(Int64 time)
        {
            this.SampleTimes.Add(time);
            this.SampleValues.Add(this.getNewSampleFunction());
        }

        public Double[] GetSampleTimes()
        {

            return this.SampleTimes.ToArray();
        }

        public Double[] GetSampleValues()
        {

            return this.SampleValues.ToArray();
        }

        public void SetSampleTimes(List<Double> SampleTimes)
        {
            this.SampleTimes = SampleTimes;
        }

        public void SetSampleValues(List<Double> SampleValues)
        {
            this.SampleValues = SampleValues;
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
            this.SampleTimes.Clear();
            this.SampleValues.Clear();
        }

        public object ToObject()
        {
            return new
                {
                    SampleTimes = this.SampleTimes,
                    SampleValues = this.SampleValues,
                    Color_A = this.Color.A,
                    Color_R = this.Color.R,
                    Color_G = this.Color.G,
                    Color_B = this.Color.B,
                    Name = this.name
                };
        }
    }
}
