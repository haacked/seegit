using SeeGit;
using SeeGit.Models;
using System;
using Xunit;

namespace UnitTests.Models
{
    public class SettingsTests
    {
        public class TheCtor
        {
            [Fact]
            public void SetsProperties()
            {
                var settings = new Settings();

                // Initial Setting
                settings.SetSetting("TrueSetting", "True");
                settings.SetSetting("FalseSetting", "False");

                // Setting and Removing
                settings.SetSetting("ToBeRemovedSetting", "nan");
                settings.RemoveSetting("ToBeRemovedSetting");

                // Setting and Updating
                settings.SetSetting("UpdatedSetting", "mySetting");
                settings.SetSetting("UpdatedSetting", "notMySetting");

                Assert.Equal(true, Convert.ToBoolean(settings.GetSetting("TrueSetting", string.Empty)));
                Assert.Equal(false, Convert.ToBoolean(settings.GetSetting("FalseSetting", string.Empty)));
                Assert.Equal(1, settings.GetSetting("ToBeRemovedSetting", 1));
                Assert.Equal("notMySetting", settings.GetSetting("UpdatedSetting", string.Empty));
            }
        }

/*        public class GetSetRemoveMethod
        {
            [Fact]
            public void ReturnsTrueIfShasAreSame()
            {
                var commit = new CommitVertex("sha", "whatever");
                var other = new CommitVertex("sha", "doesn't matter");

                Assert.True(commit.Equals(other));
                Assert.True(object.Equals(commit, other));
            }
        }*/
    }
}