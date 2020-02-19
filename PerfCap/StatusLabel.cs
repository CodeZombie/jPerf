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
        private String BaseText;

        public StatusLabel()
        {
            BaseText = "";
        }

        public void SetBaseText(string BaseText)
        {
            this.BaseText = BaseText;
            this.Text = BaseText;
        }
        
        public void Update(string ExtraText)
        {
            this.Text = BaseText + "\n" + ExtraText;
        }

    }
}
