using System.Collections.Generic;
using System.Linq;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace ETWCorrelationViewer
{
    public class DataItemViewModel
    {
        public DataItem Data { get; private set; }
        public DiagramViewModel ParentViewModel { get; private set; }
        public int Index { get; private set; }

        private IntervalBarSeries intervallBarSeries;
        private List<DataItemViewModel> dataViewModels;
        private IntervalBarItem intervallBarItem;

        public DataItemViewModel(DiagramViewModel parentViewModel, DataItem data, int index, IntervalBarSeries intervallBarSeries)
        {
            this.dataViewModels = new List<DataItemViewModel>();
            this.ParentViewModel = parentViewModel;
            this.Data = data;
            this.Index = index;
            this.intervallBarSeries = intervallBarSeries;
            this.Data.PropertyChanged += this.Data_PropertyChanged;
            this.Data.Children.CollectionChanged += this.Children_CollectionChanged;
            this.CreateView();
        }

        private void Data_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            this.intervallBarItem.Start = Axis.ToDouble(this.Data.StartPoint);
            this.intervallBarItem.End = Axis.ToDouble(this.Data.EndPoint ?? this.Data.StartPoint);
            ParentViewModel.Model.InvalidatePlot(true);
        }

        private void Children_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
            {
                dataViewModels.AddRange(DataItemViewModel.CreateIntervallItems(this.ParentViewModel, e.NewItems.Cast<DataItem>(), this.Index, this.intervallBarSeries));
            }
            ParentViewModel.Model.InvalidatePlot(true);
        }

        private void CreateView()
        {
            OxyColor color;
            if (!this.ParentViewModel.ColorMapping.TryGetValue(this.Data.ItemType, out color))
            {
                color = OxyColor.FromArgb(100, 0, 120, 0);
            }
            this.intervallBarItem = new IntervalBarItem(Axis.ToDouble(this.Data.StartPoint), Axis.ToDouble(this.Data.EndPoint?? this.Data.StartPoint)) { Color = color, CategoryIndex = this.Index };
            this.intervallBarSeries.Items.Add(this.intervallBarItem);

            dataViewModels.AddRange(DataItemViewModel.CreateIntervallItems(this.ParentViewModel, this.Data.Children, this.Index, this.intervallBarSeries));
        }

        public static List<DataItemViewModel> CreateIntervallItems(DiagramViewModel parentViewModel, IEnumerable<DataItem> dataItems, int index, IntervalBarSeries intervalBarSeries)
        {
            List<DataItemViewModel> viewModels = new List<DataItemViewModel>();
            foreach (var item in dataItems)
            {
                viewModels.Add(new DataItemViewModel(parentViewModel, item, index, intervalBarSeries));
            }

            return viewModels;
        }
    }
}