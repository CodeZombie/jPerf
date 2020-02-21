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
        private string BaseText;
        private Action OnTickedAction;
        private int Index;

        public ToolStripRadioButton()
        {
            this.Index = 69;
            this.BaseText = "Hello";
            UpdateText(false);
        }

        public int GetIndex()
        {
            return this.Index;
        }

        public void UpdateText(bool Ticked)
        {
            this.Text = (Ticked ? "⬤ " : "○ ").ToString() + this.BaseText;
        }

        public void SetOnTicked(Action OnTickedAction)
        {
            this.OnTickedAction = OnTickedAction;
        }

        public void OnTicked()
        {
            this.OnTickedAction?.Invoke();
        }
    }
}
