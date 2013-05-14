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
        private static readonly Settings _configuration = new Settings();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel = new MainWindowViewModel(Dispatcher, path => new RepositoryGraphBuilder(path));

            _viewModel.MonitorRepository(Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// A reference to the configuration.
        /// </summary>
        public static Settings Configuration
        {
            get { return _configuration; }
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
            if (!_viewModel.ToggleSettings())
            {
                foreach (CommitVertex vertex in _viewModel.Graph.Vertices)
                {
                    vertex.AdornerMessageVisibilityType = _configuration.GetSetting("AdornerCommitMessageVisibility", "ExpandedHidden");
                    vertex.DescriptionShown = _configuration.GetSetting<bool>("DescriptionInExpander", false);
                    vertex.ShaLength = _configuration.GetSetting<int>("SHALength", 8);
                }
                _configuration.Save();
                _viewModel.Refresh();
            }
        }
    }
}