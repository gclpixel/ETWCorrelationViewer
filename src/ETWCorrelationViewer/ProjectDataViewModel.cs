using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace ETWCorrelationViewer
{
    public class ProjectDataViewModel
    {
        public ProjectData Project { get; private set; }
        public DiagramViewModel ParentViewModel { get; private set; }
        public int Index { get; private set; }

        private LineSeries lifeTimeLine;
        private IntervalBarSeries intervallBarSeries;
        private List<DataItemViewModel> dataViewModels;

        public ProjectDataViewModel(DiagramViewModel parentViewModel, ProjectData project, int index)
        {
            this.dataViewModels = new List<DataItemViewModel>();

            this.Index = index;
            this.Project = project;
            this.ParentViewModel = parentViewModel;

            this.Project.PropertyChanged += Project_PropertyChanged;
            this.Project.Data.CollectionChanged += Data_CollectionChanged;
            this.CreateView();
        }

        private void Data_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                dataViewModels.AddRange(DataItemViewModel.CreateIntervallItems(this.ParentViewModel, e.NewItems.Cast<DataItem>(), this.Index, this.intervallBarSeries));
            }
            ParentViewModel.Model.InvalidatePlot(true);
        }

        private void Project_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.lifeTimeLine.Points[0] = new DataPoint(Axis.ToDouble(this.Project.StartPoint), this.Index);
            this.lifeTimeLine.Points[1] = new DataPoint(Axis.ToDouble(this.Project.EndPoint), this.Index);
            ParentViewModel.Model.InvalidatePlot(true);
        }

        private void CreateView()
        {
            CategoryAxis categoryAxis1 = this.ParentViewModel.Model.Axes[0] as CategoryAxis;
            categoryAxis1.Labels.Add("Project " + this.Project.ID);

            this.lifeTimeLine = new LineSeries();
            this.lifeTimeLine.Color = OxyColor.FromArgb(255, 0, 120, 0);
            this.lifeTimeLine.Points.Add(new DataPoint(Axis.ToDouble(this.Project.StartPoint), this.Index));
            this.lifeTimeLine.Points.Add(new DataPoint(Axis.ToDouble(this.Project.EndPoint), this.Index));
            this.ParentViewModel.Model.Series.Add(this.lifeTimeLine);

            this.intervallBarSeries = new IntervalBarSeries();
            dataViewModels.AddRange(DataItemViewModel.CreateIntervallItems(this.ParentViewModel, this.Project.Data, this.Index, this.intervallBarSeries));
            this.ParentViewModel.Model.Series.Add(this.intervallBarSeries);
        }
    }
}