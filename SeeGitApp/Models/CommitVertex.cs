using System;
using System.Diagnostics;
using SeeGit.Models;

namespace SeeGit
{
    [DebuggerDisplay("{Sha}: {Message}")]
    public class CommitVertex : GitObject<CommitVertex>, IEquatable<CommitVertex>
    {
        public CommitVertex(string sha, string message)
        {
            Sha = sha;
            Message = message;
            Branches = new BranchCollection();
            Branches.CollectionChanged += (o, e) => RaisePropertyChanged(() => HasBranches);
            ShaLength = MainWindow.Configuration.GetSetting("SHALength", 8);
            DescriptionShown = MainWindow.Configuration.GetSetting("DescriptionInExpander", false);
            AuthorShown = MainWindow.Configuration.GetSetting("AuthorInHeader", false);
            AdornerMessageVisibilityType = MainWindow.Configuration.GetSetting("AdornerCommitMessageVisibility", "ExpandedHidden");
            Expanded = false;
        }

        // Settings
        int _shaLength;
        public int ShaLength
        {
            get
            {
                return _shaLength;
            }
            set
            {
                _shaLength = value;
                RaisePropertyChanged(() => ShortSha);
            }
        }

        bool _descriptionShown;
        public bool DescriptionShown
        {
            get
            {
                return _descriptionShown;
            }
            set
            {
                _descriptionShown = value;
                RaisePropertyChanged(() => DescriptionShown);
            }
        }

        private bool _authorShown;

        public bool AuthorShown
        {
            get
            {
                return _authorShown; 
                
            }
            set
            {
                _authorShown = value;
                RaisePropertyChanged(() => AuthorShown);
            }
        }

        public bool AdornerMessageVisibility
        {
            get;
            set;
        }

        private string _adornerMessageVisibilityType;
        public string AdornerMessageVisibilityType
        {
            set
            {
                if (value.Equals("Visible"))
                {
                    AdornerMessageVisibility = true;
                }
                else if (value.Equals("Hidden"))
                {
                    AdornerMessageVisibility = false;
                }
                _adornerMessageVisibilityType = value;
                RaisePropertyChanged(() => AdornerMessageVisibility);
            }
        }

        public string Sha
        {
            get;
        }

        public string ShortSha => Sha.AtMost(ShaLength);

        public string Message
        {
            get;
        }

        public string Description
        {
            get;
            set;
        }
        public string Author
        {
            get;
            set;
        }

        public DateTimeOffset CommitDate
        {
            get;
            set;
        }

        public BranchCollection Branches
        {
            get;
        }

        public bool HasBranches => Branches.Count > 0;

        private bool _onCurrentBranch;

        public bool OnCurrentBranch
        {
            get
            {
                return _onCurrentBranch;
            }
            set
            {
                _onCurrentBranch = value;
                RaisePropertyChanged(() => OnCurrentBranch);
            }
        }

        public bool Expanded
        {
            set
            {
                if (_adornerMessageVisibilityType.Equals("ExpandedHidden"))
                {
                    AdornerMessageVisibility = !value;
                    RaisePropertyChanged(() => AdornerMessageVisibility);
                }
            }
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CommitVertex);
        }

        public override bool Equals(CommitVertex other)
        {
            return !ReferenceEquals(null, other) && (ReferenceEquals(this, other) || Equals(other.Sha, Sha));
        }

        public override string ToString()
        {
            return $"{ShortSha}: {Message}";
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Sha?.GetHashCode() ?? 0;
                result = (result * 397) ^ (Message?.GetHashCode() ?? 0);
                result = (result * 397) ^ (Description?.GetHashCode() ?? 0);
                return result;
            }
        }

        public static bool operator ==(CommitVertex commit, CommitVertex other)
        {
            return ReferenceEquals(commit, null) ? ReferenceEquals(other, null) : commit.Equals(other);
        }

        public static bool operator !=(CommitVertex commit, CommitVertex other)
        {
            return !(commit == other);
        }
    }
}