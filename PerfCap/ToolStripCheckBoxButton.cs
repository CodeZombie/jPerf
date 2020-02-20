using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerfCap
{
    class ToolStripCheckBoxButton : ToolStripMenuItem
    {
        public bool Ticked;
        public string BaseText;
        private Action OnTickedAction;
        private Action OnUntickedAction;

        public ToolStripCheckBoxButton (string BaseText)
        {
            this.BaseText = BaseText;
            this.Ticked = false;
            this.Click += (sender, e) =>
            {
                this.Toggle();
                if (this.Ticked)
                {
                    this.OnTickedAction?.Invoke();
                }
                else
                {
                    this.OnUntickedAction?.Invoke();
                }
                
            };

            this.UpdateText();
        }

        public Boolean Toggle()
        {
            SetTicked(!this.Ticked);
            return this.Ticked;
        }

        public void SetTicked(bool Val)
        {
            this.Ticked = Val;
            this.UpdateText();
        }

        private void UpdateText()
        {
             this.Text = (this.Ticked ? "☑ " : "☐ ").ToString() + this.BaseText;
        }

        public void OnTicked(Action OnTickedAction)
        {
            this.OnTickedAction = OnTickedAction;
        }

        public void OnUnticked(Action OnUntickedAction)
        {
            this.OnUntickedAction = OnUntickedAction;
        }
    }
}
