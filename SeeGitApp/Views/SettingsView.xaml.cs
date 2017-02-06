using System.Collections.Generic;
using System.Windows.Controls;
using SeeGit.Models;

namespace SeeGit.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {

        public SettingsView()
        {
            DataContext = new SettingsDataContext();
            InitializeComponent();
        }

        public class SettingsDataContext
        {
            private static readonly List<string> commitAdornerComboList = new List<string>();
            private static Settings _config;

            public SettingsDataContext()
            {
                _config = MainWindow.Configuration;

                SetComboItems();

                CommitAdornerSelectedItem = _config.GetSetting("AdornerCommitMessageVisibility", "ExpandedHidden");
                SHALengthInput = _config.GetSetting("SHALength", "8");
                CommitDescriptionShown = _config.GetSetting("DescriptionInExpander", false);
            }

            private static void SetComboItems()
            {
                commitAdornerComboList.Add("ExpandedHidden");
                commitAdornerComboList.Add("Visible");
                commitAdornerComboList.Add("Hidden");
            }

            // Data bindings
            public List<string> CommitAdornerComboItems => commitAdornerComboList;

            public string CommitAdornerSelectedItem
            {
                get
                {
                    return _config.GetSetting("AdornerCommitMessageVisibility", commitAdornerComboList[0]);
                }
                set
                {
                    _config.SetSetting("AdornerCommitMessageVisibility", value);
                }
            }

            public string SHALengthInput
            {
                get
                {
                    return _config.GetSetting("SHALength", "8");
                }
                set
                {
                    _config.SetSetting("SHALength", value);
                }
            }

            public bool CommitDescriptionShown
            {
                get
                {
                    return _config.GetSetting("DescriptionInExpander", false);
                }
                set
                {
                    _config.SetSetting("DescriptionInExpander", value ? "True" : "False");
                }
            }

            public bool CommitAuthorShown
            {
                get
                {
                    return _config.GetSetting("AuthorInHeader", false);
                }
                set
                {
                    _config.SetSetting("AuthorInHeader", value ? "True" : "False");
                }
            }
        }
    }
}
