using System.Windows;
using System.Windows.Controls;
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
            NameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            CommandTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            DescriptionTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();

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