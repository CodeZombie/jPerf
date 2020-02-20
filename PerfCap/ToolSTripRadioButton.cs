using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerfCap
{
    class ToolStripRadioButton : ToolStripMenuItem
    {

        private bool Ticked;
        private string BaseText;
        private Action OnTickedAction;
        private Action OnUntickedAction;

        public ToolStripRadioButton(string BaseText)
        {
            Ticked = false;
            this.BaseText = BaseText;
            UpdateText();
        }

        public void UpdateText()
        {
            this.Text = (this.Ticked ? "⬤ " : "○ ").ToString() + this.BaseText;
        }

        public bool IsTicked()
        {
            return Ticked;
        }

        public void SetTicked(bool Ticked)
        {
            if(this.Ticked != Ticked)
            {
                (Ticked ? this.OnTickedAction : this.OnUntickedAction)?.Invoke();
            }

            this.Ticked = Ticked;
            UpdateText();
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
