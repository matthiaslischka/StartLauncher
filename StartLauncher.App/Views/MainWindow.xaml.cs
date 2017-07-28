using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Forms;
using Application = System.Windows.Application;

namespace StartLauncher.App.Views
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            CommandsListView.Items.SortDescriptions.Add(new SortDescription
            {
                PropertyName = "Name",
                Direction = ListSortDirection.Ascending
            });
            CommandsListView.Columns[0].SortDirection = ListSortDirection.Ascending;

            InitializeTrayNotifyIcon();
        }

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
    }
}