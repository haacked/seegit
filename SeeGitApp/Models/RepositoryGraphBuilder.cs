using System.Linq;
using LibGit2Sharp;

namespace SeeGit
{
    public class RepositoryGraphBuilder : IRepositoryGraphBuilder
    {
        private readonly RepositoryGraph _graph = new RepositoryGraph();

        public RepositoryGraphBuilder(string gitRepositoryPath)
        {
            GitRepositoryPath = gitRepositoryPath;
        }

        public string GitRepositoryPath { get; private set; }

        public RepositoryGraph Graph()
        {
            var repo = new Repository(GitRepositoryPath);
            var commits = repo.Commits.QueryBy(new Filter {SortBy = GitSortOptions.Topological | GitSortOptions.Time});

            AddCommitsToGraph(_graph, commits.First(), null);

            return _graph;
        }

        private void AddCommitsToGraph(RepositoryGraph graph, Commit commit, CommitVertex childVertex)
        {
            var commitVertex = GetCommitVertex(commit);
            graph.AddVertex(commitVertex);
            if (childVertex != null)
            {
                graph.AddEdge(new CommitEdge(childVertex, commitVertex));
            }

            foreach (var parent in commit.Parents)
            {
                AddCommitsToGraph(graph, parent, commitVertex);
            }
        }

        private CommitVertex GetCommitVertex(Commit commit)
        {
            return new CommitVertex(commit.Sha, commit.MessageShort) {Description = commit.Message};
        }
    }
}