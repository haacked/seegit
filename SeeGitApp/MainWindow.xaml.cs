using System;
using System.IO;
using System.Windows;
using System.Windows.Input;

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

        private void OnChooseRepository(object sender, RoutedEventArgs args)
        {
            _viewModel.MonitorRepository(WindowsExtensions.BrowseForFolder(Environment.GetFolderPath(Environment.SpecialFolder.Personal)));
        }

        private void OnRefresh(object sender, ExecutedRoutedEventArgs e)
        {
            _viewModel.MonitorRepository(_viewModel.RepositoryPath);
        }
    }
}