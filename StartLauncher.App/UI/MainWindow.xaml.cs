using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using StartLauncher.App.Core;
using StartLauncher.App.DataAccess;
using Application = System.Windows.Application;

namespace StartLauncher.App.UI
{
    public partial class MainWindow
    {
        private readonly CommandsDataAccessor _commandsDataAccessor = CommandsDataAccessor.Current;

        private MainWindow()
        {
            InitializeComponent();
            CommandsListView.ItemsSource = _commandsDataAccessor.Commands;
            InitializeTrayNotifyIcon();
        }

        public static MainWindow Current { get; } = new MainWindow();

        private void InitializeTrayNotifyIcon()
        {
            var trayMenu = new ContextMenu();
            trayMenu.MenuItems.Add("Settings...", OnOpen);
            trayMenu.MenuItems.Add("Exit", OnExit);

            var notifyIcon = new NotifyIcon
            {
                Text = @"Start Launcher",
                Icon = Properties.Resources.Bokehlicia_Captiva_Rocket,
                ContextMenu = trayMenu,
                Visible = true
            };

            notifyIcon.DoubleClick += OnOpen;
        }

        private void OnOpen(object sender, EventArgs e)
        {
            Show();
            WindowState = WindowState.Normal;
            ShowInTaskbar = true;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            ShowInTaskbar = false;
            Visibility = Visibility.Hidden;
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

        private void OpenEditWindow(CommandDto commandDto)
        {
            var editWindow = new EditWindow(commandDto);
            editWindow.ShowDialog();
        }

        private void CommandsListView_MouseDoubleClick(object sender, EventArgs e)
        {
            EditButton_Click(sender, e);
        }

        private void RemoveButton_Click(object sender, EventArgs e)
        {
            var selectedCommandDto = CommandsListView.SelectedItem as CommandDto;
            if (selectedCommandDto == null)
                return;
            _commandsDataAccessor.DeleteCommand(selectedCommandDto);
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
        }
    }
}