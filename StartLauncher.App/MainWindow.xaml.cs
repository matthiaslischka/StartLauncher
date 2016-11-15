﻿using System;
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
    }
}