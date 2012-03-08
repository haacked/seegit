using System.Collections.ObjectModel;

namespace SeeGit.Models
{
    public class BranchCollection : ObservableCollection<BranchReference>
    {
        public void Merge(BranchReference branch)
        {
            if (Contains(branch))
            {
                branch.IsCurrent = branch.IsCurrent;
                branch.IsRemote = branch.IsRemote;
                return;
            }
            Add(branch);
        }
    }
}