using SeeGit;
using Xunit;

namespace UnitTests.Models
{
    public class CommitEdgeTests
    {
        public class TheEqualsMethod
        {
            [Fact]
            public void ReturnsTrueIfSourceAndTargetAreSame()
            {
                var edge = new CommitEdge(new CommitVertex("sha-child", "child commit"),
                                          new CommitVertex("sha-parent", "parent commit"));
                var other = new CommitEdge(new CommitVertex("sha-child", "another child commit"),
                                           new CommitVertex("sha-parent", "another parent commit"));

                Assert.True(edge.Equals(other));
                Assert.True(object.Equals(edge, other));
            }

            [Fact]
            public void ReturnsFalseIfSourceAndTargetAreSame()
            {
                var edge = new CommitEdge(new CommitVertex("sha1-child", "child commit"),
                                          new CommitVertex("sha-parent", "parent commit"));
                var other = new CommitEdge(new CommitVertex("sha-child", "another child commit"),
                                           new CommitVertex("sha-parent", "another parent commit"));

                Assert.False(edge.Equals(other));
                Assert.False(object.Equals(edge, other));
            }

            [Fact]
            public void ReturnsFalseWhenComparedToNull()
            {
                var edge = new CommitEdge(new CommitVertex("sha1-child", "child commit"),
                                          new CommitVertex("sha-parent", "parent commit"));

                Assert.False(edge.Equals(null));
                Assert.False(object.Equals(edge, null));
            }
        }

        public class TheEqualityOperator
        {
            [Fact]
            public void ReturnsTrueIfShaAreSame()
            {
                var edge = new CommitEdge(new CommitVertex("sha-child", "child commit"),
                                          new CommitVertex("sha-parent", "parent commit"));
                var other = new CommitEdge(new CommitVertex("sha-child", "another child commit"),
                                           new CommitVertex("sha-parent", "another parent commit"));

                Assert.True(edge == other);
            }

            [Fact]
            public void ReturnsFalseIfEitherShaIsDifferent()
            {
                var edge = new CommitEdge(new CommitVertex("sha-child", "child commit"),
                                          new CommitVertex("sha-parent", "parent commit"));
                var other = new CommitEdge(new CommitVertex("sha1-child", "another child commit"),
                                           new CommitVertex("sha-parent", "another parent commit"));

                Assert.False(edge == other);
                Assert.True(edge != other);
            }

            [Fact]
            public void ReturnsFalseWhenComparedToNull()
            {
                var edge = new CommitEdge(new CommitVertex("sha-child", "child commit"),
                                          new CommitVertex("sha-parent", "parent commit"));
              
                Assert.False(edge == null);
                Assert.True(edge != null);
            }
        }
    }
}