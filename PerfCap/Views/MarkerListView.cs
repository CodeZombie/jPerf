using PerfCap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace jPerf.Views
{
    class MarkerListView : Form
    {
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel selectionRangeLabel;
        private ListView listView1;
        public List<Marker> Markers { get; set; }

        public MarkerListView(List<Marker> markers)
        {
            InitializeComponent();
            listView1.Columns.Add("Name", 250);
            listView1.Columns.Add("Time (seconds)", 125);

            Markers = markers;

            foreach (Marker marker in Markers)
            {
                string[] data = { marker.Name, Math.Round(marker.Time / 1000, 2).ToString()};
                listView1.Items.Add(new ListViewItem(data));
            }
        }

        private void InitializeComponent()
        {
            this.listView1 = new System.Windows.Forms.ListView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.selectionRangeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.FullRowSelect = true;
            this.listView1.GridLines = true;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(0, 0);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(393, 424);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.Details;
            this.listView1.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listView1_ItemSelectionChanged);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectionRangeLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 402);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(393, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // selectionRangeLabel
            // 
            this.selectionRangeLabel.Name = "selectionRangeLabel";
            this.selectionRangeLabel.Size = new System.Drawing.Size(97, 17);
            this.selectionRangeLabel.Text = "Selection Range: ";
            // 
            // MarkerListView
            // 
            this.ClientSize = new System.Drawing.Size(393, 424);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.listView1);
            this.Name = "MarkerListView";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(listView1.SelectedIndices.Count < 2)
            {
                selectionRangeLabel.Text = "Selection Range: ";
                return;
            }

            double lowestTime = -1;
            double highestTime = -1;
            foreach (int index in listView1.SelectedIndices)
            {
                if(lowestTime == -1 || Markers[index].Time < lowestTime)
                {
                    lowestTime = Markers[index].Time;
                }

                if (highestTime == -1 || Markers[index].Time > highestTime)
                {
                    highestTime = Markers[index].Time;
                }

                selectionRangeLabel.Text = "Selection Range: " + Math.Round((highestTime - lowestTime) / 1000, 2).ToString() + " (seconds)";
            }
        }
    }
}
