using System.Diagnostics;
using QuickGraph;

namespace SeeGit
{
    [DebuggerDisplay("{Source.Sha}..{Target.Sha}")]
    public class CommitEdge : Edge<CommitVertex>
    {
        public CommitEdge(CommitVertex child, CommitVertex parent)
            : base(child, parent)
        {
        }
    }
}