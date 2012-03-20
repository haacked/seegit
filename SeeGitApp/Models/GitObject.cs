using System.ComponentModel;
using System.Diagnostics;

namespace SeeGit
{
    public abstract class GitObject<T> : INotifyPropertyChanged
    {
        public abstract override int GetHashCode();

        public override bool Equals(object obj)
        {
            if (!(obj is T)) return false;
            return Equals((T)obj);
        }

        public abstract bool Equals(T other);

        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }
    }
}