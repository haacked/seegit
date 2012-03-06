using System.Linq;
using System.Windows.Threading;

namespace SeeGit
{
    public class DesignTimeMainWindowViewModel : MainWindowViewModel
    {
        public DesignTimeMainWindowViewModel() : base(Dispatcher.CurrentDispatcher, _ => new DesignTimeGraphBuilder())
        {
            Graph = new DesignTimeGraphBuilder().Graph();
        }
    }

    public class DesignTimeGraphBuilder : IRepositoryGraphBuilder
    {
        public RepositoryGraph Graph()
        {
            var graph = new RepositoryGraph();
            var commits = new[]
                          {
                              new CommitVertex("c34173273", "Wrote some code")
                              {Description = "This is a long form description of the commit"},
                              new CommitVertex("b1ae7a123", "Initial commit"),
                              new CommitVertex("aa3823ca1", "Added readme"),
                              new CommitVertex("9e21435fa", "Branching")
                              {Description = "This is a long form description of the commit"}
                          };

            commits.First().Branches.Add("master");
            commits.First().Branches.Add("remotes/origin/master");

            graph.AddVertexRange(commits);
            graph.AddEdge(new CommitEdge(commits[1], commits[2]));
            graph.AddEdge(new CommitEdge(commits[0], commits[1]));
            graph.AddEdge(new CommitEdge(commits[3], commits[2]));
            graph.LayoutAlgorithmType = "EfficientSugiyama";
            return graph;
        }
    }
}