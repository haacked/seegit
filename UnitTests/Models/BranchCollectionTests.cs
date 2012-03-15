using System.Linq;
using SeeGit.Models;
using Xunit;

namespace UnitTests
{
    public class BranchCollectionTests
    {
        public class TheMergeMethod
        {
            [Fact]
            public void AddsBranchToCollection()
            {
                var branches = new BranchCollection();

                branches.Merge(new SeeGit.BranchReference {Name = "foo"});

                Assert.Equal(1, branches.Count);
            }

            [Fact]
            public void MergesBranchWithSameNameAsExistingGivingPrecedenceToNewerOne()
            {
                var branches = new BranchCollection();
                branches.Merge(new SeeGit.BranchReference {Name = "foo", IsCurrent = false, IsHead = true, IsRemote = true});

                branches.Merge(new SeeGit.BranchReference {Name = "foo", IsCurrent = true, IsRemote = true, IsHead = true});

                Assert.Equal(1, branches.Count);
                Assert.True(branches.First().IsCurrent);
            }
        }
    }
}