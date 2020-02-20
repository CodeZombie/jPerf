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

        public RadioButtonGroup(List<ToolStripRadioButton> RadioButtons)
        {
            this.RadioButtons = RadioButtons;
            foreach(ToolStripRadioButton RadioButton in RadioButtons)
            {
                RadioButton.Click += (sender, e) =>
                {
                    foreach (ToolStripRadioButton B in RadioButtons)
                    {
                        B.SetTicked(false);
                    }
                    RadioButton.SetTicked(true);
                };
                RadioButton.UpdateText();
            }
            RadioButtons[0].SetTicked(true);
        }

        public void Reset()
        {
            foreach (ToolStripRadioButton Button in RadioButtons)
            {
                Button.SetTicked(false);
            }
            RadioButtons[0].SetTicked(true);
        }

    }
}
