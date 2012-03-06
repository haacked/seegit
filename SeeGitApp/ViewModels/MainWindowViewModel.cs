using System;
using System.ComponentModel;

namespace SeeGit
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private RepositoryGraph _graph;
        private readonly IRepositoryGraphBuilder _graphBuilder;

        public MainWindowViewModel(IRepositoryGraphBuilder graphBuilder, string repositoryPath)
        {
            _graphBuilder = graphBuilder;
            RepositoryPath = repositoryPath;
            Graph = _graphBuilder.Graph();
            LayoutAlgorithmType = "Tree";
                // Tree, LinLog, KK, ISOM, EfficientSugiyama, FR, CompoundFDP, BoundedFR, Circular
        }

        public string LayoutAlgorithmType { get; private set; }

        public RepositoryGraph Graph
        {
            get { return _graph; }
            set
            {
                _graph = value;
                NotifyPropertyChanged("Graph");
            }
        }

        public string RepositoryPath { get; private set;}

        public void Refresh()
        {
            Graph = _graphBuilder.Graph();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}