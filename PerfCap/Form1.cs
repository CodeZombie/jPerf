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

namespace jPerf
{
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer UpdateTimer;
        private Stopwatch UpdateStopWatch;
        private double LastProfilerUpdateTime;
        Profiler Profiler;
        private int TrackerUpdateSpeed = 500; //make sure this is divisible by 10.
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

            //Create a new Profiler and give it some Trackers to use by default.
            DefineDefaultProfiler();

            //Initialize update timer
            UpdateTimer = new System.Windows.Forms.Timer();
            UpdateTimer.Interval = (int)Math.Floor(TrackerUpdateSpeed / 10.0);
            UpdateTimer.Tick += UpdateTimer_Tick;
            UpdateTimer.Enabled = true;

            //Initialize Update Stopwatch:
            UpdateStopWatch = new Stopwatch();
            UpdateStopWatch.Start();
            LastProfilerUpdateTime = UpdateStopWatch.Elapsed.TotalMilliseconds;
        }

        private void DefineDefaultProfiler()
        {

            //Clear Series from OxyPlot:
            plotView1.Model.Series.Clear();

            Profiler = new Profiler(TrackerUpdateSpeed);

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

            AddTracker(new Tracker("GPU Load", System.Drawing.Color.FromArgb(255, 244, 67, 54), () => { GpuLoad.Hardware.Update(); return (double)GpuLoad.Value; }));
            AddTracker(new Tracker("CPU Load", System.Drawing.Color.FromArgb(255, 33, 150, 243), () => { return 100.0 - (double)cpuCounter.NextValue(); }));
            ReactToProfilerStateChange();
        }

        private void Redraw()
        {

            //its really quite simple.
            //we find out where the bar is, and only draw a few points around it.
            //Console.WriteLine(plotView1.Model.);
            //Apply every data point from each Tracker to a corresponding Chart Series.
            for (var tracker_i = 0; tracker_i < Profiler.GetTrackers().Count(); tracker_i++)
            {
                LineSeries LineSeries = ((LineSeries)plotView1.Model.Series[tracker_i]);
                LineSeries.Points.Clear();
                Tracker Tracker = Profiler.GetTrackers()[tracker_i];
                for (int sample_i = 0; sample_i < Tracker.GetSampleTimes().Count(); sample_i++)
                {
                    LineSeries.Points.Add(new DataPoint(Tracker.GetSampleTimes()[sample_i] / 1000, Tracker.GetSampleValues()[sample_i]));
                }
            }

            plotView1.Model.InvalidatePlot(true);
            //plotView1.Model.Axes[0].Zoom((programLoopStopwatch.ElapsedMilliseconds-10000)/1000, (programLoopStopwatch.ElapsedMilliseconds/1000));
            ((IPlotModel)plotView1.Model).Update(true);
            //plotView1.Model.InvalidatePlot(true);
        }

        private void AddTracker(Tracker T)
        {
            Profiler.AddTracker(T);

            plotView1.Model.Series.Add(new LineSeries()
            {
                Color = OxyColor.FromArgb(T.GetColor().A, T.GetColor().R, T.GetColor().G, T.GetColor().B),
                MarkerType = MarkerType.None,
                MarkerSize = 3,
                MarkerStroke = OxyColor.FromArgb(T.GetColor().A, T.GetColor().R, T.GetColor().G, T.GetColor().B),
                MarkerStrokeThickness = 1.5,
                Title = T.GetName(),
            });
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            double CurrentTime = UpdateStopWatch.Elapsed.TotalMilliseconds;
            if(CurrentTime - LastProfilerUpdateTime > TrackerUpdateSpeed)
            {
                Profiler.Update();
                LastProfilerUpdateTime = CurrentTime;
            }
        }


        private void ReactToProfilerStateChange()
        {
            //get new state and do something based on that...
            switch (Profiler.GetState())
            {
                case ProfilerState.Ready:
                    startRecordingToolStripMenuItem.Enabled = true;
                    stopRecordingToolStripMenuItem.Enabled = false;
                    this.Text = "jPerf (Ready)";
                    label1.Text = "Ready.";
                    label1.Show();
                    plotView1.Hide();
                    break;
                case ProfilerState.Recording:
                    startRecordingToolStripMenuItem.Enabled = false;
                    stopRecordingToolStripMenuItem.Enabled = true;
                    this.Text = "jPerf (Recording...) - *";
                    label1.Text = "Recording...";
                    break;
                case ProfilerState.Stopped:
                    startRecordingToolStripMenuItem.Enabled = false;
                    stopRecordingToolStripMenuItem.Enabled = false;
                    this.Text = "jPerf (Stopped) - *";
                    label1.Hide();
                    plotView1.Show();
                    Redraw();
                    break;
            }
        }

        private void startRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Profiler.StartRecording();
            ReactToProfilerStateChange();
        }

        private void stopRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Profiler.StopRecording();
            ReactToProfilerStateChange();
        }

        //SAVE
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Profiler.StopRecording();
            ReactToProfilerStateChange();

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "jPerf Capture|*.jpc|JSON File|*.json";
            saveFileDialog1.Title = "Save the capture file";
            saveFileDialog1.AddExtension = true;
            saveFileDialog1.ShowDialog();

            if(saveFileDialog1.FileName != "")
            {
                List<object> data = new List<object>();
                foreach(Tracker T in Profiler.GetTrackers())
                {
                    data.Add(T.ToObject());
                }
                File.WriteAllText(saveFileDialog1.FileName, JsonConvert.SerializeObject(data));
                this.Text = "jPerf - " + saveFileDialog1.FileName;
            }
        }

        //OPEN 
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Filter = "jPerf Capture|*.jpc|JSON File|*.json";
            openFileDialog1.Title = "Open a capture file";
            openFileDialog1.ShowDialog();

            if(openFileDialog1.FileName != "")
            {
                string TextData;
                FileStream Stream = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                using (StreamReader Reader = new StreamReader(Stream, Encoding.UTF8))
                {
                    TextData = Reader.ReadToEnd();
                }
                List<object> data = JsonConvert.DeserializeObject<List<object>>(TextData);
                Profiler = new Profiler(TrackerUpdateSpeed);
                plotView1.Model.Series.Clear();

                //create profilers:
                foreach (dynamic o in data)
                {
                    string ProfilerName = o.Name;
                    List<Double> FileSampleTimes = o.SampleTimes.ToObject<List<Double>>();
                    List<Double> FileSampleValues = o.SampleValues.ToObject<List<Double>>();
                    int Color_A = o.Color_A;
                    int Color_R = o.Color_R;
                    int Color_G = o.Color_G;
                    int Color_B = o.Color_B;
                    System.Drawing.Color Color = System.Drawing.Color.FromArgb(Color_A, Color_R, Color_G, Color_B);
                    Tracker T = new Tracker(ProfilerName, Color, () => { return 0.0; });
                    T.SetSampleTimes(FileSampleTimes);
                    T.SetSampleValues(FileSampleValues);
                    AddTracker(T);

                    this.Profiler.StopRecording();
                    ReactToProfilerStateChange();
                    this.Text = "jPerf - " + openFileDialog1.FileName;
                }
            }
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DefineDefaultProfiler();
            ReactToProfilerStateChange();
            Redraw();
        }
    }
}
