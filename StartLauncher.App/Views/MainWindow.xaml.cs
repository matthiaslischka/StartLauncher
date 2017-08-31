using System.ComponentModel;

namespace StartLauncher.App.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            CommandsListView.Items.SortDescriptions.Add(new SortDescription
            {
                PropertyName = "Name",
                Direction = ListSortDirection.Ascending
            });
        }
    }
}