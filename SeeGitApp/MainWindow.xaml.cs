using System;
using System.IO;
using System.Windows;
using SeeGit.Models;

namespace SeeGit
{
    public partial class MainWindow : Window
    {
        private readonly FileSystemWatcher watcher;
        private readonly MainWindowViewModel _viewModel;

        public MainWindow()
        {
            const string gitRepositoryPath = @"C:\dev\git\Test";

            var graphBuilder = new RepositoryGraphBuilder(gitRepositoryPath);
            DataContext = _viewModel = new MainWindowViewModel(graphBuilder);

            watcher = new FileSystemWatcher(Path.Combine(gitRepositoryPath, ".git"))
                      {
                          IncludeSubdirectories = true,
                          EnableRaisingEvents = true,
                          NotifyFilter =
                              NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.FileName |
                              NotifyFilters.LastWrite
                      };
            watcher.Changed += (o, e) =>
                               {
                                   var vm = _viewModel;
                                   Dispatcher.Invoke(new Action(vm.Refresh), null);
                               };

            InitializeComponent();
        }
    }
}