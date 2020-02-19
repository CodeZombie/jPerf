using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerfCap
{
    class StatusLabel : Label
    {
        private Dictionary<string, string> DynamicText;

        public StatusLabel()
        {
            this.DynamicText = new Dictionary<string, string>();
        }

        public void Clear()
        {
            this.DynamicText.Clear();
            this.Text = "";
        }

        public void UpdateText(string Key, string Value)
        {
            if (this.DynamicText.ContainsKey(Key))
            {
                this.DynamicText[Key] = Value;
            }
            else
            {
                this.DynamicText.Add(Key, Value);
            }

            this.Text = "";
            foreach (KeyValuePair<string, string> entry in this.DynamicText)
            {
                this.Text += entry.Value + "\n";
            }
        }

    }
}
