using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerfCap
{
    class RadioButtonGroup
    {

        List<ToolStripRadioButton> RadioButtons;
        private int Selected;
                     
        public RadioButtonGroup(List<ToolStripRadioButton> RadioButtons)
        {
            this.Selected = 0;
            this.RadioButtons = RadioButtons;
            foreach (ToolStripRadioButton Button in RadioButtons)
            {
                Button.Click += (sender, e) =>
                {
                    foreach (ToolStripRadioButton B in RadioButtons)
                    {
                        B.UpdateText(false);
                    }
                    Button.UpdateText(true);
                    Button.OnTicked();
                    this.Select(Button.GetIndex());
                };
            }
            this.Reset();
        }

        public void Reset()
        {
            foreach (ToolStripRadioButton Button in RadioButtons)
            {
                Button.UpdateText(false);
            }
            RadioButtons[0].UpdateText(true);
        }

        public void Select(int Index)
        {
            Console.WriteLine("SMooth Sleecetd: " + Index.ToString());
            this.Selected = Index;

        }

        public int GetSelected()
        {
            return this.Selected;
        }


        public void SetEnabled(bool Enabled)
        {
            foreach (ToolStripRadioButton Button in RadioButtons)
            {
                Button.Enabled = Enabled;
            }
        }
    }
}
