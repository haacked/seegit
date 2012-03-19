namespace SeeGit
{
    public abstract class GitObject<T> : NotifyPropertyChanged
    {
        public abstract override int GetHashCode();

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof (T)) return false;
            return Equals((T)obj);
        }

        public abstract bool Equals(T other);

        
    }
}