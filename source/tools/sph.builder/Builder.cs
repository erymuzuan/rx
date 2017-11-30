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
// ReSharper disable ExplicitCallerInfoArgument

namespace Bespoke.Sph.SourceBuilders
{
    public abstract class Builder<T> where T : Entity
    {
        private readonly ILogger m_logger;
        protected abstract Task<RxCompilerResult> CompileAssetAsync(T item);

        protected virtual async Task<RxCompilerResult> CompileAsync(IProjectDefinition project)
        {
            var results = new List<RxCompilerResult>();
            WriteDebug($"Found {this.DeveloperService.ProjectBuilders.Length} IProjectBuilders");
            foreach (var compiler in this.DeveloperService.ProjectBuilders)
            {
                WriteDebug($"Building with {compiler.GetType().Name}...");
                var sources = await compiler.GenerateCodeAsync(project);
                foreach (var src in sources)
                {
                    var path = $@"{ConfigurationManager.SphSourceDirectory}\EntityDefinition\{src.Key}";
                    if (Path.IsPathRooted(src.Key))
                        path = src.Key;
                    File.WriteAllText(path, src.Value);
                }
                WriteDebug($"Savings source files {sources.Keys.ToString(", ")}");
                var cr = await compiler.BuildAsync(project, sources.Keys.ToArray());
                results.Add(cr);
                WriteMessage($"{compiler.GetType().Name} has {(cr.Result ? "successfully building" : "failed to build")} {project.Name}");
                WriteMessage(cr.ToString());
            }

            var final = new RxCompilerResult { Result = results.All(x => x.Result) };
            final.Errors.AddRange(results.SelectMany(x => x.Errors));

            return final;
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