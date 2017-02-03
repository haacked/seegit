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
                    vertex.AuthorShown = Configuration.GetSetting("AuthorInHeader", false);
                    vertex.DescriptionShown = Configuration.GetSetting("DescriptionInExpander", false);
                    vertex.ShaLength = Configuration.GetSetting("SHALength", 8);
                }
                Configuration.Save();
                _viewModel.Refresh();
                // Ideally we wouldn't have to call relayout.  Refreshing the view model would
                // update the Graph which would then cause the RepositoryGraphLayout control to adjust its layout.
                // Unfortunately, I am not as familiar with the RepositoryGraphLayout control and when the vertices have their
                // width's increased due to displaying the author, the graph needs to have its layout adjusted.
                graphLayout.Relayout();
            }
        }
    }
}