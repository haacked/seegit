using System.Collections.Generic;
using System.Linq;
using LibGit2Sharp;

namespace SeeGit
{
    using CommitWithChildVertex = KeyValuePair<Commit, CommitVertex>;

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
            catch (LibGit2SharpException)
            {
            }
        }

        public string GitRepositoryPath { get; }

        public RepositoryGraph Graph()
        {
            if (_repository == null) return new RepositoryGraph();

            var commits =
                _repository.Commits.QueryBy(new CommitFilter { SortBy = CommitSortStrategies.Topological | CommitSortStrategies.Time });

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

            // Efficient Sugiyama crashes if we have a single commit repository. So we start with CompoundFDP.
            // The original attempt to fix this, 50ca739aaadd7249f864d17cae060b1a27e22029, had a bug where we never
            // changed the algorithm back to Efficient Sugiyama. This fixes that.
            _graph.LayoutAlgorithmType = 
                _graph.VertexCount == 1
                ? "CompoundFDP"
                : App.LayoutAlgorithm;
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
            HighlightCommitsOnCurrentBranch(headCommit);
        }

        private bool HighlightCommit(Commit commit)
        {
            var commitVertex = GetCommitVertex(commit);
            if (commitVertex.OnCurrentBranch)
                return false;

            commitVertex.OnCurrentBranch = true;
            return true;
        }

        private void HighlightCommitsOnCurrentBranch(Commit commit)
        {
            var queue = new Queue<Commit>();
            queue.Enqueue(commit);

            while (queue.Count != 0)
            {
                commit = queue.Dequeue();
                if (!HighlightCommit(commit))
                    return;

                foreach (var parent in commit.Parents)
                {
                    queue.Enqueue(parent);
                }
            }
        }

        private bool AddCommitToGraph(Commit commit, CommitVertex childVertex)
        {
            var commitVertex = GetCommitVertex(commit);
            _graph.AddVertex(commitVertex);
            if (childVertex != null)
            {
                var edge = new CommitEdge(childVertex, commitVertex);
                if (_edges.ContainsKey(edge.Id))
                    return false;

                _graph.AddEdge(edge);
                _edges.Add(edge.Id, edge);
            }
            return true;
        }

        private void AddCommitsToGraph(Commit commit, CommitVertex childVertex)
        {
            // KeyValuePair is faster than a Tuple in this case.
            // We create as many instances as we pass to the AddCommitToGraph.
            var queue = new Queue<CommitWithChildVertex>();
            queue.Enqueue(new CommitWithChildVertex(commit, childVertex));

            while (queue.Count != 0)
            {
                var commitIter = queue.Dequeue();
                if (!AddCommitToGraph(commitIter.Key, commitIter.Value))
                    continue;

                foreach (var parent in commitIter.Key.Parents)
                {
                    queue.Enqueue(new CommitWithChildVertex(parent, GetCommitVertex(commitIter.Key)));
                }
            }
        }

        private CommitVertex GetCommitVertex(Commit commit)
        {
            CommitVertex commitVertex;
            if (!_vertices.TryGetValue(commit.Sha, out commitVertex))
            {
                commitVertex = new CommitVertex(commit.Sha, commit.MessageShort)
                {
                    Description = commit.Message,
                    Author = commit.Author.Name,
                    CommitDate = commit.Author.When
                };
                _vertices.Add(commit.Sha, commitVertex);
            }

            return commitVertex;
        }
    }
}