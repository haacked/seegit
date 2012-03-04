namespace SeeGit
{
    public class DesignTimeMainWindowViewModel : MainWindowViewModel
    {
        protected override void CreateGraphData()
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
    }
}