using PerfCap.Model;
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

        public Marker(String name, Double time, Log log)
        {
            this.Name = name;
            this.Time = time;
            log.AddLine("Marker '" + name + "' created at: " + time.ToString());
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