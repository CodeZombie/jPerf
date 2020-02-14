using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jPerf
{
    class Sample
    {
        private double value;
        private double time;

        public Sample(double value, double time)
        {
            this.value = value;
            this.time = time;
        }

        public double getValue()
        {
            return this.value;
        }

        public double getTime()
        {
            return this.time;
        }
    }
}
