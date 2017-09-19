// The MIT License(MIT)
//
// Copyright(c) 2017 Microsoft Corporation. All Rights Reserved.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and 
// associated documentation files (the "Software"), to deal in the Software without restriction, 
// including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, 
// and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE 
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR 
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Multilingual.Utilities.Providers.Editor.Model;

namespace Multilingual.Utilities.Providers.Editor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ProviderInfo> Providers { get; set; }

        private string _providerConfigFile;

        /// <summary>
        /// Constructor
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Providers = new ObservableCollection<ProviderInfo>();
            ProviderBinder.ItemsSource = Providers;
        }

        /// <summary>
        /// Called after the Windows is loaded to initialize any required states
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var programDataPath = Environment.GetEnvironmentVariable("ProgramData");
            if (programDataPath == null)
            {
                MessageBox.Show(
                    "Environment variable 'ProgramData' is required to locate the provider configurations file, but it is currently null.",
                    "Configuration error");
                Close();
                return;
            }

            _providerConfigFile = System.IO.Path.Combine(programDataPath, "Multilingual App Toolkit", "TranslationManager.xml");
            PopulateProviderListView(_providerConfigFile);
        }

        /// <summary>
        /// Load the list of active providers
        /// </summary>
        /// <param name="configFile">Provider configuration file</param>
        private void PopulateProviderListView(string configFile)
        {
            Providers.Clear();

            var providers = ProvidersConfig.Load(configFile);
            foreach (var provider in providers)
            {
                Providers.Add(provider);
            }
        }

        /// <summary>
        /// Save the modified list of providers
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            ProvidersConfig.Save(_providerConfigFile, Providers.ToList());
            MessageBox.Show("Configuration updated", "Saved");
            Close();
        }

        /// <summary>
        /// Restore the list of providers based on the project's RestoreDefaultProviders.xml 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RestoreButton_OnClick(object sender, RoutedEventArgs e)
        {
            PopulateProviderListView(@".\RestoreDefaultProviders.xml");
        }

        /// <summary>
        /// Cancels changes and exits the app
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Removes the selected provider from the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (ProviderBinder.SelectedValue is ProviderInfo item)
            {
                Providers.Remove(item);
            }
        }

        /// <summary>
        /// Moves the selected provider up in the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveUpMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (ProviderBinder.SelectedValue is ProviderInfo item)
            {
                var idx = Providers.IndexOf(item);
                if (idx > 0)
                {
                    Providers.Move(idx, idx - 1);
                }
            }
        }

        /// <summary>
        /// Moves the selected provider down in the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveDownMenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            if (ProviderBinder.SelectedValue is ProviderInfo item)
            {
                var idx = Providers.IndexOf(item);
                if (idx < Providers.Count - 1)
                {
                    Providers.Move(idx, idx + 1);
                }
            }
        }

        /// <summary>
        /// Manages the visual state of the Provider context menus
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContextMenu_OnOpened(object sender, RoutedEventArgs e)
        {
            if (ProviderBinder == null || ProviderBinder.Items.Count == 0)
            {
                SetMenuState(MenuMoveUp, false);
                SetMenuState(MenuMoveDown, false);
                SetMenuState(MenuDelete, false);

                MenuDelete.IsEnabled = false;
                MenuDelete.IsEnabled = false;
            }
            else
            {
                SetMenuState(MenuMoveUp, ProviderBinder.SelectedIndex > 0);
                SetMenuState(MenuMoveDown, ProviderBinder.SelectedIndex < ProviderBinder.Items.Count - 1);
                SetMenuState(MenuDelete, ProviderBinder.SelectedIndex >= 0);
            }
        }

        /// <summary>
        /// Sets the visual state of the Provider context menus
        /// </summary>
        /// <param name="menuItem">Context menu item</param>
        /// <param name="isEnabled">Enabled state to set</param>
        private static void SetMenuState(MenuItem menuItem, bool isEnabled)
        {
            menuItem.IsEnabled = isEnabled;
            if (menuItem.Icon is Image image)
            {
                image.Opacity = menuItem.IsEnabled ? 1 : 0.5;
            }
        }
    }
}
