using PerfCap.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jPerf
{
    public class Sample
    {
        public double Value { get; }
        public double Time { get; }

        public static List<Sample> FromDynamicList(List<dynamic> ObjectList, Log log)
        {
            log.AddLine("Creating list of Samples from JSON list");
            List<Sample> SampleList = new List<Sample>();
            foreach(dynamic O in ObjectList)
            {
                SampleList.Add(new Sample(O.Value.ToObject<Double>(), O.Time.ToObject<Double>()));
            }
            return SampleList;
        }

        public Sample(double value, double time)
        {
            this.Value = value;
            this.Time = time;
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
