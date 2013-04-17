using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace SeeGit
{
    public static class WindowsExtensions
    {
        private static string _lastDirectory;

        public static string BrowseForFolder(string startingPath)
        {
            string initialDirectory = _lastDirectory ?? startingPath;
            string ret = null;

            try
            {
                var cfd = new CommonOpenFileDialog
                {
                    InitialDirectory = initialDirectory,
                    IsFolderPicker = true,
                };
                
                if (cfd.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    ret = cfd.FileName;
                }
            }
            catch (System.PlatformNotSupportedException)
            {
                var fd = new System.Windows.Forms.FolderBrowserDialog
                {
                    SelectedPath = initialDirectory,
                };

                if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    ret = fd.SelectedPath;
                }
            }

            _lastDirectory = ret;
            return ret;
        }
    }
}