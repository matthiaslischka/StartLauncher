using System.Windows;
using StartLauncher.App.DataAccess;

namespace StartLauncher.App.UI
{
    public partial class SettingsWindow
    {
        public SettingsWindow()
        {
            InitializeComponent();

            CommandsJsonFilePathTextBox.Text = CommandsDataAccessor.Current.GetCommandsFile().FullName;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            CommandsDataAccessor.Current.ChangeCommandsJsonFilePath(CommandsJsonFilePathTextBox.Text);
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}