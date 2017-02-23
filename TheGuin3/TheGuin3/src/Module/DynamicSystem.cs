using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Emit;

namespace TheGuin3.Module
{
    class DynamicSystem
    {

        public DynamicSystem(Module module)
        {
            try
            {
                Module = module;
                Watcher = new FileSystemWatcher();
                Watcher.IncludeSubdirectories = true;
                Watcher.Path = SourceFolder;
                Watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite
                             | NotifyFilters.FileName | NotifyFilters.DirectoryName;
                Watcher.Filter = "*.cs";

                Watcher.Changed += new FileSystemEventHandler(OnChanged);
                Watcher.Created += new FileSystemEventHandler(OnChanged);
                Watcher.Renamed += new RenamedEventHandler(OnRenamed);
                Watcher.Deleted += new FileSystemEventHandler(OnDeleted);

                Watcher.EnableRaisingEvents = true;

                Recompile();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void CompileList(string[] paths)
        {
            List<SyntaxTree> syntaxTrees = new List<SyntaxTree>();

            foreach (var path in paths)
            {
                string code = "";
                try
                {
                    code = File.ReadAllText(path);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }

                syntaxTrees.Add(CSharpSyntaxTree.ParseText(code));
            }

            CSharpCompilation compilation = CSharpCompilation.Create(
                Path.GetRandomFileName(),
                syntaxTrees: syntaxTrees.ToArray(),
                references: DependencyAssemblyReferences,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                );

            var ms = new MemoryStream();
            EmitResult result = compilation.Emit(ms);

            foreach (var diagnostic in result.Diagnostics)
            {
                Console.WriteLine(diagnostic.GetMessage());
            }

            if (!result.Success)
                return;

            ms.Seek(0, SeekOrigin.Begin);
            Assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
        }

        private Type[] Dependencies => new Type[]
        {
            typeof(Program),
            typeof(object),
            typeof(System.Drawing.Image),
            typeof(System.Net.Http.HttpClient),
        };

        private MetadataReference[] DependencyAssemblyReferences
        {
            get
            {
                List<MetadataReference> references = new List<MetadataReference>();
                foreach(var dependency in Dependencies)
                {
                    references.Add(MetadataReference.CreateFromFile(dependency.GetTypeInfo().Assembly.Location));
                }
                return references.ToArray();
            }
        }

        private void Recompile()
        {
            string[] allfiles = Directory.GetFiles(SourceFolder, "*.cs", SearchOption.AllDirectories);
            foreach (var filePath in allfiles)
            {
                CompileList(allfiles);
            }
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            Recompile();
        }

        private void OnDeleted(object source, FileSystemEventArgs e)
        {
            Recompile();
        }

        private void OnRenamed(object source, RenamedEventArgs e)
        {
            Recompile();
        }

        public Assembly Assembly;
        private string SourceFolder => String.Format("{0}/{1}/hooks", Config.StaticConfig.Paths.ModulesPath, Module.Meta.Name);
        private FileSystemWatcher Watcher;
        private Module Module;
    }
}
