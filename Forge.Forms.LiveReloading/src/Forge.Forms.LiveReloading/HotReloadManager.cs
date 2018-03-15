using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using Forge.Forms.Controls;
using Forge.Forms.LiveReloading.Annotations;
using Forge.Forms.LiveReloading.Extensions;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using Proxier.Extensions;

namespace Forge.Forms.LiveReloading
{
    /// <summary>
    ///     Hot reload manager
    /// </summary>
    public class HotReloadManager
    {
        private bool watchAllFiles = true;

        public HotReloadManager()
        {
            DynamicForm.InterceptorChain.Add(new HotReloadInterceptor());
            Initialize();
        }

        /// <summary>
        /// Gets the static instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static HotReloadManager Instance { get; } = new HotReloadManager();


        /// <summary>
        ///     Directories to be watched
        /// </summary>
        public ObservableCollection<string> Directories { get; }
            = new ObservableCollection<string>();

        public bool WatchAllFiles
        {
            private get => watchAllFiles;
            set
            {
                if (value == watchAllFiles) return;

                watchAllFiles = value;
                Initialize();
            }
        }

        private List<FileSystemWatcher> Watchers { get; } = new List<FileSystemWatcher>();

        private CSharpCodeProvider CodeDom { get; } = CreateCSharpCodeProvider();

        ~HotReloadManager()
        {
            CodeDom.Dispose();
        }


        private void Initialize()
        {
            if (Watchers.Count > 0)
            {
                foreach (var systemWatcher in Watchers) systemWatcher.Dispose();

                Watchers.Clear();
            }

            if (!WatchAllFiles)
            {
                var filesToWatch = AppDomain.CurrentDomain.GetAssemblies()
                    .Where(i => !i.IsDynamic && i.IsAssemblyDebugBuild())
                    .SelectMany(i => i.GetLoadableTypes())
                    .Where(i => i.GetCustomAttribute<HotReloadAttribute>() != null)
                    .Select(i => new
                    {
                        FileName = Path.GetFileName(
                            i.GetCustomAttribute<HotReloadAttribute>().FilePath),
                        Directory = Path.GetDirectoryName(i.GetCustomAttribute<HotReloadAttribute>().FilePath)
                    });

                foreach (var toWatch in filesToWatch) AddWatcher(toWatch.Directory, toWatch.FileName);

                return;
            }

            Directories.CollectionChanged += DirectoriesOnCollectionChanged;

            foreach (var path in FindProjects()) Directories.Add(path);
        }

        private static IEnumerable<string> FindProjects()
        {
            return AppDomain.CurrentDomain.GetAssemblies().Where(i => !i.IsDynamic && i.IsAssemblyDebugBuild())
                .Select(i => Path.GetDirectoryName(i?.CodeBase.Replace("file:///", ""))).Distinct()
                .Select(i => i.FindProjectRoot())
                .Where(i => !string.IsNullOrEmpty(i))
                .ToList();
        }


        private void DirectoriesOnCollectionChanged(object s, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (var item in e.NewItems)
                {
                    if (!(item is string directory)) continue;

                    AddWatcher(directory);
                }
            else if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (var item in e.OldItems)
                {
                    if (!(item is string directory)) continue;

                    Watchers.Remove(Watchers.First(i => i.Path == directory));
                }
        }

        private void AddWatcher(string directory1, string filter = "*.cs")
        {
            if (Watchers.Any(i => i.Path == directory1 && i.Filter == filter)) return;

            var watcher = new FileSystemWatcher
            {
                NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                                        | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = filter,
                Path = directory1,
                IncludeSubdirectories = true
            };

            watcher.Changed += OnChanged;
            watcher.Error += (sender, eventArgs) => { };
            watcher.EnableRaisingEvents = true;
            Watchers.Add(watcher);
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                var types = GetTypesFromFile(e.FullPath).ToList();

                foreach (var type in types)
                {
                    var attr = type.GetCustomAttribute<HotReloadAttribute>() ?? new HotReloadAttribute();
                    if (attr.IsPersistent) HotReloadInterceptor.AddOrReplaceReplacement(GetBaseType(type), type);
                }

                ApplyTypesToDynamicForms(types);
            }
            catch
            {
                // ignored
            }
        }

        private static Type GetBaseType(Type type)
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(i => !i.IsDynamic && i.IsAssemblyDebugBuild())
                .Select(i => i.GetLoadableTypes()).SelectMany(i => i).Single(i => i.FullName == type.FullName);
        }

        /// <summary>
        ///     Applies the types to dynamic forms.
        /// </summary>
        /// <param name="types">The types.</param>
        public static void ApplyTypesToDynamicForms(List<Type> types)
        {
            Application.Current.Dispatcher.Invoke(delegate
            {
                var dynamicForms =
                    DynamicForm.ActiveForms.Where(i =>
                        types.Select(o => o.FullName).Contains(i.Model?.GetType().FullName));

                foreach (var dynamicForm in dynamicForms)
                    dynamicForm.Model =
                        dynamicForm.Model.CopyTo(types.Last(i => i.FullName == dynamicForm.Model?.GetType().FullName));
            });
        }

        private static CSharpCodeProvider CreateCSharpCodeProvider()
        {
            var csc = new CSharpCodeProvider();
            var settings = csc
                .GetType()
                .GetField("_compilerSettings", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(csc);

            var path = settings?.GetType()
                .GetField("_compilerFullPath", BindingFlags.Instance | BindingFlags.NonPublic);

            path?.SetValue(settings,
                Path.Combine(
                    Path.Combine(
                        Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                        throw new InvalidOperationException(), "roslyn-bin")
                    ,
                    "csc.exe"));

            return csc;
        }

        private IEnumerable<Type> GetTypesFromFile(string path)
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(stream))
                {
                    return CompileCode(reader.ReadToEnd());
                }
            }
        }

        /// <summary>
        ///     Compiles the code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public IEnumerable<Type> CompileCode(string code)
        {
            var results = CodeDom.CompileAssemblyFromSource(CreateCompilerParameters(), code);
            if (results.Errors.HasErrors)
            {
                var sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                    sb.AppendLine($"Error ({error.ErrorNumber}): {error.ErrorText}");

                throw new InvalidOperationException(sb.ToString());
            }

            var assembly = results.CompiledAssembly;
            return assembly.GetTypes();
        }

        private static CompilerParameters CreateCompilerParameters()
        {
            var parameters = new CompilerParameters();

            parameters.ReferencedAssemblies.AddRange(
                AppDomain.CurrentDomain.GetAssemblies().Where(i => !i.IsDynamic).Select(i => i.Location).ToArray());
            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = false;
            return parameters;
        }
    }
}