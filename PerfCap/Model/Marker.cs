using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfCap
{
    public class Marker
    {
        public string Name { get; }
        public double Time { get; }

        public Marker(String Name, Double Time)
        {
            this.Name = Name;
            this.Time = Time;
        }

        public object ToObject()
        {
            return new
            {
                this.Name,
                this.Time
            };
        }
    }
}