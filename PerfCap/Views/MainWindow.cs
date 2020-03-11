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
using PerfCap.Model.HardwareMonitors;
using PerfCap.Views;

namespace jPerf
{
    public partial class MainWindow : Form
    {
        private SampleChart sampleChart;
        private Dictionary<String, HardwareMonitor> hardwareMonitors;
        private Profiler profiler;
        private Timer viewUpdateTimer;
        private Log log;
        private SmoothMode smoothMode;
        private TimeUnit timeUnit;

        public MainWindow()
        {
            InitializeComponent();
            log = new Log();
            hardwareMonitors = new Dictionary<string, HardwareMonitor>();
            hardwareMonitors.Add("CPULoad", new CPUHardwareMonitor());
            hardwareMonitors.Add("GPULoad", new GPUHardwareMonitor());
            hardwareMonitors.Add("RAMLoad", new RAMHardwareMonitor());

            this.sampleChart = new SampleChart(plotView1);
            ResetProfiler();
            smoothMode = SmoothMode.None;
            timeUnit = TimeUnit.Seconds;

            UpdateView();

            viewUpdateTimer = new Timer();
            viewUpdateTimer.Interval = 1000;
            viewUpdateTimer.Enabled = true;
            viewUpdateTimer.Tick += (object o, EventArgs e) => { UpdateView(); };
        }

        private void ResetProfiler()
        {
            this.profiler = new Profiler(ProfilerState.Ready, log);
            this.profiler.Trackers.Add(new Tracker("CPU Load", System.Drawing.Color.Blue, () => hardwareMonitors["CPULoad"].GetNextValue(), log));
            this.profiler.Trackers.Add(new Tracker("GPU Load", System.Drawing.Color.Red, () => hardwareMonitors["GPULoad"].GetNextValue(), log));
            this.profiler.Trackers.Add(new Tracker("RAM Usage", System.Drawing.Color.Green, () => hardwareMonitors["RAMLoad"].GetNextValue(), log));
        }

        public void UpdateView(bool redrawChart = false)
        {
            startRecordingToolStripMenuItem.Enabled = startRecordingTooltipButton.Enabled   = this.profiler.State == ProfilerState.Ready;
            stopRecordingToolStripMenuItem.Enabled = stopRecordingTooltipButton.Enabled     = this.profiler.State == ProfilerState.Recording;
            addMarkerToolStripMenuItem.Enabled = AddMarkerButton.Enabled                    = this.profiler.State == ProfilerState.Recording;
            timeUnitToolStripMenuItem.Enabled                                               = this.profiler.State == ProfilerState.Stopped;
            markersToolStripMenuItem.Enabled                                                = this.profiler.State == ProfilerState.Stopped;
            saveToolStripMenuItem.Enabled                                                   = this.profiler.State != ProfilerState.Ready;
            showMarkersToolStripMenuItem.Enabled = this.smoothModeToolStripMenuItem.Enabled = this.profiler.State == ProfilerState.Stopped;
            jPMImportToolStripMenuItem.Enabled                                              = this.profiler.State == ProfilerState.Stopped;
            mergeJPerfCaptureFileJPCToolStripMenuItem.Enabled                               = this.profiler.State == ProfilerState.Stopped;
            resetViewToolStripMenuItem.Enabled                                              = this.profiler.State == ProfilerState.Stopped;

            sampleCountStatusLabel.Text = "Samples: " + (profiler.State != ProfilerState.Ready ? profiler.GetSampleCount().ToString() : "0");
            markerCountStatusLabel.Text = "Markers: " + (profiler.State != ProfilerState.Ready ? profiler.Markers.Count().ToString() : "0");

            timeUnitStatus.Text = "Time Units: " + (timeUnit == TimeUnit.Milliseconds ? "Milliseconds" : timeUnit == TimeUnit.Seconds ? "Seconds" : "Minutes");

            (profiler.State == ProfilerState.Stopped ? (Action)plotView1.Show : plotView1.Hide)();
            (profiler.State == ProfilerState.Stopped ? (Action)pictureBox1.Hide : pictureBox1.Show)();

            this.noneToolStripMenuItem.Checked = smoothMode == SmoothMode.None;
            this.moderateToolStripMenuItem.Checked = smoothMode == SmoothMode.Moderate;
            this.highToolStripMenuItem.Checked = smoothMode == SmoothMode.Heavy;

            this.millisecondsToolStripMenuItem.Checked = timeUnit == TimeUnit.Milliseconds;
            this.secondsToolStripMenuItem.Checked = timeUnit == TimeUnit.Seconds;
            this.minutesToolStripMenuItem.Checked = timeUnit == TimeUnit.Minutes;

            statusText.Text = ((profiler.State == ProfilerState.Ready) ? "Ready" : (profiler.State == ProfilerState.Recording) ? "Recording" : "Stopped");
            this.Text = "jPerf - " + profiler.name + " (" + statusText.Text + ")";
            this.statusIcon.Text = (profiler.State == ProfilerState.Ready) ? "✅" : (profiler.State == ProfilerState.Recording) ? "⚫" : "⬛";
            statusIcon.ForeColor = (profiler.State == ProfilerState.Ready) ? System.Drawing.Color.Green : (profiler.State == ProfilerState.Recording) ? System.Drawing.Color.Red : System.Drawing.Color.Black;

            if (redrawChart)
            {
                sampleChart.Draw(profiler, showMarkersToolStripMenuItem.Checked, smoothMode, timeUnit);
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ResetProfiler();
            UpdateView(true); //redraw an empty chart to decrease cpu load.
        }

        private void noneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            smoothMode = SmoothMode.None;
            UpdateView(true);
        }

