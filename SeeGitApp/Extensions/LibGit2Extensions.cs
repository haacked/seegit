using SeeGit.Models;

namespace SeeGit
{
    public static class LibGit2Extensions
    {
        public static BranchReference ToBranchReference(this LibGit2Sharp.Branch branch)
        {
            return new BranchReference
                   {
                       Name = branch.Name,
                       IsRemote = branch.IsRemote,
                       IsCurrent = branch.IsRemote,
                       IsHead = branch.IsCurrentRepositoryHead
                   };
        }
    }
}