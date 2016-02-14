using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace ETWCorrelationViewer
{
    public class DiagramViewModel
    {
        private Dictionary<int, ProjectDataViewModel> projectViewModels;

        public DiagramViewModel()
        {
            this.Model = IntervalBarSeries();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        public DiagramViewModel(ProjectDataCollection projects)
        {
            this.ColorMapping = new Dictionary<int, OxyColor>();
            this.projectViewModels = new Dictionary<int, ProjectDataViewModel>();

            this.Projects = projects;
            this.Projects.CollectionChanged += Projects_CollectionChanged;
            this.Model = DiagramBase();
        }

        private void Projects_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                int i = e.NewStartingIndex;
                foreach (ProjectData project in e.NewItems)
                {
                    //AddProject(project, i);
                    projectViewModels.Add(project.ID, new ProjectDataViewModel(this, project, i));
                    i++;
                }
                this.Model.InvalidatePlot(true);
            }
        }

        private void AddProject(ProjectData project, int index)
        {
            CategoryAxis categoryAxis1 = this.Model.Axes[0] as CategoryAxis;
            categoryAxis1.Labels.Add("Project " + project.ID);

            var lineSeries1 = new LineSeries();
            lineSeries1.Color = OxyColor.FromArgb(255, 0, 120, 0);
            lineSeries1.Points.Add(new DataPoint(Axis.ToDouble(project.StartPoint), index));
            lineSeries1.Points.Add(new DataPoint(Axis.ToDouble(project.EndPoint), index));
            this.Model.Series.Add(lineSeries1);

            //var intervalBarSeries1 = new IntervalBarSeries();
            //CreateIntervallItems(project.Data, index, intervalBarSeries1);

            //this.Model.Series.Add(intervalBarSeries1);
        }

        //private void CreateIntervallItems(DataItemCollection dataItems, int index, IntervalBarSeries intervalBarSeries)
        //{
        //    foreach (var item in dataItems)
        //    {
        //        OxyColor color;
        //        if (!this.ColorMapping.TryGetValue(item.ItemType, out color))
        //        {
        //            color = OxyColor.FromArgb(100, 0, 120, 0);
        //        }
        //        intervalBarSeries.Items.Add(new IntervalBarItem(Axis.ToDouble(item.StartPoint), Axis.ToDouble(item.EndPoint), "Hallo") { Color = color, CategoryIndex = index });

        //        this.CreateIntervallItems(item.Children, index, intervalBarSeries);
        //    }
        //}

        public static PlotModel DiagramBase()
        {
            var plotModel1 = new PlotModel();
            plotModel1.LegendPlacement = LegendPlacement.Outside;
            plotModel1.Title = "Project Events";
            var categoryAxis1 = new CategoryAxis();
            categoryAxis1.MinorStep = 0.5;
            categoryAxis1.Position = AxisPosition.Left;

            plotModel1.Axes.Add(categoryAxis1);
            var linearAxis1 = new DateTimeAxis();
            linearAxis1.MaximumPadding = 0.1;
            linearAxis1.MinimumPadding = 0.1;
            linearAxis1.Position = AxisPosition.Bottom;
            linearAxis1.MajorGridlineStyle = LineStyle.Solid;
            linearAxis1.MinorGridlineStyle = LineStyle.Dot;
            plotModel1.Axes.Add(linearAxis1);

            return plotModel1;
        }

        public static PlotModel IntervalBarSeries()
        {
            var plotModel1 = new PlotModel();
            plotModel1.LegendPlacement = LegendPlacement.Outside;
            plotModel1.Title = "IntervalBarSeries";
            var categoryAxis1 = new CategoryAxis();
            categoryAxis1.MinorStep = 1;
            categoryAxis1.Position = AxisPosition.Left;

            categoryAxis1.Labels.Add("Activity A");
            categoryAxis1.Labels.Add("Activity B");
            categoryAxis1.Labels.Add("Activity C");
            categoryAxis1.Labels.Add("Activity D");

            //categoryAxis1.ActualLabels.Add("Activity A");
            //categoryAxis1.ActualLabels.Add("Activity B");
            //categoryAxis1.ActualLabels.Add("Activity C");
            //categoryAxis1.ActualLabels.Add("Activity D");
            plotModel1.Axes.Add(categoryAxis1);
            var linearAxis1 = new LinearAxis();
            linearAxis1.MaximumPadding = 0.1;
            linearAxis1.MinimumPadding = 0.1;
            linearAxis1.Position = AxisPosition.Bottom;
            plotModel1.Axes.Add(linearAxis1);

            AddProjectLifeTime(0, 6, 11, plotModel1);
            AddProjectLifeTime(1, 5, 10, plotModel1);
            AddProjectLifeTime(2, 8, 15, plotModel1);
            AddProjectLifeTime(3, 4, 20, plotModel1);

            //var intervalBarSeries1 = new IntervalBarSeries();
            //intervalBarSeries1.Title = "IntervalBarSeries 1";
            //intervalBarSeries1.Items.Add(new IntervalBarItem(6, 7));
            //intervalBarSeries1.Items.Add(new IntervalBarItem(4, 8));
            //intervalBarSeries1.Items.Add(new IntervalBarItem(5, 11));
            //intervalBarSeries1.Items.Add(new IntervalBarItem(4, 12));
            //plotModel1.Series.Add(intervalBarSeries1);
            //var intervalBarSeries2 = new IntervalBarSeries();
            //intervalBarSeries2.Title = "IntervalBarSeries 2";
            //intervalBarSeries2.Items.Add(new IntervalBarItem(8, 9));

            ////intervalBarSeries2.Items.Add(new IntervalBarItem(8, 10));
            ////intervalBarSeries2.Items.Add(new IntervalBarItem(11, 12));
            ////intervalBarSeries2.Items.Add(new IntervalBarItem(12, 12.5));
            //plotModel1.Series.Add(intervalBarSeries2);
            var intervalBarSeries3 = new IntervalBarSeries();
            intervalBarSeries3.Items.Add(new IntervalBarItem(9, 10) { Color = OxyColor.FromArgb(99, 120, 120, 120), CategoryIndex = 1 });
            intervalBarSeries3.Items.Add(new IntervalBarItem(9.5, 9.8) { Color = OxyColor.FromArgb(99, 120, 120, 120), CategoryIndex = 1 });
            plotModel1.Series.Add(intervalBarSeries3);

            var intervalBarSeries2 = new IntervalBarSeries();
            intervalBarSeries2.Items.Add(new IntervalBarItem(9, 10) { Color = OxyColor.FromArgb(99, 120, 120, 120), CategoryIndex = 3 });
            intervalBarSeries2.Items.Add(new IntervalBarItem(9.5, 9.8) { Color = OxyColor.FromArgb(99, 120, 120, 120), CategoryIndex = 3 });
            plotModel1.Series.Add(intervalBarSeries2);

            //intervalBarSeries2.Items.Add(new IntervalBarItem(11, 12));
            //intervalBarSeries2.Items.Add(new IntervalBarItem(12, 12.5));

            return plotModel1;
        }

        private static void AddProjectLifeTime(int project, int start, int end, PlotModel model)
        {
            var lineSeries1 = new LineSeries();
            lineSeries1.Color = OxyColor.FromArgb(255, 0, 120, 0);

            //lineSeries1.Title = "LineSeries 1";
            lineSeries1.Points.Add(new DataPoint(start, project));
            lineSeries1.Points.Add(new DataPoint(end, project));
            model.Series.Add(lineSeries1);
        }

        /// <summary>
        /// Gets the plot model.
        /// </summary>
        public PlotModel Model { get; private set; }

        public ProjectDataCollection Projects { get; private set; }

        public Dictionary<int, OxyColor> ColorMapping { get; private set; }
    }

}