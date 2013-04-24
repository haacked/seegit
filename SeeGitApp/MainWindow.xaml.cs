using SeeGit.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Controls;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace SeeGit
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;
        private readonly Settings _configuration;
        private Dictionary<Control, Tuple<string, dynamic>> _defaultSettings;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel = new MainWindowViewModel(Dispatcher, path => new RepositoryGraphBuilder(path));

            _viewModel.MonitorRepository(Directory.GetCurrentDirectory());

            _defaultSettings = new Dictionary<Control, Tuple<string, dynamic>>
            {
            };

            try
            {
                _configuration = new Settings();
            }
            catch (ConfigurationErrorsException)
            {
            }

            LoadSettings();
        }

        /// <summary>
        /// Updates the UI elements states to stored values.
        /// </summary>
        private void LoadSettings()
        {
            foreach (var element in _defaultSettings)
            {
                try
                {
                    var val = _configuration.GetSetting(element.Value.Item1, element.Value.Item2);

                    if (element.Key is CheckBox) // special case for checkboxes, changing "Text" is not correct
                        ((CheckBox)element.Key).IsChecked = Convert.ToBoolean(val);
                    else // if (element.Key is ComboBox)
                        ((ComboBox)element.Key).Text = val.ToString();
                }
                catch
                {
                }
            }
        }

        /// <summary>
        /// Saves the UI elements states.
        /// </summary>
        private void SaveSettings()
        {
            foreach (var element in _defaultSettings)
            {
                string val;
                if (element.Key is CheckBox)
                    val = (bool)((CheckBox)element.Key).IsChecked ? "True" : "False";
                else // if (element.Key is ComboBox)
                    val = ((ComboBox)element.Key).Text;

                _configuration.SetSetting(element.Value.Item1, val);
            }

            _configuration.Save();
        }

        private void OnChooseRepository(object sender, RoutedEventArgs args)
        {
            _viewModel.MonitorRepository(WindowsExtensions.BrowseForFolder(Environment.GetFolderPath(Environment.SpecialFolder.Personal)));
        }

        private void OnRefresh(object sender, ExecutedRoutedEventArgs e)
        {
            _viewModel.MonitorRepository(_viewModel.RepositoryPath);
        }

        private void OnToggleSettings(object sender, RoutedEventArgs args)
        {
            _viewModel.ToggleSettings();
        }
    }
}