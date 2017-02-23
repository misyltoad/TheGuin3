using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TheGuin3.Module
{
    public class ModuleRegistry
    {
        public ModuleRegistry()
        {
            Watcher = new FileSystemWatcher();
            Watcher.Path = Config.StaticConfig.Paths.ModulesPath;
            Watcher.NotifyFilter = NotifyFilters.DirectoryName;

            Watcher.Changed += new FileSystemEventHandler(OnChanged);
            Watcher.Created += new FileSystemEventHandler(OnChanged);
            Watcher.Renamed += new RenamedEventHandler(OnRenamed);
            Watcher.Deleted += new FileSystemEventHandler(OnDeleted);

            Watcher.EnableRaisingEvents = true;

            Modules = new List<Module>();

            RecompileAll();
        }

        private void RecompileAll()
        {
            string[] paths = Directory.GetDirectories(Config.StaticConfig.Paths.ModulesPath);
            foreach (var path in paths)
                UpdateOrAddModule(GetModuleNameFromPath(path));
        }

        private void UpdateOrAddModule(string name)
        {
            int searchIndex = ModuleNames.BinarySearch(name);
            if (searchIndex > 0)
                Modules.RemoveAt(searchIndex);

            Module module = new Module(name);
            Modules.Add(module);
        }

        private string GetModuleNameFromPath(string path)
        {
            return path.Substring(Config.StaticConfig.Paths.ModulesPath.Length + 1);
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            UpdateOrAddModule(GetModuleNameFromPath(e.FullPath));
        }

        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            int searchIndex = ModuleNames.BinarySearch(GetModuleNameFromPath(e.FullPath));
            if (searchIndex > 0)
                Modules.RemoveAt(searchIndex);
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            UpdateOrAddModule(GetModuleNameFromPath(e.FullPath));
        }

        public List<string> ModuleNames
        {
            get
            {
                List<string> names = new List<string>();
                foreach (var module in Modules)
                {
                    names.Add(module.Meta.Name);
                }

                return names;
            }
        }
        private FileSystemWatcher Watcher;
        public List<Module> Modules;
    }
}
