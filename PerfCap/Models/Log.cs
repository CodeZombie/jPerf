using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfCap.Model
{
    public class Log
    {
        public string Text { get; set; }
        private int length;
        public List<ILogSubscriber> Subscribers { get; set; }

        public Log()
        {
            Subscribers = new List<ILogSubscriber>();
            Text = DateTime.Now.ToShortTimeString() + ": Log started.";
            length = 1;
        }

        public void AddLine(string s)
        {
            if (length > 10000)
            {
                Text = "";
            }

            Text += Environment.NewLine + DateTime.UtcNow.ToLocalTime().ToShortTimeString() + ": " + s;
            length++;

            foreach (ILogSubscriber subscriber in Subscribers)
            {
                subscriber.UpdateLog(this);
            }
        }
    }
}
