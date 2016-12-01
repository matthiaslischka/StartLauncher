using System.Windows;
using StartLauncher.App.DataAccess;

namespace StartLauncher.App.UI
{
    public partial class SettingsWindow
    {
        private readonly ICommandsDataAccessor _commandsDataAccessor;

        public SettingsWindow(ICommandsDataAccessor commandsDataAccessor)
        {
            _commandsDataAccessor = commandsDataAccessor;
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