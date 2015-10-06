namespace SeeGit
{
    public class BranchReference : GitObject<BranchReference>
    {
        public string Name { get; set; }
        public bool IsRemote { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsHead { get; set; }

        public override bool Equals(BranchReference other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Name, Name);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Name?.GetHashCode() ?? 0;
                result = (result*397) ^ IsRemote.GetHashCode();
                result = (result*397) ^ IsCurrent.GetHashCode();
                return result;
            }
        }
    }
}