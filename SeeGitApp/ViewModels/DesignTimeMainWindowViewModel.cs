namespace SeeGit
{
    public class DesignTimeMainWindowViewModel : MainWindowViewModel
    {
        public DesignTimeMainWindowViewModel() : base(new DesignTimeGraphBuilder())
        {
        }
    }

    public class DesignTimeGraphBuilder : IRepositoryGraphBuilder
    {
        public RepositoryGraph Graph()
        {
            var graph = new RepositoryGraph();
            var commits = new[]
                          {
                              new CommitVertex("c", "Wrote some code")
                              {Description = "This is a long form description of the commit"},
                              new CommitVertex("b", "Initial commit"),
                              new CommitVertex("a", "Added readme")
                          };

            graph.AddVertexRange(commits);
            graph.AddEdge(new CommitEdge(commits[1], commits[2]));
            graph.AddEdge(new CommitEdge(commits[0], commits[1]));
            return graph;
        }
    }
}