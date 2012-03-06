using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace SeeGit
{
    public class RepositoryGraphBuilder : IRepositoryGraphBuilder
    {
        private readonly RepositoryGraph _graph = new RepositoryGraph();
        private readonly Repository _repository;
        private readonly Dictionary<string, CommitVertex> _vertices = new Dictionary<string, CommitVertex>();

        public RepositoryGraphBuilder(string gitRepositoryPath)
        {
            GitRepositoryPath = gitRepositoryPath;
            try
            {
                _repository = new Repository(GitRepositoryPath);
            }
            catch (LibGit2Exception)
            {
            }
        }

        public string GitRepositoryPath { get; private set; }

        public RepositoryGraph Graph()
        {
            if (_repository == null) return new RepositoryGraph();

            var commits =
                _repository.Commits.QueryBy(new Filter {SortBy = GitSortOptions.Topological | GitSortOptions.Time});

            if (!commits.Any())
            {
                _graph.Clear();
                return _graph;
            }
            AddCommitsToGraph(commits.First(), null);

            foreach (var branch in _repository.Branches.Where(branch => branch.Commits.Any()))
            {
                var firstCommit = branch.Commits.First();
                var commit = GetCommitVertex(firstCommit);
                commit.Branches.Add(branch.Name);
                AddCommitsToGraph(firstCommit, null);
            }

            return _graph;
        }

        private void AddCommitsToGraph(Commit commit, CommitVertex childVertex)
        {
            var commitVertex = GetCommitVertex(commit);
            _graph.AddVertex(commitVertex);
            if (childVertex != null)
            {
                _graph.AddEdge(new CommitEdge(childVertex, commitVertex));
            }

            foreach (var parent in commit.Parents)
            {
                AddCommitsToGraph(parent, commitVertex);
            }
        }

        private CommitVertex GetCommitVertex(Commit commit)
        {
            CommitVertex commitVertex = null;
            if (!_vertices.TryGetValue(commit.Sha, out commitVertex))
            {
                commitVertex = new CommitVertex(commit.Sha, commit.MessageShort) {Description = commit.Message};
                _vertices.Add(commit.Sha, commitVertex);
            }

            return commitVertex;
        }
    }
}