using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerfCap.View
{
    class MarkerPrompt : Form
    {
        private TextBox textBox1;
        private Button okayButton;
        private Button cancelButton;
        private Label label1;
        public string ReturnValue;

        public MarkerPrompt()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.okayButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name of new marker:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(15, 25);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(257, 20);
            this.textBox1.TabIndex = 1;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // okayButton
            // 
            this.okayButton.Location = new System.Drawing.Point(197, 51);
            this.okayButton.Name = "okayButton";
            this.okayButton.Size = new System.Drawing.Size(75, 23);
            this.okayButton.TabIndex = 2;
            this.okayButton.Text = "Okay";
            this.okayButton.UseVisualStyleBackColor = true;
            this.okayButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Location = new System.Drawing.Point(116, 51);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // MarkerPrompt
            // 
            this.ClientSize = new System.Drawing.Size(284, 90);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.okayButton);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "MarkerPrompt";
            this.Text = "Create new marker...";
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ReturnValue = this.textBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(this.textBox1.Text != "")
            {
                this.okayButton.Enabled = true;
            }
            else
            {
                this.okayButton.Enabled = false;
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
