using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ControlzEx.Theming;
using StartLauncher.App.Models;

namespace StartLauncher.App.Views
{
    public partial class MainWindow
    {
        private const string SunnyGlyph = "";
        private const string MoonGlyph = "";

        public MainWindow()
        {
            InitializeComponent();

            CommandsListView.Items.SortDescriptions.Add(new SortDescription
            {
                PropertyName = "Name",
                Direction = ListSortDirection.Ascending
            });

            UpdateThemeToggleIcon();
        }

        private void SearchBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var searchText = SearchBox.Text?.Trim();

            if (string.IsNullOrEmpty(searchText))
            {
                CommandsListView.Items.Filter = null;
                return;
            }

            CommandsListView.Items.Filter = item =>
                item is CommandDto command &&
                ((command.Name?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                 (command.Description?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                 (command.Command?.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0));
        }

        private void ThemeToggleButton_OnClick(object sender, RoutedEventArgs e)
        {
            var currentTheme = ThemeManager.Current.DetectTheme(Application.Current);
            var newBaseColor = currentTheme?.BaseColorScheme == "Dark" ? "Light" : "Dark";
            ThemeManager.Current.ChangeThemeBaseColor(Application.Current, newBaseColor);
            UpdateThemeToggleIcon();
        }

        private void UpdateThemeToggleIcon()
        {
            var currentTheme = ThemeManager.Current.DetectTheme(Application.Current);
            var isDark = currentTheme?.BaseColorScheme == "Dark";
            ThemeToggleIcon.Text = isDark ? SunnyGlyph : MoonGlyph;
        }
    }
}
