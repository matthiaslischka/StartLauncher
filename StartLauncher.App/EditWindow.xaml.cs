using System.Windows;
using System.Windows.Input;

namespace StartLauncher.App
{
    public partial class EditWindow
    {
        public EditWindow(CommandDto commandDto)
        {
            InitializeComponent();
            DataContext = commandDto;
            Loaded += (sender, e) => MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            DataAccessor.Current.SaveCommand((CommandDto) DataContext);
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}