using System.Diagnostics;
using QuickGraph;

namespace SeeGit
{
    [DebuggerDisplay("{Source.Sha}..{Target.Sha}")]
    public class CommitEdge : Edge<CommitVertex>
    {
        private string _id;

        public CommitEdge(CommitVertex child, CommitVertex parent)
            : base(child, parent)
        {
        }

        public string Id
        {
            get
            {
                if (_id == null)
                {
                    if (Source != null)
                    {
                        _id = Source.Sha;
                    }
                    _id += "..";
                    if (Target != null)
                    {
                        _id += Target.Sha;
                    }
                }
                return _id;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (CommitEdge)) return false;
            return Equals((CommitEdge)obj);
        }

        public bool Equals(CommitEdge other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Source, other.Target);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Target != null ? Target.GetHashCode() : 0);
                result = (result*397) ^ (Source != null ? Source.GetHashCode() : 0);
                return result;
            }
        }
    }
}