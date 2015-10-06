using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using SeeGit.Models;

namespace SeeGit
{
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _viewModel = new MainWindowViewModel(Dispatcher, path => new RepositoryGraphBuilder(path));

            _viewModel.MonitorRepository(Directory.GetCurrentDirectory());
        }

        /// <summary>
        /// A reference to the configuration.
        /// </summary>
        public static Settings Configuration { get; } = new Settings();

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
                foreach (var vertex in _viewModel.Graph.Vertices)
                {
                    vertex.AdornerMessageVisibilityType = Configuration.GetSetting("AdornerCommitMessageVisibility", "ExpandedHidden");
                    vertex.DescriptionShown = Configuration.GetSetting("DescriptionInExpander", false);
                    vertex.ShaLength = Configuration.GetSetting("SHALength", 8);
                }
                Configuration.Save();
                _viewModel.Refresh();
            }
        }
    }
}