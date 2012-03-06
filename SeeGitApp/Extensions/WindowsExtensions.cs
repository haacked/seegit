using Microsoft.WindowsAPICodePack.Dialogs;

namespace SeeGit
{
    public static class WindowsExtensions
    {
        public static string BrowseForFolder(string startingPath)
        {
            var cfd = new CommonOpenFileDialog
                      {
                          DefaultFileName = startingPath,
                          IsFolderPicker = true,
                      };

            if (cfd.ShowDialog() != CommonFileDialogResult.Ok) return null;

            var ret = cfd.FileName;
            return ret;
        }
    }
}