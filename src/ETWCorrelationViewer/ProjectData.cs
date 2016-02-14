using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;

namespace ETWCorrelationViewer
{
    public class ProjectData : Bindable
    {
        public DataItemCollection Data { get; set; }

        public DateTime? StartPoint
        {
            get { return Get<DateTime?>(); }
            set { Set(value); }
        }

        public DateTime? EndPoint
        {
            get { return Get<DateTime?>(); }
            set { Set(value); }
        }

        public int ID
        {
            get { return Get<int>(); }
            set { Set(value); }
        }

        public ProjectData(int id)
        {
            this.Data = new DataItemCollection();
            ((INotifyPropertyChanged)this.Data).PropertyChanged += ProjectData_PropertyChanged;
            this.ID = id;
        }

        private void ProjectData_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(StartPoint))
            {
                this.StartPoint = this.Data.StartPoint;
            }
            else if (e.PropertyName == nameof(EndPoint))
            {
                this.EndPoint = this.Data.EndPoint;
            }
        }
    }
}