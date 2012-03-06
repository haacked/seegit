using System;
using System.IO;
using System.Reactive.Linq;

namespace SeeGit
{
    public static class ReactiveExtensions
    {
        public static IObservable<FileSystemEventArgs> ObserveFileSystemChangeEvents(this FileSystemWatcher watcher)
        {
            return Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                h => watcher.Changed += h,
                h => watcher.Changed -= h)
                .Select(e => e.EventArgs);
        }
    }
}