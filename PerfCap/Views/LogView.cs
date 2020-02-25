using PerfCap.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PerfCap.View
{
    public partial class  LogView : Form, ILogSubscriber
    {
        private TextBox textBox1;

        public LogView(Log log)
        {
            InitializeComponent();
            log.Subscribers.Add(this);
            this.UpdateLog(log);
        }

        public void UpdateLog(Log log)
        {
            this.textBox1.Text = log.Text;
            this.textBox1.SelectionStart = this.textBox1.TextLength;
            this.textBox1.ScrollToCaret();
        }

        private void InitializeComponent()
        {
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(284, 261);
            this.textBox1.TabIndex = 0;
            // 
            // LogView
            // 
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.textBox1);
            this.Name = "LogView";
            this.Text = "jPerf Log";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
