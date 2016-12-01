using System.Windows;
using StartLauncher.App.DataAccess;

namespace StartLauncher.App.UI
{
    public partial class SettingsWindow
    {
        private readonly CommandsDataAccessor _commandsDataAccessor = CommandsDataAccessor.Current;

        public SettingsWindow()
        {
            InitializeComponent();

            CommandsJsonFilePathTextBox.Text = _commandsDataAccessor.GetCommandsFile().FullName;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            _commandsDataAccessor.ChangeCommandsJsonFilePath(CommandsJsonFilePathTextBox.Text);
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}