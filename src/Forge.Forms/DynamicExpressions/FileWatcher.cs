using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Forge.Forms.FormBuilding;

namespace Forge.Forms.DynamicExpressions
{
    public sealed class FileWatcher : IStringProxy, IProxy, IDisposable, INotifyPropertyChanged
    {
        private class Watcher : IDisposable
        {
            private readonly List<FileWatcher> listeners;
            private readonly FileSystemWatcher fileSystemWatcher;
            private readonly string filePath;

            private bool isLatestValue;
            private string value;

            public Watcher(FileWatcher initialListener)
            {
                filePath = initialListener.filePath;
                listeners = new List<FileWatcher> { initialListener };
                fileSystemWatcher = new FileSystemWatcher
                {
                    Path = Path.GetDirectoryName(filePath),
                    Filter = Path.GetFileName(filePath),
                    EnableRaisingEvents = true
                };

                fileSystemWatcher.Created += (s, e) => Update();
                fileSystemWatcher.Changed += (s, e) => Update();
                fileSystemWatcher.Deleted += (s, e) => Update();
                fileSystemWatcher.Renamed += (s, e) => Update();
                fileSystemWatcher.Error += (s, e) => Update();
            }

            public string Value
            {
                get
                {
                    lock (this)
                    {
                        if (!isLatestValue)
                        {
                            value = Utilities.TryReadFile(filePath);
                            isLatestValue = true;
                        }

                        return value;

                    }
                }
                private set => this.value = value;
            }

            public int Count => listeners.Count;

            public void AddListener(FileWatcher listener)
            {
                listeners.Add(listener);
            }

            public void RemoveListener(FileWatcher listener)
            {
                listeners.Remove(listener);
            }

            public void Dispose()
            {
                fileSystemWatcher.Dispose();
            }

            private void Update()
            {
                try
                {
                    fileSystemWatcher.EnableRaisingEvents = false;
                    isLatestValue = false;
                    foreach (var listener in listeners)
                    {
                        listener.NotifyChanged();
                    }
                }
                finally
                {
                    fileSystemWatcher.EnableRaisingEvents = true;
                }
            }
        }

        private static readonly Dictionary<string, Watcher> Watchers
            = new Dictionary<string, Watcher>(StringComparer.OrdinalIgnoreCase);

        private readonly string filePath;
        private bool disposed;

        public FileWatcher(string filePath)
        {
            filePath = Path.GetFullPath(filePath ?? throw new ArgumentNullException(nameof(filePath)));
            this.filePath = filePath;
            lock (Watchers)
            {
                if (Watchers.ContainsKey(filePath))
                {
                    Watchers[filePath].AddListener(this);
                }
                else
                {
                    Watchers[filePath] = new Watcher(this);
                }
            }
        }

        ~FileWatcher()
        {
            Dispose(false);
        }

        public string Value
        {
            get
            {
                lock (Watchers)
                {
                    if (disposed)
                    {
                        throw new ObjectDisposedException(nameof(FileWatcher));
                    }

                    return Watchers[filePath].Value;
                }
            }
        }

        public Action ValueChanged { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
            ValueChanged?.Invoke();
        }

        object IProxy.Value => Value;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            lock (Watchers)
            {
                if (disposed)
                {
                    return;
                }

                disposed = true;
                var watcher = Watchers[filePath];
                watcher.RemoveListener(this);
                if (watcher.Count == 0)
                {
                    watcher.Dispose();
                    Watchers.Remove(filePath);
                }
            }
        }
    }
}