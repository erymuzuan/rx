using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.Api
{
    public abstract partial class Adapter
    {
        public string[] SaveSources(Dictionary<string, string> sources, string folder)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            foreach (var cs in sources.Keys)
            {
                var file = Path.Combine(folder, cs);
                File.WriteAllText(file, sources[cs]);
            }
            return sources.Keys.ToArray()
                    .Select(f => Path.Combine(folder, f))
                    .ToArray();
        }

        public override Task<IEnumerable<ValidationError>> ValidateAsync()
        {
            var vr = new ObjectCollection<ValidationError>();
            if (string.IsNullOrWhiteSpace(this.Name))
                vr.Add("Name", "Name cannot be empty");

            return Task.FromResult(vr.AsEnumerable());
        }



        public SphCompilerResult Compile(CompilerOptions options, params string[] files)
        {
            
        }

        public virtual Task OpenAsync(bool verbose = false)
        {
            return Task.FromResult(0);
        }
        public async Task<SphCompilerResult> CompileAsync()
        {

            var options = new CompilerOptions();

            var sourceFolder = Path.Combine(ConfigurationManager.UserSourceDirectory, this.Name);
            var sources = new List<string>();

            foreach (var table in this.Tables)
            {
                var td = await this.GetSchemaDefinitionAsync(table.Name);
                td.CodeNamespace = this.CodeNamespace;
                var es = string.Format("{0}.{1}.schema.json", this.Name.ToLowerInvariant(), table);

                //if (!options.EmbeddedResourceCollection.Contains(es))
                //{
                //    File.WriteAllText(es, td.ToJsonString(true));
                //    options.EmbeddedResourceCollection.Add(es);
                //}

                var codes = td.GenerateCode(this);
                var tdSources = this.SaveSources(codes, sourceFolder);
                sources.AddRange(tdSources);

            }

            var adapterCodes = await this.GenerateSourceCodeAsync(options, this.CodeNamespace);
            var adapterSources = this.SaveSources(adapterCodes, sourceFolder);
            sources.AddRange(adapterSources);

            var odataTranslatorCode = await this.GenerateOdataTranslatorSourceCodeAsync();
            if (null != odataTranslatorCode)
            {
                var odataSource = this.SaveSources(new Dictionary<string, string>
                {
                    {
                        odataTranslatorCode.Item1,
                        odataTranslatorCode.Item2
                    }
                }, sourceFolder);
                sources.AddRange(odataSource);

            }

            var pagingCode = await this.GeneratePagingSourceCodeAsync();
            if (null != pagingCode)
            {
                var pagingSource = this.SaveSources(new Dictionary<string, string>
                {
                    {
                        pagingCode.Item1,
                        pagingCode.Item2
                    }
                }, sourceFolder);
                sources.AddRange(pagingSource);

            }


            var result = this.Compile(options, sources.ToArray());

            if (!result.Result)
                throw new Exception(string.Join("\r\n", result.Errors.Select(e => e.ToString())));

            return result;

        }

        protected abstract Task<Dictionary<string, string>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces);
        protected abstract Task<Tuple<string, string>> GenerateOdataTranslatorSourceCodeAsync();
        protected abstract Task<Tuple<string, string>> GeneratePagingSourceCodeAsync();
        protected abstract Task<TableDefinition> GetSchemaDefinitionAsync(string table);

        public virtual void SaveAssets()
        {

        }


    }
}
