using QuickGraph;

namespace SeeGit
{
    public class RepositoryGraph : BidirectionalGraph<CommitVertex, CommitEdge>
    {
        public string LayoutAlgorithmType { get; set; }
    }
}