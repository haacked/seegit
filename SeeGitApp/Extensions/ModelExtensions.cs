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

        public static string GetGitRepositoryPath(string path)
        {
            if (path == null) throw new ArgumentNullException(nameof(path));

            //If we are passed a .git directory, just return it straightaway
            var pathDirectoryInfo = new DirectoryInfo(path);
            if (pathDirectoryInfo.Name == GitDirectoryName)
            {
                return path;
            }

            if (!pathDirectoryInfo.Exists) return Path.Combine(path, GitDirectoryName);

            var checkIn = pathDirectoryInfo;

            while (checkIn != null)
            {
                string pathToTest = Path.Combine(checkIn.FullName, GitDirectoryName);
                if (Directory.Exists(pathToTest))
                {
                    return pathToTest;
                }
                else
                {
                    checkIn = checkIn.Parent;
                }
            }

            // This is not good, it relies on the rest of the code being ok
            // with getting a non-git repo dir
            return Path.Combine(path, GitDirectoryName);
        }

        public static IObservable<FileSystemEventArgs> CreateGitRepositoryCreationObservable(string path)
        {
            string expectedGitDirectory = Path.Combine(path, GitDirectoryName);
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
                .Take(1);
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