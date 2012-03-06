using GraphSharp.Controls;
using SeeGit.Models;

namespace SeeGit
{
    public class RepositoryGraphLayout : GraphLayout<CommitVertex, CommitEdge, RepositoryGraph>
    {
        public RepositoryGraphLayout()
        {
            HighlightAlgorithmFactory =
                new ReachableHighlightAlgorithmFactory<CommitVertex, CommitEdge, RepositoryGraph>();
        }
    }
}