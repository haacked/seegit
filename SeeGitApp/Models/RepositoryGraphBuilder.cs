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
        private readonly Dictionary<string, CommitEdge> _edges = new Dictionary<string, CommitEdge>();

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

            foreach (var vertice in _vertices.Values)
            {
                if (vertice.Branches.Any())
                {
                    vertice.Branches.Clear();
                }
                vertice.OnCurrentBranch = false;
            }

            if (!commits.Any()) return _graph;
            AddCommitsToGraph(commits.First(), null);

            AddBranchReferences();
            AddHeadReference();

            if (_vertices.Count > 1)
            {
                _graph.LayoutAlgorithmType = "EfficientSugiyama";
            }

            return _graph;
        }

        void AddBranchReferences()
        {
            foreach (var branch in _repository.Branches.Where(branch => branch.Commits.Any()))
            {
                var firstCommit = branch.Commits.First();
                var commit = GetCommitVertex(firstCommit);
                commit.Branches.Merge(branch.ToBranchReference());
                AddCommitsToGraph(firstCommit, null);
            }
        }

        void AddHeadReference()
        {
            var head = _repository.Head;
            var headCommit = head.Commits.First();
            var headCommitVertex = GetCommitVertex(headCommit);
            var headBranceReference = new BranchReference
            {
                Name = "HEAD",
                IsRemote = false,
                IsHead = true
            };

            headCommitVertex.Branches.Merge(headBranceReference);
            AddCommitsToGraph(headCommit, null);
            HighlightCommitsOnCurrentBranch(headCommit, headCommitVertex);
        }

        private void HighlightCommitsOnCurrentBranch(Commit commit, CommitVertex commitVertex)
        {
            if (commitVertex.OnCurrentBranch) return;
            commitVertex.OnCurrentBranch = true;

            foreach (var parent in commit.Parents)
            {
                HighlightCommitsOnCurrentBranch(parent, GetCommitVertex(parent));
            }
        }

        private void AddCommitsToGraph(Commit commit, CommitVertex childVertex)
        {
            var commitVertex = GetCommitVertex(commit);
            _graph.AddVertex(commitVertex);
            if (childVertex != null)
            {
                var edge = new CommitEdge(childVertex, commitVertex);
                if (_edges.ContainsKey(edge.Id)) return;
                _graph.AddEdge(edge);
                _edges.Add(edge.Id, edge);
            }

            foreach (var parent in commit.Parents)
            {
                AddCommitsToGraph(parent, commitVertex);
            }
        }

        private CommitVertex GetCommitVertex(Commit commit)
        {
            CommitVertex commitVertex;
            if (!_vertices.TryGetValue(commit.Sha, out commitVertex))
            {
                commitVertex = new CommitVertex(commit.Sha, commit.MessageShort) { Description = commit.Message };
                _vertices.Add(commit.Sha, commitVertex);
            }

            return commitVertex;
        }
    }
}