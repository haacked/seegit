using System;
using System.IO;
using System.Reactive.Linq;

namespace SeeGit
{
    public static class ReactiveExtensions
    {
        public static IObservable<object> ObserveFileSystemEvents(this FileSystemWatcher watcher)
        {
            return
                Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                    h => watcher.Deleted += h,
                    h => watcher.Deleted -= h)
                    .Select(e => new {e.EventArgs.ChangeType, e.EventArgs.FullPath, e.EventArgs.Name})
                    .Merge(Observable.FromEventPattern<FileSystemEventHandler, FileSystemEventArgs>(
                        h => watcher.Changed += h,
                        h => watcher.Changed -= h)
                               .Select(e => new {e.EventArgs.ChangeType, e.EventArgs.FullPath, e.EventArgs.Name}))
                    .Merge(Observable.FromEventPattern<RenamedEventHandler, RenamedEventArgs>(
                        h => watcher.Renamed += h,
                        h => watcher.Renamed -= h)
                               .Select(e => new
                                            {
                                                e.EventArgs.ChangeType,
                                                e.EventArgs.FullPath,
                                                Name = e.EventArgs.OldName + " renamed to " + e.EventArgs.Name
                                            }));
        }
    }
}