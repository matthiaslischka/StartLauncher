using System;
using System.ComponentModel;
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
            Refresh();

            var trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Settings...", OnOpen);
            trayMenu.MenuItems.Add("Exit", OnExit);

            var notifyIcon = new NotifyIcon
            {
                Text = @"Start Launcher",
                Icon = new Icon(SystemIcons.WinLogo, 40, 40),
                ContextMenu = trayMenu,
                Visible = true
            };

            notifyIcon.DoubleClick += OnOpen;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            ShowInTaskbar = false;
            Visibility = Visibility.Hidden;
        }

        private void OnOpen(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
            ShowInTaskbar = true;
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

        private void AddButton_Click(object sender, EventArgs e)
        {
            OpenEditWindow(new CommandDto());
        }

        private void EditButton_Click(object sender, EventArgs e)
        {
            var selectedCommandDto = CommandsListView.SelectedItem as CommandDto;
            if (selectedCommandDto == null)
                return;

            OpenEditWindow(selectedCommandDto);
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            var selectedCommandDto = CommandsListView.SelectedItem as CommandDto;
            if (selectedCommandDto == null)
                return;

            DataAccessor.GetInstance().DeleteCommand(selectedCommandDto);
        }

        private void CommandsListView_MouseDoubleClick(object sender, EventArgs e)
        {
            EditButton_Click(sender, e);
        }

        private void OpenEditWindow(CommandDto commandDto)
        {
            var editWindow = new EditWindow(commandDto);
            var showDialogResult = editWindow.ShowDialog();
            if (showDialogResult == true)
                Refresh();
        }

        public void Refresh()
        {
            CommandsListView.ItemsSource = DataAccessor.GetInstance().GetCommands();
            CommandsListView.Items.Refresh();
        }
    }
}