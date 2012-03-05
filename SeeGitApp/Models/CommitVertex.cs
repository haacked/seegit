using System.Collections.Generic;
using System.Diagnostics;

namespace SeeGit
{
    [DebuggerDisplay("{Sha}: {Message}")]
    public class CommitVertex
    {
        public CommitVertex(string sha, string message)
        {
            Sha = sha;
            Message = message;
            Branches = new List<string>();
        }

        public string Sha { get; private set; }

        public string ShortSha
        {
            get { return Sha.AtMost(8); }
        }

        public string Message { get; private set; }
        public string Description { get; set; }

        public override string ToString()
        {
            return string.Format("{0}: {1}", ShortSha, Message);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (CommitVertex)) return false;
            return Equals((CommitVertex)obj);
        }

        public bool Equals(CommitVertex other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Sha, Sha);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (Sha != null ? Sha.GetHashCode() : 0);
                result = (result*397) ^ (Message != null ? Message.GetHashCode() : 0);
                result = (result*397) ^ (Description != null ? Description.GetHashCode() : 0);
                return result;
            }
        }

        public IList<string> Branches { get; private set; }
    }
}