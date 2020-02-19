using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jPerf
{
    class Sample
    {
        private double Value;
        private double Time;

        public static List<Sample> FromDynamicList(List<dynamic> ObjectList)
        {
            List<Sample> SampleList = new List<Sample>();
            foreach(dynamic O in ObjectList)
            {
                SampleList.Add(new Sample(O.Value.ToObject<Double>(), O.Time.ToObject<Double>()));
            }
            return SampleList;
        }

        public Sample(double Value, double Time)
        {
            Console.WriteLine("Created Sample: " + Value.ToString() + " | Time: " + Time.ToString());
            this.Value = Value;
            this.Time = Time;
        }

        public double GetValue()
        {
            return this.Value;
        }

        public double GetTime()
        {
            return this.Time;
        }

        public object ToObject()
        {
            return new
            {
                this.Value,
                this.Time
            };
        }
    }
}
