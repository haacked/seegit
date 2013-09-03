using System;
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

            _graph.LayoutAlgorithmType = "EfficientSugiyama";
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

        private bool HighlightCommit(Commit commit)
        {
            CommitVertex commitVertex = GetCommitVertex(commit);
            if (commitVertex.OnCurrentBranch)
                return false;

            commitVertex.OnCurrentBranch = true;
            return true;
        }

        private void HighlightCommitsOnCurrentBranch(Commit commit, CommitVertex commitVertex)
        {
            Queue<Commit> queue = new Queue<Commit>();
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
            Queue<CommitWithChildVertex> queue = new Queue<CommitWithChildVertex>();
            CommitWithChildVertex commitIter;
            queue.Enqueue(new CommitWithChildVertex(commit, childVertex));

            while (queue.Count != 0)
            {
                commitIter = queue.Dequeue();
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
                commitVertex = new CommitVertex(commit.Sha, commit.MessageShort) { Description = commit.Message };
                _vertices.Add(commit.Sha, commitVertex);
            }

            return commitVertex;
        }
    }
}