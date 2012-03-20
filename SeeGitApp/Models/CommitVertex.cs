using System;
using System.Diagnostics;
using SeeGit.Models;

namespace SeeGit
{
    [DebuggerDisplay("{Sha}: {Message}")]
    public class CommitVertex : GitObject<CommitVertex>, IEquatable<CommitVertex>
    {
        public CommitVertex(string sha, string message)
        {
            Sha = sha;
            Message = message;
            Branches = new BranchCollection();
            Branches.CollectionChanged += (o, e) => RaisePropertyChanged(() => HasBranches);
        }

        public string Sha { get; private set; }

        public string ShortSha
        {
            get { return Sha.AtMost(8); }
        }

        public string Message { get; private set; }
        public string Description { get; set; }

        public BranchCollection Branches { get; private set; }

        public bool HasBranches
        {
            get { return Branches.Count > 0; }
        }

        public override bool Equals(CommitVertex other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Sha, Sha);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", ShortSha, Message);
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

        public static bool operator ==(CommitVertex commit, CommitVertex other)
        {
            return commit.Equals(other);
        }

        public static bool operator !=(CommitVertex commit, CommitVertex other)
        {
            return !commit.Equals(other);
        }
    }
}