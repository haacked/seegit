namespace SeeGit
{
    public static class LibGit2Extensions
    {
        public static BranchReference ToBranchReference(this LibGit2Sharp.Branch branch)
        {
            return new BranchReference
            {
                Name = branch.FriendlyName,
                IsRemote = branch.IsRemote,
                IsHead = branch.IsCurrentRepositoryHead
            };
        }
    }
}