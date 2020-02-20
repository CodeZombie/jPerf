using LibreHardwareMonitor;
using LibreHardwareMonitor.Hardware;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using PerfCap;
using OxyPlot.Annotations;

namespace jPerf
{

    enum JPerfStates
    {
        Ready,
        Recording,
        Stopped
    };

    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer UpdateTimer;
        private Stopwatch UpdateStopWatch;
        private double LastProfilerUpdateTime;
        Profiler Profiler;
        private int TrackerUpdateSpeed = 500; //make sure this is divisible by 10.

        private StateMachine StateMachine;

        private RadioButtonGroup RadioButtonGroup;
        public Form1()
        {
            InitializeComponent();

            //Definet the type of plot OxyPlot will use:
            plotView1.Model = new PlotModel
            {
                PlotType = PlotType.Cartesian,
                Background = OxyColors.White
            };

            //Define OxyPlot's X-Axis
            plotView1.Model.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                AbsoluteMinimum = 0,
                Minimum = 0,
                Maximum = 10000,
            });

            //Define OxyPlot's Y-Axis
            plotView1.Model.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                AbsoluteMinimum = 0,
                AbsoluteMaximum = 100,
                Minimum = 0,
                Maximum = 100,
                IsZoomEnabled = false,
            });

            //Create new state machine, register the default state, and run it:
            StateMachine = new StateMachine(new State((int)JPerfStates.Ready, () =>
            {
                //Clear Series from OxyPlot:
                plotView1.Model.Series.Clear();


                //Create a new profiler. The old one will be garbage collected.
                Profiler = new Profiler();

                //Create radio/checkbox buttons:
                showMarkersToolStripMenuItem.SetTicked(true);

                showMarkersToolStripMenuItem.OnTicked(() =>
                {
                    Console.WriteLine("SHOW MARKERS TICKED");
                });

                showMarkersToolStripMenuItem.OnUnticked(() =>
                {
                    Console.WriteLine("SHOW MARKERS UN TICKED");

                });

                ///None
                noneToolStripMenuItem.OnTicked(() =>
                {
                    Profiler.SetSmoothMode(0);
                    Redraw();
                });
                //Moderate
                moderateToolStripMenuItem.OnTicked(() =>
                {
                    Profiler.SetSmoothMode(1);
                    Redraw();
                });
                //High
                highToolStripMenuItem.OnTicked(() =>
                {
                    Profiler.SetSmoothMode(2);
                    Redraw();
                });

                RadioButtonGroup = new RadioButtonGroup(new List<ToolStripRadioButton>
                {
                    noneToolStripMenuItem,
                    moderateToolStripMenuItem,
                    highToolStripMenuItem
                });

                //Initialize Computer (For LibreHardwareMonitor)
                Computer Computer = new Computer();
                Computer.Open();
                Computer.IsGpuEnabled = true;
                Computer.IsCpuEnabled = true;

                ISensor GpuLoad = Computer.Hardware
                  .First(h => h.HardwareType == HardwareType.GpuAmd || h.HardwareType == HardwareType.GpuNvidia)
                  .Sensors.First(s => s.SensorType == SensorType.Load);

                //Initialize the CPU Idle time monitor from HWI.
                PerformanceCounter cpuCounter = new PerformanceCounter("Processor", "% Idle Time", "_Total");
                cpuCounter.NextValue();

                Profiler.AddTracker(new Tracker("GPU Load", System.Drawing.Color.FromArgb(255, 244, 67, 54), () => { GpuLoad.Hardware.Update(); return (double)GpuLoad.Value; }));
                Profiler.AddTracker(new Tracker("CPU Load", System.Drawing.Color.FromArgb(255, 33, 150, 243), () => { return 100.0 - (double)cpuCounter.NextValue(); }));

                addFlagButton.Enabled = false;
                jPXLogFileToolStripMenuItem.Enabled = false;
                startRecordingToolStripMenuItem.Enabled = true;
                stopRecordingToolStripMenuItem.Enabled = false;
                this.Text = "jPerf (Ready)";
                label1.Clear();
                label1.UpdateText("base", "Ready.");
                label1.Show();
                plotView1.Hide();
                RadioButtonGroup.Reset();
            }));

            //register the other states:
            StateMachine.AddState(new State((int)JPerfStates.Recording, () =>
            {
                Profiler.StartRecording();
                addFlagButton.Enabled = true;
                jPXLogFileToolStripMenuItem.Enabled = false;
                startRecordingToolStripMenuItem.Enabled = false;
                stopRecordingToolStripMenuItem.Enabled = true;
                this.Text = "jPerf (Recording...) - *";
                label1.UpdateText("base", "Recording...");
                label1.UpdateText("StartTime", "Started: " + Profiler.GetStartTime().ToLongTimeString());
                UpdateStopWatch.Start();
            }));

            StateMachine.AddState(new State((int)JPerfStates.Stopped, () =>
            {
                Profiler.StopRecording();

                addFlagButton.Enabled = false;
                jPXLogFileToolStripMenuItem.Enabled = true;
                startRecordingToolStripMenuItem.Enabled = false;
                stopRecordingToolStripMenuItem.Enabled = false;
                this.Text = "jPerf (Stopped) - *";
                label1.Hide();
                plotView1.Show();
                plotView1.Model.Axes[0].Zoom(0, Math.Floor(Profiler.GetElapsedTime()/1000));
                Redraw();
                RadioButtonGroup.Reset();
            }));

            //Initialize update timer
            UpdateTimer = new System.Windows.Forms.Timer();
            UpdateTimer.Interval = (int)Math.Floor(TrackerUpdateSpeed / 10.0);
            UpdateTimer.Tick += UpdateTimer_Tick;
            UpdateTimer.Enabled = true;

            //Initialize Update Stopwatch:
            UpdateStopWatch = new Stopwatch();
            LastProfilerUpdateTime = UpdateStopWatch.Elapsed.TotalMilliseconds;
        }

        private void Redraw()
        {
            //Clear and re-add all series:
            plotView1.Model.Series.Clear();
            foreach (Tracker Tracker in Profiler.GetTrackers())
            {
                plotView1.Model.Series.Add(new LineSeries()
                {
                    Color = OxyColor.FromArgb(Tracker.GetColor().A, Tracker.GetColor().R, Tracker.GetColor().G, Tracker.GetColor().B),
                    MarkerType = MarkerType.None,
                    MarkerSize = 3,
                    MarkerStroke = OxyColor.FromArgb(Tracker.GetColor().A, Tracker.GetColor().R, Tracker.GetColor().G, Tracker.GetColor().B),
                    MarkerStrokeThickness = 1.5,
                    Title = Tracker.GetName(),
                });
            }



            //For every tracker in the profiler...
            for (var tracker_i = 0; tracker_i < this.Profiler.GetTrackers().Count(); tracker_i++)
            {
                LineSeries LineSeries = (LineSeries)plotView1.Model.Series[tracker_i];
                LineSeries.Points.Clear();
                Tracker Tracker = Profiler.GetTrackers()[tracker_i];

                ///for every sample in each tracker...
                foreach(Sample Sample in Tracker.GetSamples())
                {
                    LineSeries.Points.Add(new DataPoint(Sample.GetTime() / 1000, Sample.GetValue()));
                }
            }

            //Markers...
            plotView1.Model.Annotations.Clear();

            foreach(Marker Marker in this.Profiler.GetMarkers())
            {
                plotView1.Model.Annotations.Add(new LineAnnotation()
                {
                    StrokeThickness = 1,
                    Color = OxyColors.Green,
                    Type = LineAnnotationType.Vertical,
                    Font = "Segoe",
                    LineStyle = LineStyle.LongDash,
                    FontSize = 10,
                    Text = Marker.Name + " (" + Math.Round(Marker.Time / 1000, 2).ToString() + " s)",
                    TextColor = OxyColors.Black,
                    X = Marker.Time / 1000
                });
            }

            plotView1.Model.InvalidatePlot(true);
            ((IPlotModel)plotView1.Model).Update(true);
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            double CurrentTime = UpdateStopWatch.Elapsed.TotalMilliseconds;
            if(StateMachine.GetCurrentStateIndex() == (int)JPerfStates.Recording &&  CurrentTime - LastProfilerUpdateTime > TrackerUpdateSpeed)
            {
                Profiler.Update();
                LastProfilerUpdateTime = CurrentTime;
                label1.UpdateText("SampleCount", Profiler.GetNumberOfSamples().ToString() + " samples");
                label1.UpdateText("TimeCount", Profiler.GetElapsedTime() + " ms");
            }
        }

        private void startRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StateMachine.SetState((int)JPerfStates.Recording);
        }

        private void stopRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StateMachine.SetState((int)JPerfStates.Stopped);
        }

        //Open
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "jPerf Capture|*.jpc|JSON File|*.json";
            openFileDialog1.Title = "Open a capture file";
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "")
            {
                string TextData;
                FileStream Stream = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                using (StreamReader Reader = new StreamReader(Stream, Encoding.UTF8))
                {
                    TextData = Reader.ReadToEnd();
                }
                
                Profiler = Profiler.FromJson(TextData);
                plotView1.Model.Series.Clear();

                this.StateMachine.SetState((int)JPerfStates.Stopped);
                this.Text = "jPerf - " + openFileDialog1.FileName;
            }
        }

        //Save
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StateMachine.SetState((int)JPerfStates.Stopped);

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "jPerf Capture|*.jpc|JSON File|*.json";
            saveFileDialog1.Title = "Save the capture file";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.ShowDialog();

            if (saveFileDialog1.FileName != "")
            {
                File.WriteAllText(saveFileDialog1.FileName, this.Profiler.ToJson());
                this.Text = "jPerf - " + saveFileDialog1.FileName;
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.StateMachine.SetState((int)JPerfStates.Ready);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(StateMachine.GetCurrentStateIndex() != (int)JPerfStates.Ready)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to exit? Unsaved data will be lost.", "Exit?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }

            Application.Exit();
        }

        private void addFlagButton_Click(object sender, EventArgs e)
        {
            double Time = this.Profiler.GetElapsedTime();

            Form Prompt = new Form()
            {
                Width = 304,
                Height = 132,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = "New Marker...",
                StartPosition = FormStartPosition.CenterParent
            };

            //Text Box
            TextBox TextBox = new TextBox() { Left = 16, Top = 32, Width = 256 };
            Prompt.Controls.Add(TextBox);
            TextBox.KeyUp += (object SenderObject, KeyEventArgs KeyEventArgs) => {
                if(KeyEventArgs.KeyCode == Keys.Return)
                {
                    Prompt.DialogResult = DialogResult.OK;
                    Prompt.Close();
                }
            };

            //Label
            Prompt.Controls.Add(new Label() { Left = 14, Top = 16, Text = "Name of marker: " });

            //Confirm Button
            Button ConfirmationButton = new Button() { Text = "Ok", Left = 128, Width = 64, Top = 64, DialogResult = DialogResult.OK };
            Prompt.Controls.Add(ConfirmationButton);
            ConfirmationButton.Click += (s, ea) => { Prompt.Close(); };

            //Cancel Button
            Button CloseButton = new Button() { Text = "Cancel", Left = 208, Width = 64, Top = 64, DialogResult = DialogResult.Cancel };
            Prompt.Controls.Add(CloseButton);
            CloseButton.Click += (s, ea) => { Prompt.Close(); };

            if (Prompt.ShowDialog() == DialogResult.OK)
            {
                this.Profiler.AddMarker(TextBox.Text);
            }
        }

        private void jPMLogFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "jPerf Marker File|*.jpm|JSON File|*.json";
            openFileDialog1.Title = "Open Marker file";
            openFileDialog1.ShowDialog();

            if (openFileDialog1.FileName != "")
            {
                string TextData;
                FileStream Stream = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                using (StreamReader Reader = new StreamReader(Stream, Encoding.UTF8))
                {
                    TextData = Reader.ReadToEnd();
                }

                this.Profiler.AddMarkerFile(TextData);
            }
            Redraw();
        }
    }
}
