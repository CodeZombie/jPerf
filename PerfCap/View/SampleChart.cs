using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Annotations;
using OxyPlot.WindowsForms;
using PerfCap.Model;
using jPerf;

namespace PerfCap.Controller
{
    //This class will abstract away much of the complexities of working with Oxyplot.
    public class SampleChart
    {
        private PlotView plotView;

        public SampleChart (PlotView plotView)
        {
            this.plotView = plotView;
            this.plotView.Model = new PlotModel
            {
                PlotType = PlotType.Cartesian,
                Background = OxyColors.White
            };

            this.plotView.Model.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Bottom,
                AbsoluteMinimum = 0,
                Minimum = 0,
                Maximum = 10000,
            });

            this.plotView.Model.Axes.Add(new OxyPlot.Axes.LinearAxis
            {
                Position = OxyPlot.Axes.AxisPosition.Left,
                AbsoluteMinimum = 0,
                AbsoluteMaximum = 100,
                Minimum = 0,
                Maximum = 100,
                IsZoomEnabled = false,
            });
        }

        public void Draw(Profiler profiler)
        {
            //Clear and re-add all series:
            this.plotView.Model.Series.Clear();

            foreach (Tracker tracker in profiler.Trackers)
            {
                this.plotView.Model.Series.Add(new LineSeries()
                {
                    Color = OxyColor.FromArgb(tracker.DrawColor.A, tracker.DrawColor.R, tracker.DrawColor.G, tracker.DrawColor.B),
                    MarkerType = MarkerType.None,
                    MarkerSize = 3,
                    MarkerStroke = OxyColor.FromArgb(tracker.DrawColor.A, tracker.DrawColor.R, tracker.DrawColor.G, tracker.DrawColor.B),
                    MarkerStrokeThickness = 1.5,
                    Title = tracker.Name,
                });
            }

            //For every tracker in the profiler...
            for (var tracker_i = 0; tracker_i < profiler.GetTrackers().Count(); tracker_i++)
            {
                LineSeries lineSeries = (LineSeries)this.plotView.Model.Series[tracker_i];
                lineSeries.Points.Clear();
                Tracker tracker = profiler.Trackers[tracker_i];

                ///for every sample in each tracker...
                foreach (Sample sample in tracker.Samples)
                {
                    lineSeries.Points.Add(new DataPoint(sample.Time / 1000, sample.Value));
                }
            }

            //Markers...
            this.plotView.Model.Annotations.Clear();

            foreach (Marker marker in profiler.Markers)
            {
                this.plotView.Model.Annotations.Add(new LineAnnotation()
                {
                    StrokeThickness = 1,
                    Color = OxyColors.Green,
                    Type = LineAnnotationType.Vertical,
                    Font = "Segoe",
                    LineStyle = LineStyle.LongDash,
                    FontSize = 10,
                    Text = marker.Name + " (" + Math.Round(marker.Time / 1000, 2).ToString() + " s)",
                    TextColor = OxyColors.Black,
                    X = marker.Time / 1000
                });
            }

            this.plotView.Model.InvalidatePlot(true);
            ((IPlotModel)this.plotView.Model).Update(true);
        }
    }
}
