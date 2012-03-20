using System.ComponentModel;

namespace SeeGit
{
    public abstract class GitObject<T> : NotifyPropertyChanged
    {
        public abstract override int GetHashCode();

        public override bool Equals(object obj)
        {
            if (!(obj is T)) return false;
            return Equals((T)obj);
        }

        public abstract bool Equals(T other);
    }
}