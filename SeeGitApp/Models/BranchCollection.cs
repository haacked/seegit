using System.Collections.ObjectModel;
using System.Linq;

namespace SeeGit.Models
{
    public class BranchCollection : ObservableCollection<BranchReference>
    {
        public void Merge(BranchReference branch)
        {
            var existing = Items.FirstOrDefault(b => b.Name == branch.Name);
            if (existing != null)
            {
                existing.IsCurrent = branch.IsCurrent;
                existing.IsHead = branch.IsHead;
                existing.IsRemote = branch.IsRemote;
                return;
            }
            Add(branch);
        }
    }
}