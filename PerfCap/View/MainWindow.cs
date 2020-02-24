using LibreHardwareMonitor;
using LibreHardwareMonitor.Hardware;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using PerfCap;
using PerfCap.Controller;
using PerfCap.Model;
using PerfCap.View;

namespace jPerf
{
    public partial class MainWindow : Form
    {
        private SampleChart sampleChart;
        private ProfilerController profilerController;
        private Timer viewUpdateTimer;

        public MainWindow()
        {
            
            InitializeComponent();
            this.sampleChart = new SampleChart(plotView1);
            this.profilerController = new ProfilerController();
            UpdateView();

            viewUpdateTimer = new Timer();
            viewUpdateTimer.Interval = 250;
            viewUpdateTimer.Enabled = true;
            viewUpdateTimer.Tick += ViewUpdateTimer_Tick;
        }

        private void ViewUpdateTimer_Tick(object sender, EventArgs e)
        {
            if(this.profilerController.Profiler.State == ProfilerState.Recording)
            {
                this.sampleCountStatusLabel.Text = "Samples: " + this.profilerController.Profiler.GetSampleCount().ToString();
                this.markerCountStatusLabel.Text = "Markers: " + this.profilerController.Profiler.Markers.Count().ToString();
            }
        }

        public void UpdateView()
        {
            if (this.profilerController.Profiler.SmoothMode == SmoothMode.None)
            {
                this.noneToolStripMenuItem.Checked = true;
                this.moderateToolStripMenuItem.Checked = false;
                this.highToolStripMenuItem.Checked = false;
            }else if (this.profilerController.Profiler.SmoothMode == SmoothMode.Moderate)
            {
                this.noneToolStripMenuItem.Checked = false;
                this.moderateToolStripMenuItem.Checked = true;
                this.highToolStripMenuItem.Checked = false;
            }
            else
            {
                this.noneToolStripMenuItem.Checked = false;
                this.moderateToolStripMenuItem.Checked = false;
                this.highToolStripMenuItem.Checked = true;
            }

            if (this.profilerController.Profiler.ShowMarkers)
            {
                this.showMarkersToolStripMenuItem.Checked = true;
            }
            else
            {
                this.showMarkersToolStripMenuItem.Checked = false;
            }

            if(this.profilerController.Profiler.State == ProfilerState.Ready)
            {
                this.pictureBox1.Show();
                this.plotView1.Hide();
                this.startRecordingToolStripMenuItem.Enabled = true;
                this.stopRecordingToolStripMenuItem.Enabled = false;
                this.addMarkerToolStripMenuItem.Enabled = false;
                this.markersToolStripMenuItem.Enabled = false;
                this.saveToolStripMenuItem.Enabled = false;
                this.showMarkersToolStripMenuItem.Enabled = false;
                this.smoothModeToolStripMenuItem.Enabled = false;
                this.AddMarkerButton.Enabled = false;
                this.sampleCountStatusLabel.Text = "Samples: 0";
                this.markerCountStatusLabel.Text = "Markers: 0";
                this.Text = "jPerf (Ready)";
                this.statusText.Text = "Ready";
                statusIcon.ForeColor = System.Drawing.Color.Green;
                this.statusIcon.Text = "✅";
            }
            else if(this.profilerController.Profiler.State == ProfilerState.Recording)
            {
                this.plotView1.Hide();
                this.pictureBox1.Show();
                this.stopRecordingToolStripMenuItem.Enabled = true;
                this.addMarkerToolStripMenuItem.Enabled = true;
                this.markersToolStripMenuItem.Enabled = true;
                this.saveToolStripMenuItem.Enabled = false;
                this.showMarkersToolStripMenuItem.Enabled = false;
                this.smoothModeToolStripMenuItem.Enabled = false;
                this.AddMarkerButton.Enabled = true;

                this.Text = "jPerf (Recording...) - *";
                this.statusText.Text = "Recording...";
                statusIcon.ForeColor = System.Drawing.Color.Red;
                this.statusIcon.Text = "⚫";
            }else
            {
                this.pictureBox1.Hide();
                this.plotView1.Show();
                this.sampleChart.Draw(this.profilerController.Profiler);
                this.stopRecordingToolStripMenuItem.Enabled = false;
                this.addMarkerToolStripMenuItem.Enabled = false;
                this.markersToolStripMenuItem.Enabled = false;
                this.saveToolStripMenuItem.Enabled = true;
                this.showMarkersToolStripMenuItem.Enabled = true;
                this.smoothModeToolStripMenuItem.Enabled = true;
                this.AddMarkerButton.Enabled = false;
                Text = "jPerf (Stopped) - *";
                statusText.Text = "Stopped";
                statusIcon.ForeColor = System.Drawing.Color.Black;
                statusIcon.Text = "⬛";
            }

            if(this.profilerController.Profiler.State == ProfilerState.Stopped || this.profilerController.Profiler.State == ProfilerState.Recording)
            {
                this.startRecordingToolStripMenuItem.Enabled = false;
                this.sampleCountStatusLabel.Text = "Samples: " + this.profilerController.Profiler.GetSampleCount().ToString();
                this.markerCountStatusLabel.Text = "Markers: " + this.profilerController.Profiler.Markers.Count().ToString();
            }

        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.profilerController.NewProfiler();
            UpdateView();
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.profilerController.SetSmoothMode(SmoothMode.None);
            UpdateView();
        }

        private void moderateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.profilerController.SetSmoothMode(SmoothMode.Moderate);
            UpdateView();
        }

        private void highToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.profilerController.SetSmoothMode(SmoothMode.Heavy);
            UpdateView();
        }

        private void showMarkersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.profilerController.ToggleMarkers();
            UpdateView();
        }

        private void addFlagButton_Click(object sender, EventArgs e)
        {
            MarkerPrompt markerPrompt = new MarkerPrompt();
            if(markerPrompt.ShowDialog() == DialogResult.OK)
            {
                profilerController.Profiler.Markers.Add(new Marker(markerPrompt.ReturnValue, profilerController.Profiler.Stopwatch.Elapsed.Milliseconds));
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void startRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.profilerController.StartRecording();
            UpdateView();
        }

        private void stopRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.profilerController.StopRecording();
            UpdateView();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
