using System;
using System.Globalization;
using System.IO;
using System.Reactive.Linq;

namespace SeeGit
{
    public static class ModelExtensions
    {
        private const string GitDirectoryName = ".git";

        private readonly static string GitDirectoryNameWithSeparator = string.Concat(GitDirectoryName,
                                                                                    Path.DirectorySeparatorChar);

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
            if (path.EnsureTrailingDirectorySeperator().EndsWith(GitDirectoryNameWithSeparator))
                return path;

            return Path.Combine(path, GitDirectoryName);
        }

        /// <summary>
        /// Appends the <see cref="Path.DirectorySeparatorChar"/> to the string.
        /// </summary>
        /// <param name="path"></param>
        /// <returns>Returns <paramref name="path"/> with the <see cref="Path.DirectorySeparatorChar"/> appended if it is not already present.</returns>
        private static string EnsureTrailingDirectorySeperator(this string path)
        {
            return path.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)) ? path : string.Concat(path,Path.DirectorySeparatorChar);
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