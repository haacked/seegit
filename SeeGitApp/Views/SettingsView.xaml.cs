using SeeGit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            private static List<string> commitAdornerComboList = new List<string>();
            private static Settings _config;

            public SettingsDataContext()
            {
                _config = MainWindow.Configuration;

                SetComboItems();

                CommitAdornerSelectedItem = _config.GetSetting<string>("AdornerCommitMessageVisibility", "ExpandedHidden");
                SHALengthInput = _config.GetSetting<string>("SHALength", "8");
                CommitDescriptionShown = _config.GetSetting<bool>("DescriptionInExpander", false);
            }

            private void SetComboItems()
            {
                commitAdornerComboList.Add("ExpandedHidden");
                commitAdornerComboList.Add("Visible");
                commitAdornerComboList.Add("Hidden");
            }

            // Data bindings
            public List<string> CommitAdornerComboItems
            {
                get { return commitAdornerComboList; }
            }

            public string CommitAdornerSelectedItem
            {
                get
                {
                    return _config.GetSetting<string>("AdornerCommitMessageVisibility", commitAdornerComboList[0]);
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
                    return _config.GetSetting<string>("SHALength", "8");
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
                    return _config.GetSetting<bool>("DescriptionInExpander", false);
                }
                set
                {
                    _config.SetSetting("DescriptionInExpander", value ? "True" : "False");
                }
            }
        }
    }
}
