using System;
using System.IO;
using System.Windows;

namespace SeeGit
{
    public partial class MainWindow : Window
    {
        private FileSystemWatcher _watcher;
        private MainWindowViewModel _viewModel;
        private string _gitRepositoryPath;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnChooseRepository(object sender, RoutedEventArgs args)
        {
            DisposeWatcher();

            _gitRepositoryPath = WindowsExtensions.BrowseForFolder(@"c:\dev\git");

            SetupGraphBuilder();

            string gitDirectory = Path.Combine(_gitRepositoryPath, ".git");
            if (Directory.Exists(gitDirectory))
            {
                SetupGitRepositoryWatcher(gitDirectory);
            }
            else
            {
                _watcher = new FileSystemWatcher(_gitRepositoryPath)
                           {
                               IncludeSubdirectories = true,
                               EnableRaisingEvents = true,
                               NotifyFilter =
                                   NotifyFilters.CreationTime | NotifyFilters.DirectoryName 
                                   
                           };

                _watcher.Changed += (o, e) =>
                                    {
                                        if (!Directory.Exists(gitDirectory)) return;
                                        DisposeWatcher();
                                        Dispatcher.Invoke(new Action(SetupGraphBuilder));
                                        SetupGitRepositoryWatcher(gitDirectory);
                                    };
            }
        }

        private void SetupGitRepositoryWatcher(string gitDirectory)
        {
            _watcher = new FileSystemWatcher(gitDirectory)
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
        }

        private void SetupGraphBuilder()
        {
            var graphBuilder = new RepositoryGraphBuilder(_gitRepositoryPath);
            DataContext = _viewModel = new MainWindowViewModel(graphBuilder, _gitRepositoryPath);
        }

        private void DisposeWatcher()
        {
            var oldWatcher = _watcher;
            if (oldWatcher != null)
            {
                oldWatcher.Dispose();
            }
        }
    }
}