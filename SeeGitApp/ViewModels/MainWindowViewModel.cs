using System;
using System.ComponentModel;
using GraphSharp.Controls;
using SeeGit.Models;

namespace SeeGit
{
    public class RepositoryGraphLayout : GraphLayout<CommitVertex, CommitEdge, RepositoryGraph>
    {
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private RepositoryGraph _graph;
        private readonly IRepositoryGraphBuilder _graphBuilder;

        public MainWindowViewModel(IRepositoryGraphBuilder graphBuilder)
        {
            _graphBuilder = graphBuilder;
            Graph = _graphBuilder.Graph();
            LayoutAlgorithmType = "Tree";
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