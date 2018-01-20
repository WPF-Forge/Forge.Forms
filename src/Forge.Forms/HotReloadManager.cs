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
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using Microsoft.Win32;
using Proxier.Extensions;

namespace Forge.Forms
{
    /// <summary>
    /// Hot reload manager
    /// </summary>
    public static class HotReloadManager
    {
        private static ObservableCollection<string> Directories { get; set; }
            = new ObservableCollection<string>();

        static HotReloadManager()
        {
            Directories.CollectionChanged += DirectoriesOnCollectionChanged;
        }

        private static void DirectoriesOnCollectionChanged(object s, NotifyCollectionChangedEventArgs e)
        {
            foreach (var item in e.NewItems)
            {
                if (!(item is string directory))
                {
                    continue;
                }
                var watcher = new FileSystemWatcher
                {
                    NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                                   | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                    Filter = "*.cs",
                    Path = directory,
                    IncludeSubdirectories = true
                };

                watcher.Changed += OnChanged;
                watcher.Error += (sender, eventArgs) => { };
                watcher.EnableRaisingEvents = true;
            }
        }

        private static void OnChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                var types = GetTypesFromFile(e.FullPath).ToList();
                ApplyTypesToDynamicForms(types);
            }
            catch
            {
                // ignored
            }
        }

        /// <summary>
        /// Applies the types to dynamic forms.
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
                {
                    dynamicForm.Model =
                        dynamicForm.Model.CopyTo(types.Last(i => i.FullName == dynamicForm.Model?.GetType().FullName));
                }
            });
        }

        /// <summary>
        /// Gets the visual studio installed path.
        /// </summary>
        /// <returns></returns>
        internal static string GetVisualStudioInstalledPath()
        {
            var visualStudioInstalledPath = string.Empty;
            var visualStudioRegistryPath =
                Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\VisualStudio\14.0");
            if (visualStudioRegistryPath != null)
            {
                visualStudioInstalledPath = visualStudioRegistryPath.GetValue("InstallDir", string.Empty) as string;
            }

            if (string.IsNullOrEmpty(visualStudioInstalledPath) || !Directory.Exists(visualStudioInstalledPath))
            {
                visualStudioRegistryPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\14.0");
                if (visualStudioRegistryPath != null)
                {
                    visualStudioInstalledPath = visualStudioRegistryPath.GetValue("InstallDir", string.Empty) as string;
                }
            }

            if (string.IsNullOrEmpty(visualStudioInstalledPath) || !Directory.Exists(visualStudioInstalledPath))
            {
                visualStudioRegistryPath =
                    Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\VisualStudio\12.0");
                if (visualStudioRegistryPath != null)
                {
                    visualStudioInstalledPath = visualStudioRegistryPath.GetValue("InstallDir", string.Empty) as string;
                }
            }

            if (string.IsNullOrEmpty(visualStudioInstalledPath) || !Directory.Exists(visualStudioInstalledPath))
            {
                visualStudioRegistryPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\VisualStudio\12.0");
                if (visualStudioRegistryPath != null)
                {
                    visualStudioInstalledPath = visualStudioRegistryPath.GetValue("InstallDir", string.Empty) as string;
                }
            }

            if (string.IsNullOrEmpty(visualStudioInstalledPath) || !Directory.Exists(visualStudioInstalledPath))
            {
                visualStudioRegistryPath =
                    Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Microsoft\VisualStudio\SxS\VS7");
                if (visualStudioRegistryPath != null)
                {
                    visualStudioInstalledPath = visualStudioRegistryPath.GetValue("15.0", string.Empty) as string;
                }
            }

            return visualStudioInstalledPath;
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
                    $"{GetVisualStudioInstalledPath()}MSBuild\\15.0\\Bin\\Roslyn",
                    "csc.exe"));

            return csc;
        }

        private static IEnumerable<Type> GetTypesFromFile(string path)
        {
            using (var stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var reader = new StreamReader(stream))
                {
                    var readToEnd = reader.ReadToEnd();
                    return CompileCode(readToEnd);
                }
            }
        }

        /// <summary>
        /// Compiles the code.
        /// </summary>
        /// <param name="code">The code.</param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public static IEnumerable<Type> CompileCode(string code)
        {
            var provider = CreateCSharpCodeProvider();

            var parameters = new CompilerParameters();

            foreach (var assemblyName in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    parameters.ReferencedAssemblies.Add(assemblyName.CodeBase.Replace("file:///", ""));
                }
                catch
                {
                    // ignored
                }
            }


            parameters.GenerateInMemory = true;
            parameters.GenerateExecutable = false;
            var results = provider.CompileAssemblyFromSource(parameters, code);
            if (results.Errors.HasErrors)
            {
                var sb = new StringBuilder();

                foreach (CompilerError error in results.Errors)
                {
                    sb.AppendLine($"Error ({error.ErrorNumber}): {error.ErrorText}");
                }

                throw new InvalidOperationException(sb.ToString());
            }

            var assembly = results.CompiledAssembly;
            return assembly.GetTypes();
        }
    }
}
