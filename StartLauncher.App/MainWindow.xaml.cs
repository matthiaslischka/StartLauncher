using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace StartLauncher.App
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            commandsListView.ItemsSource = DataAccessor.GetInstance().GetCommands();

            var trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Exit", OnExit);

            var notifyIcon = new NotifyIcon
            {
                Text = @"Start Launcher",
                Icon = new Icon(SystemIcons.WinLogo, 40, 40),
                ContextMenu = trayMenu,
                Visible = true
            };

            notifyIcon.DoubleClick +=
                delegate
                {
                    Show();
                    WindowState = WindowState.Normal;
                };
        }

        private void OnExit(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
                Hide();

            base.OnStateChanged(e);
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            OpenEditWindow(new CommandDto());
        }

        private void editButton_Click(object sender, EventArgs e)
        {
            var selectedCommandDto = commandsListView.SelectedItem as CommandDto;
            if (selectedCommandDto == null)
                return;

            OpenEditWindow(selectedCommandDto);
        }

        private void removeButton_Click(object sender, EventArgs e)
        {
            var selectedCommandDto = commandsListView.SelectedItem as CommandDto;
            if (selectedCommandDto == null)
                return;

            DataAccessor.GetInstance().DeleteCommand(selectedCommandDto);
        }

        private void commandsListView_MouseDoubleClick(object sender, EventArgs e)
        {
            editButton_Click(sender, e);
        }

        private static void OpenEditWindow(CommandDto commandDto)
        {
            var editWindow = new EditWindow(commandDto);
            editWindow.Show();
        }
    }
}