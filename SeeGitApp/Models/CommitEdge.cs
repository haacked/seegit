using System;
using System.Diagnostics;
using QuikGraph;

namespace SeeGit
{
    [DebuggerDisplay("{Source.Sha}..{Target.Sha}")]
    public class CommitEdge : Edge<CommitVertex>, IEquatable<CommitEdge>
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
                if(_id == null)
                {
                    if(Source != null)
                    {
                        _id = Source.Sha;
                    }
                    _id += "..";
                    if(Target != null)
                    {
                        _id += Target.Sha;
                    }
                }
                return _id;
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CommitEdge);
        }

        public bool Equals(CommitEdge other)
        {
            if(ReferenceEquals(null, other))
                return false;
            if(ReferenceEquals(this, other))
                return true;
            return Source == other.Source && Target == other.Target;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Target?.GetHashCode() ?? 0;
                result = (result * 397) ^ (Source?.GetHashCode() ?? 0);
                return result;
            }
        }

        public static bool operator ==(CommitEdge edge, CommitEdge other)
        {
            return ReferenceEquals(edge, null) ? ReferenceEquals(other, null) : edge.Equals(other);
        }

        public static bool operator !=(CommitEdge edge, CommitEdge other)
        {
            return !(edge == other);
        }
    }
}