using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace SeeGit
{
    public static class WindowsExtensions
    {
        private static string _lastDirectory;

        public static string BrowseForFolder(string startingPath)
        {
            string ret = null;
            
            var fd = new System.Windows.Forms.FolderBrowserDialog
            {
                SelectedPath = _lastDirectory ?? startingPath,
            };

            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ret = fd.SelectedPath;
                _lastDirectory = ret;
                
            }

            return ret;
        }
    }
}