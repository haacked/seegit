using System;
using System.ComponentModel;
using GraphSharp.Controls;

namespace SeeGit
{
    public class RepositoryGraphLayout : GraphLayout<CommitVertex, CommitEdge, RepositoryGraph>
    {
    }

    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private RepositoryGraph _graph;

        public MainWindowViewModel()
        {
            _graph = new RepositoryGraph();
            PopulateGraph();
            LayoutAlgorithmType = "Tree";
        }

        public void PopulateGraph()
        {
            _graph.Clear();
            CreateGraphData();
            NotifyPropertyChanged("Graph");
        }

        protected virtual void CreateGraphData()
        {
            var commits = new[]
                          {
                              new CommitVertex("c", "Wrote some code")
                              {Description = "This is a long form description of the commit"},
                              new CommitVertex("b", "Initial commit"),
                              new CommitVertex("a", "Added readme")
                          };

            Graph.AddVertexRange(commits);
            Graph.AddEdge(new CommitEdge(commits[1], commits[2]));
            Graph.AddEdge(new CommitEdge(commits[0], commits[1]));
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