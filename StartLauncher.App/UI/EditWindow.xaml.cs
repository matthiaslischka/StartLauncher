using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using StartLauncher.App.Core;
using StartLauncher.App.DataAccess;

namespace StartLauncher.App.UI
{
    public partial class EditWindow
    {
        private readonly ICommandsDataAccessor _commandsDataAccessor;

        public EditWindow(CommandDto commandDto, ICommandsDataAccessor commandsDataAccessor)
        {
            _commandsDataAccessor = commandsDataAccessor;
            InitializeComponent();
            DataContext = commandDto;
            Loaded += (sender, e) => MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            NameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            CommandTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            DescriptionTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            RunAsAdminCheckBox.GetBindingExpression(ToggleButton.IsCheckedProperty).UpdateSource();
            
            _commandsDataAccessor.SaveCommand((CommandDto) DataContext);
            DialogResult = true;
            Close();
        }

        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                WindowStyle = ProcessWindowStyle.Hidden,
                FileName = "cmd.exe",
                Arguments = "/C start \"\" /B " + CommandTextBox.Text,
                UseShellExecute = false
            };
            process.StartInfo = startInfo;
            process.Start();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}