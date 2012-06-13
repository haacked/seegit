using System;
using System.IO;
using System.Reactive.Linq;

namespace SeeGit
{
    public static class ModelExtensions
    {
        private const string GitDirectoryName = ".git";

        public static string AtMost(this string s, int characterCount)
        {
            if (s == null) return null;
            if (s.Length <= characterCount)
            {
                return s;
            }
            return s.Substring(0, characterCount);
        }

        public static string GetGitRepositoryPath(DirectoryInfo pathInfo)
        {
            if (pathInfo == null) 
                throw new ArgumentNullException("pathInfo");

            // given direct .git path (e.g. C:\myrepo\.git) so throw right-away if not found
            if (pathInfo.Name == GitDirectoryName && !pathInfo.Exists) 
                throw new DirectoryNotFoundException(GitDirectoryName);

            var potentialGitDirectoryPath = Path.Combine(pathInfo.FullName, GitDirectoryName);

            if (Directory.Exists(potentialGitDirectoryPath))
                return potentialGitDirectoryPath;

            if (pathInfo.Parent == null)
                throw new DirectoryNotFoundException(GitDirectoryName);

            return GetGitRepositoryPath(pathInfo.Parent);
        }

        public static IObservable<FileSystemEventArgs> CreateGitRepositoryCreationObservable(string path)
        {
            string expectedGitDirectory = Path.Combine(path, ".git");
            return new FileSystemWatcher(path)
                   {
                       IncludeSubdirectories = false,
                       EnableRaisingEvents = true,
                       NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.DirectoryName
                   }.ObserveFileSystemCreateEvents()
                .Where(
                    e =>
                    e.ChangeType == WatcherChangeTypes.Created &&
                    e.FullPath.Equals(expectedGitDirectory, StringComparison.OrdinalIgnoreCase))
                .Throttle(TimeSpan.FromSeconds(1));
        }

        public static IObservable<FileSystemEventArgs> CreateGitRepositoryChangesObservable(string path)
        {
            return new FileSystemWatcher(path)
                   {
                       IncludeSubdirectories = true,
                       EnableRaisingEvents = true,
                       NotifyFilter = NotifyFilters.CreationTime | NotifyFilters.DirectoryName | NotifyFilters.LastWrite
                   }.ObserveFileSystemChangeEvents()
                .Throttle(TimeSpan.FromSeconds(1));
        }
    }
}