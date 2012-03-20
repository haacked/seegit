using System.IO;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace SeeGit
{
    public static class WindowsExtensions
    {
        private static string _lastDirectory;

        public static string BrowseForFolder(string startingPath)
        {
            var cfd = new CommonOpenFileDialog
                      {
                          InitialDirectory = _lastDirectory ?? startingPath,
                          IsFolderPicker = true,
                      };

            if (cfd.ShowDialog() == CommonFileDialogResult.Ok)
                _lastDirectory = Path.GetDirectoryName(cfd.FileName);
            else
                return null;

            var ret = cfd.FileName;
            return ret;
        }
    }
}