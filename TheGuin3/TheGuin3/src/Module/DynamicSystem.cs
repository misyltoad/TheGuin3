using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                while (true)
                {
                    try
                    {
                        code = File.ReadAllText(path);
                        break;
                    }
                    catch (Exception e)
                    {
                    }
                }

                var relativePath = "";
                if (path.Length > Config.StaticConfig.Paths.ModulesPath.Length)
                    relativePath = path.Substring(Config.StaticConfig.Paths.ModulesPath.Length);

                syntaxTrees.Add(CSharpSyntaxTree.ParseText(code, null, relativePath));
            }

            CSharpCompilation compilation = CSharpCompilation.Create(
                Path.GetRandomFileName(),
                syntaxTrees: syntaxTrees.ToArray(),
                references: DependencyAssemblyReferences,
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                );

            var ms = new MemoryStream();
            EmitResult result = compilation.Emit(ms);

            if (!result.Success)
            {
                Console.WriteLine("Compiling module \"{0}\" FAILED!", Module.Meta.Name);
            }

            foreach (var diagnostic in result.Diagnostics)
            {
                // Random info spams console.
                if (diagnostic.Severity == DiagnosticSeverity.Error)
                {
                    Console.WriteLine("({0}) {1} in {2} at {3}: {4}",
                        diagnostic.Id,
                        diagnostic.Severity == DiagnosticSeverity.Error ? "Error" : diagnostic.Severity == DiagnosticSeverity.Warning ? "Warning" : "Info",
                        diagnostic.Location.SourceTree.FilePath,
                        diagnostic.Location.SourceTree.GetLineSpan(diagnostic.Location.SourceSpan).StartLinePosition.Line,
                        diagnostic.GetMessage());
                }
            }

            if (result.Success)
            {
                ms.Seek(0, SeekOrigin.Begin);
                Assembly = AssemblyLoadContext.Default.LoadFromStream(ms);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private Type[] Dependencies => new Type[]
        {
            typeof(object),
            typeof(System.Collections.IList),
            typeof(System.Drawing.Image),
            typeof(System.Net.Http.HttpClient),
        };

        private MetadataReference[] DependencyAssemblyReferences
        {
            get
            {
                List<MetadataReference> references = new List<MetadataReference>();
                foreach (var dependency in Dependencies)
                {
                    references.Add(MetadataReference.CreateFromFile(dependency.GetTypeInfo().Assembly.Location));
                }

                
                references.Add(MetadataReference.CreateFromFile(System.Reflection.Assembly.GetEntryAssembly().Location));

                // HACK FOR .NETCore!

                var dd = typeof(Enumerable).GetTypeInfo().Assembly.Location;
                var coreDir = Directory.GetParent(dd);

                string[] files = Directory.GetFiles(coreDir.FullName);

                foreach (var file in files)
                {
                    if ((Path.GetFileName(file) == "mscorlib.dll" || Path.GetFileName(file).Substring(0, "System".Length) == "System") && Path.GetExtension(file) == ".dll")
                    {
                        references.Add(MetadataReference.CreateFromFile(file));
                    }
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
        private string SourceFolder
        {
            get
            {
                var modulePath = String.Format("{0}/{1}", Config.StaticConfig.Paths.ModulesPath, Module.Meta.Name);
                try { Directory.CreateDirectory(modulePath); } catch { }
                var hookPath = String.Format("{0}/hooks", modulePath);
                try { Directory.CreateDirectory(hookPath); } catch { }

                return hookPath;
            }
        }
        private FileSystemWatcher Watcher;
        private Module Module;
    }
}
