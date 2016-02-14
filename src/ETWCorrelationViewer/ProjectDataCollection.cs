using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ETWCorrelationViewer
{
    public class ProjectDataCollection : ObservableCollection<ProjectData>
    {
        private DateTime? startPoint;

        public DateTime? StartPoint
        {
            get
            {
                return startPoint;
            }
            set
            {
                if (startPoint != value)
                {
                    startPoint = value;
                    this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(StartPoint)));
                }
            }
        }

        private DateTime? endPoint;

        public DateTime? EndPoint
        {
            get
            {
                return endPoint;
            }
            set
            {
                if (endPoint != value)
                {
                    endPoint = value;
                    this.OnPropertyChanged(new PropertyChangedEventArgs(nameof(EndPoint)));
                }
            }
        }

        protected override void InsertItem(int index, ProjectData item)
        {
            item.PropertyChanged += Item_PropertyChanged;

            base.InsertItem(index, item);
            this.ValidateTimeSpan(item);
        }

        private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ProjectData item = sender as ProjectData;
            this.ValidateTimeSpan(item);
        }

        private void ValidateTimeSpan(ProjectData item)
        {
            if (!this.StartPoint.HasValue || this.StartPoint > item.StartPoint)
            {
                this.StartPoint = item.StartPoint;
            }

            if (!this.EndPoint.HasValue || this.EndPoint < item.EndPoint)
            {
                this.EndPoint = item.EndPoint;
            }
        }
    }
}