        private void moderateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            smoothMode = SmoothMode.Moderate;
            UpdateView(true);
        }

        private void highToolStripMenuItem_Click(object sender, EventArgs e)
        {
            smoothMode = SmoothMode.Heavy;
            UpdateView(true);
        }

        private void showMarkersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateView(true);
        }

        private void addMarkerButton_Click(object sender, EventArgs e)
        {
            double currentTime = profiler.Stopwatch.Elapsed.TotalMilliseconds;
            MarkerPrompt markerPrompt = new MarkerPrompt();
            if(markerPrompt.ShowDialog() == DialogResult.OK)
            {
                profiler.Markers.Add(new Marker(markerPrompt.ReturnValue, currentTime, this.log));
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            log.AddLine("Saving capture");
            profiler.StopRecording();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "jPerf Capture|*.jpc|JSON File|*.json";
            saveFileDialog.Title = "Save the capture file";
            saveFileDialog.AddExtension = true;
            saveFileDialog.ShowDialog();

            if (saveFileDialog.FileName != "")
            {
                File.WriteAllText(saveFileDialog.FileName, profiler.ToJson(log));
                this.profiler.name =  saveFileDialog.FileName;
            }
            UpdateView(false);
        }

        private void startRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.profiler.StartRecording();
            UpdateView();
        }

        private void stopRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.profiler.StopRecording();
            UpdateView(true);
            sampleChart.Zoom(0, (int)Math.Floor(this.profiler.Stopwatch.Elapsed.TotalMilliseconds / SampleChart.TimeUnitDivisor(timeUnit)));
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void openLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LogView logView = new LogView(this.log);
            logView.Show();
        }

        private void millisecondsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timeUnit = TimeUnit.Milliseconds;
            UpdateView(true);
            sampleChart.Zoom(0, profiler.GetRecordingLength() / SampleChart.TimeUnitDivisor(timeUnit));
        }

        private void secondsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timeUnit = TimeUnit.Seconds;
            UpdateView(true);
            sampleChart.Zoom(0, profiler.GetRecordingLength() / SampleChart.TimeUnitDivisor(timeUnit));
        }

        private void minutesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timeUnit = TimeUnit.Minutes;
            UpdateView(true);
            sampleChart.Zoom(0, profiler.GetRecordingLength() / SampleChart.TimeUnitDivisor(timeUnit));
        }

        private void resetViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sampleChart.Zoom(0, profiler.GetRecordingLength() / SampleChart.TimeUnitDivisor(timeUnit));
        }

        private void jPMImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "jPerf Marker File|*.jpm|JSON File|*.json";
            openFileDialog.Title = "Open Marker file";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                string TextData;
                FileStream Stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                using (StreamReader Reader = new StreamReader(Stream, Encoding.UTF8))
                {
                    TextData = Reader.ReadToEnd();
                }
                this.profiler.AddMarkerFile(TextData, log);
                UpdateView(true);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "jPerf Capture|*.jpc|JSON File|*.json";
            openFileDialog.Title = "Open jPerf Capture File";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                string textData;
                FileStream stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    textData = reader.ReadToEnd();
                }

                this.profiler = Profiler.FromJson(textData, log);
                this.profiler.name = openFileDialog.FileName;
                UpdateView(true);
                sampleChart.Zoom(0, profiler.GetRecordingLength() / SampleChart.TimeUnitDivisor(timeUnit));
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutForm aboutForm = new AboutForm();
            aboutForm.Show();
        }

        private void mergeJPerfCaptureFileJPCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "jPerf Capture|*.jpc|JSON File|*.json";
            openFileDialog.Title = "Open and Merge jPerf Capture File";
            openFileDialog.ShowDialog();

            if (openFileDialog.FileName != "")
            {
                string textData;
                FileStream stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read);
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    textData = reader.ReadToEnd();
                }
                Profiler profilerToMerge = Profiler.FromJson(textData, log);

                //Open a window asking for a location (in seconds) in the active Profiler to merge to
                MergeTimePrompt mergeTimePrompt = new MergeTimePrompt();
                if (mergeTimePrompt.ShowDialog() == DialogResult.OK)
                {
                    this.profiler.MergeWithProfiler(profilerToMerge, decimal.ToDouble(mergeTimePrompt.ReturnValue) * 1000, log);
                    sampleChart.Draw(profiler, showMarkersToolStripMenuItem.Checked, smoothMode, timeUnit);
                }
            }
        }
    }
}
