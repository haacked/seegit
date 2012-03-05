using System;
using System.IO;
using System.Windows;

namespace SeeGit
{
    public partial class MainWindow : Window
    {
        private readonly FileSystemWatcher _watcher;
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            const string gitRepositoryPath = @"C:\Users\Administrator\Desktop\SeeGit";

            var graphBuilder = new RepositoryGraphBuilder(gitRepositoryPath);
            DataContext = _viewModel = new MainWindowViewModel(graphBuilder);

            _watcher = new FileSystemWatcher(Path.Combine(gitRepositoryPath, ".git"))
                       {
                           IncludeSubdirectories = true,
                           EnableRaisingEvents = true,
                           NotifyFilter =
                               NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName |
                               NotifyFilters.LastWrite
                       };
            _watcher.Changed += (o, e) =>
                                {
                                    var vm = _viewModel;
                                    Dispatcher.Invoke(new Action(vm.Refresh), null);
                                };

            InitializeComponent();
        }
    }
}