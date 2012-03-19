using System;
using System.IO;
using System.Reactive.Linq;

namespace SeeGit
{
    public static class ModelExtensions
    {
        public static string AtMost(this string s, int characterCount)
        {
            if (s == null) return null;
            if (s.Length <= characterCount)
            {
                return s;
            }
            return s.Substring(0, characterCount);
        }

        public static string NormalizeGitRepositoryPath(this string path)
        {
            if (path == null) throw new ArgumentNullException("path");

            //If we are passed a .git directory, just return it straightaway
            if (path.EndsWith(".git", StringComparison.OrdinalIgnoreCase))
            {
                return path;
            }

            return Path.Combine(path, ".git");
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