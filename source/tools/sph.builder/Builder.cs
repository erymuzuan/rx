using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Extensions;
using Bespoke.Sph.SourceBuilders.Extensions;

// ReSharper disable ExplicitCallerInfoArgument

namespace Bespoke.Sph.SourceBuilders
{
    public abstract class Builder<T> where T : Entity
    {
        public string[] Compilers { get; }

        protected Builder(string[] compilers)
        {
            Compilers = compilers ?? Array.Empty<string>();
        }
        private readonly ILogger m_logger;
        protected abstract Task<RxCompilerResult> CompileAssetAsync(T item);

        protected virtual async Task<RxCompilerResult> CompileAsync(IProjectDefinition project)
        {

            var results = new List<RxCompilerResult>();
            WriteDebug($"Found {this.DeveloperService.ProjectBuilders.Length} IProjectBuilders");
            var compilers = this.DeveloperService.ProjectBuilders
                .Where(x => x.IsAvailableInBuildMode)
                .ToArray();//.AsQueryable().WhereIf(x => this.Compilers.Contains(x.Name), Compilers.Length > 0).ToArray();
            WriteDebug($"Found {compilers.Length} compilers matching the name");
            foreach (var compiler in compilers)
            {
                WriteMessage($"Building with {compiler.Name}...");
                var dll = $"{ConfigurationManager.CompilerOutputPath}\\{project.AssemblyName}.dll";
                var temp = new FileInfo(Path.GetTempFileName());
                RxCompilerResult cr;
                using (var stream = new FileStream(temp.FullName, FileMode.Create))
                {
                    //TODO : read Debug and Verbose from the command line option
                    var options2 = new CompilerOptions2(stream) { IsDebug = true, IsVerbose = true };
                    cr = await compiler.BuildAsync(project, x => options2);
                    results.Add(cr);
                    WriteMessage($"{compiler.GetType().Name} has {(cr.Result ? "successfully building" : "failed to build")} {project.Name}");
                    WriteMessage(cr.ToString());
                }
                if (!cr.IsEmpty && temp.Exists && temp.Length > 0)
                    temp.CopyTo(dll);
            }

            var final = new RxCompilerResult { Result = results.All(x => x.Result) };
            final.Errors.AddRange(results.SelectMany(x => x.Errors));

            return final;
        }

        protected virtual async Task<IEnumerable<string>> GenerateSourceCodeAsync(IProjectDefinition project)
        {
            var files = new List<string>();
            WriteDebug($"Found {this.DeveloperService.ProjectBuilders.Length} IProjectBuilders");
            var compilers = this.DeveloperService.ProjectBuilders;//.AsQueryable().WhereIf(x => this.Compilers.Contains(x.Name), Compilers.Length > 0).ToArray();
            WriteDebug($"Found {compilers.Length} compilers matching the name");
            foreach (var compiler in compilers)
            {
                WriteMessage($"Writing source code with {compiler.Name}...");
                var sources = await compiler.GenerateCodeAsync(project);
                files.AddRange(sources.Select(x => x.WriteSource(project)));

            }
            return files;

        }


        [Import(typeof(IDeveloperService))]
        public IDeveloperService DeveloperService { get; set; }

        protected Builder()
        {
            m_logger = ObjectBuilder.GetObject<ILogger>();
        }

        public virtual async Task RestoreAllAsync()
        {
            this.Initialize();
            var items = this.GetItems();
            foreach (var asset in items)
            {
                await this.RestoreAsync(asset);
            }
        }

        public async Task<RxCompilerResult> RestoreAsync(T item)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            logger.WriteInfo($"Compiling {typeof(T).Name} : {item.Id} ......");
            var result = await CompileAssetAsync(item);

            if (result.Errors.Any())
                logger.WriteError($" ================ Compiling {typeof(T).Name}[{item.Id}] with {result.Errors.Count} errors , 1 failed ==================");
            else
                logger.WriteInfo($" ================ Compiling {typeof(T).Name}[{item.Id}] 0 errors , 1 succeeded ==================");
            result.Errors.ForEach(x => logger.WriteError(x.ToString()));


            return result;
        }

        public IEnumerable<T> GetItems()
        {
            var context = new SphDataContext();
            var folder = Path.Combine(ConfigurationManager.SphSourceDirectory, typeof(T).Name);

            if (!Directory.Exists(folder))
                return new List<T>();

            var list = context.LoadFromSources<T>().ToList();
            list.ForEach(x => ObjectBuilder.ComposeMefCatalog(x));
            return list;
        }



        public void Initialize()
        {
            ObjectBuilder.ComposeMefCatalog(this);
        }

        public void Clean()
        {
            var name = typeof(T).Name;
            var ed = new EntityDefinition { Name = name, Id = name.ToIdFormat() };
            ObjectBuilder.GetObject<IReadOnlyRepository>()
                .CleanAsync(ed.Name)
                .Wait(500);
        }

        protected void WriteMessage(string message, Severity level = Severity.Info,
            [CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "", [CallerLineNumber]int lineNumber = 0)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            switch (level)
            {
                case Severity.Debug:
                    logger.WriteDebug(message, filePath, memberName, lineNumber);
                    break;
                case Severity.Verbose:
                    logger.WriteVerbose(message, filePath, memberName, lineNumber);
                    break;
                case Severity.Info:
                case Severity.Log:
                    logger.WriteInfo(message, filePath, memberName, lineNumber);
                    break;
                case Severity.Warning:
                    logger.WriteWarning(message, filePath, memberName, lineNumber);
                    break;
                case Severity.Error:
                case Severity.Critical:
                    logger.WriteError(message, filePath, memberName, lineNumber);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }
        }

        protected void WriteDebug(string message,
            [CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "", [CallerLineNumber]int lineNumber = 0)
        {
            m_logger.WriteDebug(message, filePath, memberName, lineNumber);
        }
        protected void WriteError(string message,
            [CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "", [CallerLineNumber]int lineNumber = 0)
        {
            m_logger.WriteError(message, filePath, memberName, lineNumber);
        }

        protected void WriteWarning(string message,
            [CallerFilePath]string filePath = "", [CallerMemberName]string memberName = "", [CallerLineNumber]int lineNumber = 0)
        {
            m_logger.WriteWarning(message, filePath, memberName, lineNumber);
        }




    }
}