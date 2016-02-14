using System;
using System.Windows;
using OxyPlot;

namespace ETWCorrelationViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.Projects = new ProjectDataCollection();

            this.ViewModel = new DiagramViewModel(this.Projects);

            InitializeComponent();

            this.PlotView1.DataContext = this.ViewModel;

            this.ViewModel.ColorMapping.Add(1, OxyColor.FromArgb(100, 120, 0, 0));
            this.ViewModel.ColorMapping.Add(2, OxyColor.FromArgb(100, 0, 120, 0));
            this.ViewModel.ColorMapping.Add(3, OxyColor.FromArgb(100, 120, 0, 120));
            this.ViewModel.ColorMapping.Add(4, OxyColor.FromArgb(100, 0, 120, 120));
        }

        private int projectId = 0;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProjectData project = new ProjectData(projectId++);
            DateTime now = DateTime.Now;
            project.Data.Add(new DataItem(1, now, now.AddMinutes(5)));
            project.Data.Add(DataItem.Create(2, now.AddMinutes(30), now.AddMinutes(55), DataItem.Create(3, now.AddMinutes(40), now.AddMinutes(45))));
            this.Projects.Add(project);
        }

        private void Modify_Click(object sender, RoutedEventArgs e)
        {
            ProjectData project = this.Projects[projectId - 1];
            project.Data.Add(DataItem.Create(1, project.EndPoint.Value.AddMinutes(20), null));
        }

        public DiagramViewModel ViewModel { get; private set; }
        public ProjectDataCollection Projects { get; private set; }
    }
}