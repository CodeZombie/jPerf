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

        public ToolStripRadioButton(string BaseText, int Index)
        {
            this.Index = Index;
            this.BaseText = BaseText;
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
