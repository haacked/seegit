using SeeGit;
using Xunit;

namespace UnitTests.Models
{
    public class CommitVertexTests
    {
        public class TheCtor
        {
            [Fact]
            public void SetsProperties()
            {
                var commit = new CommitVertex("shasha", "commit message");

                Assert.Equal("shasha", commit.Sha);
                Assert.Equal("commit message", commit.Message);
                Assert.NotNull(commit.Branches);
            }
        }

        public class TheEqualsMethod
        {
            [Fact]
            public void ReturnsTrueIfShasAreSame()
            {
                var commit = new CommitVertex("sha", "whatever");
                var other = new CommitVertex("sha", "doesn't matter");

                Assert.True(commit.Equals(other));
            }

            [Fact]
            public void ReturnsFalseIfShasAreDifferent()
            {
                var commit = new CommitVertex("sha1", "whatever");
                var other = new CommitVertex("sha2", "doesn't matter");

                Assert.False(commit.Equals(other));
            }

            [Fact]
            public void ReturnsFalseWhenComparedToNull()
            {
                Assert.False(new CommitVertex("sha", "message").Equals(null));
            }
        }

        public class TheEqualityOperator
        {
            [Fact]
            public void ReturnsTrueIfShasAreSame()
            {
                var commit = new CommitVertex("sha", "whatever");
                var other = new CommitVertex("sha", "doesn't matter");

                Assert.True(commit == other);
            }

            [Fact]
            public void ReturnsFalseIfShasAreDifferent()
            {
                var commit = new CommitVertex("sha", "whatever");
                var other = new CommitVertex("sha1", "doesn't matter");

                Assert.False(commit == other);
                Assert.True(commit != other);
            }

            [Fact]
            public void ReturnsFalseWhenComparedToNull()
            {
                Assert.False(new CommitVertex("sha", "message") == null);
                Assert.True(new CommitVertex("sha", "message") != null);
            }
        }
    }
}