using System.Windows;

namespace StartLauncher.App
{
    public partial class EditWindow
    {
        public EditWindow(CommandDto commandDto)
        {
            InitializeComponent();
            DataContext = commandDto;
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            DataAccessor.GetInstance().SaveCommand((CommandDto) DataContext);
        }
    }
}