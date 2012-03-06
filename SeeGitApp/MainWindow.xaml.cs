using System;
using System.IO;
using System.Reactive.Linq;
using System.Windows;

namespace SeeGit
{
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _viewModel;
        private string _gitRepositoryPath;
        private IObservable<object> _fileSystemObservable;

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
                _fileSystemObservable = new FileSystemWatcher(_gitRepositoryPath)
                                        {
                                            IncludeSubdirectories = true,
                                            EnableRaisingEvents = true,
                                            NotifyFilter =
                                                NotifyFilters.CreationTime | NotifyFilters.DirectoryName
                                        }.ObserveFileSystemEvents().Throttle(TimeSpan.FromSeconds(1));

                _fileSystemObservable.Subscribe(_ =>
                                                {
                                                    if (!Directory.Exists(gitDirectory)) return;
                                                    DisposeWatcher();
                                                    Dispatcher.Invoke(new Action(SetupGraphBuilder));
                                                    SetupGitRepositoryWatcher(gitDirectory);
                                                });
            }
        }

        private void SetupGitRepositoryWatcher(string gitDirectory)
        {
            _fileSystemObservable = new FileSystemWatcher(gitDirectory)
                                    {
                                        IncludeSubdirectories = true,
                                        EnableRaisingEvents = true,
                                        NotifyFilter =
                                            NotifyFilters.CreationTime | NotifyFilters.DirectoryName |
                                            NotifyFilters.FileName |
                                            NotifyFilters.LastWrite
                                    }.ObserveFileSystemEvents().Throttle(TimeSpan.FromSeconds(1));

            _fileSystemObservable.Subscribe(_ =>
                                            {
                                                var vm = _viewModel;
                                                Dispatcher.Invoke(new Action(vm.Refresh), null);
                                            });
        }

        private void SetupGraphBuilder()
        {
            var graphBuilder = new RepositoryGraphBuilder(_gitRepositoryPath);
            DataContext = _viewModel = new MainWindowViewModel(graphBuilder, _gitRepositoryPath);
        }

        private void DisposeWatcher()
        {
            _fileSystemObservable = null;
        }
    }
}