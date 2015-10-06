using System;
using System.IO;
using System.Windows.Threading;

namespace SeeGit
{
    public class MainWindowViewModel : NotifyPropertyChanged
    {
        private RepositoryGraph _graph;
        private string _repositoryPath;

        private IRepositoryGraphBuilder _graphBuilder;
        // Tree, LinLog, KK, ISOM, EfficientSugiyama, FR, CompoundFDP, BoundedFR, Circular

        private string _layoutAlgorithmType = "Tree";
        private readonly Dispatcher _uiDispatcher;
        private readonly Func<string, IRepositoryGraphBuilder> _graphBuilderThunk;
        private bool _settingsVisible;

        public MainWindowViewModel(Dispatcher uiDispatcher, Func<string, IRepositoryGraphBuilder> graphBuilderThunk)
        {
            _uiDispatcher = uiDispatcher;
            _graphBuilderThunk = graphBuilderThunk ?? (path => new RepositoryGraphBuilder(path));
        }

        public string LayoutAlgorithmType
        {
            get { return _layoutAlgorithmType; }
            private set
            {
                _layoutAlgorithmType = value;
                RaisePropertyChanged(() => LayoutAlgorithmType);
            }
        }

        public RepositoryGraph Graph
        {
            get { return _graph; }
            set
            {
                _graph = value;
                LayoutAlgorithmType = _graph.LayoutAlgorithmType;
                RaisePropertyChanged(() => Graph);
            }
        }

        public string RepositoryPath
        {
            get { return _repositoryPath; }
            private set
            {
                _repositoryPath = value;
                RaisePropertyChanged(() => RepositoryPath);
            }
        }

        public void MonitorRepository(string repositoryWorkingPath)
        {
            if (repositoryWorkingPath == null) return;

            string gitPath = ModelExtensions.GetGitRepositoryPath(repositoryWorkingPath);

            _graphBuilder = _graphBuilderThunk(gitPath);
            RepositoryPath = Directory.GetParent(gitPath).FullName;
            var graph = _graphBuilder.Graph();

            if (graph.VertexCount > 1)
                graph.LayoutAlgorithmType = "EfficientSugiyama";
            Graph = graph;

            if (!Directory.Exists(gitPath))
                MonitorForRepositoryCreation(RepositoryPath);
            else
                MonitorForRepositoryChanges(gitPath);
        }

        private void MonitorForRepositoryCreation(string repositoryWorkingPath)
        {
            ModelExtensions.CreateGitRepositoryCreationObservable(repositoryWorkingPath)
                .Subscribe(_ => _uiDispatcher.Invoke(() => MonitorRepository(repositoryWorkingPath)));
        }

        private void MonitorForRepositoryChanges(string gitRepositoryPath)
        {
            ModelExtensions.CreateGitRepositoryChangesObservable(gitRepositoryPath)
                .Subscribe(_ => _uiDispatcher.Invoke(Refresh));
        }

        public void Refresh()
        {
            string gitPath = ModelExtensions.GetGitRepositoryPath(RepositoryPath);
            if (!Directory.Exists(gitPath))
                MonitorRepository(RepositoryPath);
            else
                Graph = _graphBuilder.Graph();
        }

        public bool ToggleSettings()
        {
            _settingsVisible = !_settingsVisible;
            return _settingsVisible;
        }
    }
}