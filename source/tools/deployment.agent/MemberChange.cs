using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Mangements
{
    public class MemberChange
    {
        public MemberChange()
        {

        }
        public MemberChange(Member mbr, Member existing, string parent, string oldParent)
        {
            this.WebId = mbr.WebId;
            this.Name = mbr.Name;
            NewPath = $"{parent}.{mbr.Name}";
            if (null == existing)
            {
                Action = "Added";
                OldPath = null;
                IsEmpty = false;
                return;
            }

            var oldType = existing.GetMemberTypeName();
            var type = mbr.GetMemberTypeName();
            OldPath = $"{oldParent}.{existing.Name}";
            NewType = mbr.GetMemberTypeName();
            OldType = existing.GetMemberTypeName();

            if (existing.Name != mbr.Name && null != oldType && null != type && oldType == type)
            {
                Action = "NameChanged";
                return;
            }

            // TODO : complex member , named changed, now all it's children has got to change
            if (existing.Name != mbr.Name && null == oldType && null == type)
            {
                Action = "NameChanged";
                return;
            }
            if ((null != oldType || null != type) && oldType != type)
            {
                Action = "TypeChanged";
                MigrationStrategy = MemberMigrationStrategies.Script;
                return;
            }
            IsEmpty = true;
            MigrationStrategy = MemberMigrationStrategies.Ignore;
        }
        public string WebId { get; set; }
        public string Name { get; set; }
        public string Action { get; set; }
        public string NewPath { get; set; }
        public string OldPath { get; set; }
        public string OldType { get; set; }
        public string NewType { get; set; }
        public bool IsEmpty { get; }
        public string MigrationScript { get; set; }
        public MemberMigrationStrategies MigrationStrategy { get; set; }

        public void Migrate(dynamic item, JObject json)
        {
            if (this.IsEmpty) return;
            if (this.MigrationStrategy == MemberMigrationStrategies.Script) this.InvokeScript(item, json);
            if (this.MigrationStrategy != MemberMigrationStrategies.Direct) return;
            if (string.IsNullOrWhiteSpace(this.OldPath)) throw new InvalidOperationException($"OldPath is required for \"{NewPath}\" when using Direct mmigration strategy");


            var oldValue = json.SelectToken(this.OldPath).Value<dynamic>();
            var names = this.NewPath.Replace("$.", "").Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            Type type = item.GetType();
            PropertyInfo prop = null;
            var target = item;

            foreach (var name in names)
            {
                prop = type.GetProperty(name);
                if (null == prop)
                    throw new InvalidOperationException("Cannot read the property type for " + name);
                type = prop.PropertyType;

                if (type.Namespace.StartsWith("Bespoke"))
                {
                    target = prop.GetValue(target);
                }
            }
            try
            {
                if (null != prop && typeof(JValue) == oldValue.GetType())
                    prop.SetValue(target, oldValue.Value);

            }
            catch (Exception e)
            {
                Console.WriteLine("oldValue " + oldValue.ToString(CultureInfo.InvariantCulture));
                Console.WriteLine(e.Message);
            }
        }

        private void InvokeScript(dynamic item, JObject json)
        {
            Type type = item.GetType();
            var cs = $@"
using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.MigrationScripts
{{
    public class Host
    {{

        public void Execute({type.Namespace}.{type.Name} item, string source)
        {{
            item.{this.NewPath.Replace("$.", "")} = this.GetValue(source);
        }}
        {this.MigrationScript}

    }}

}}
";

            var result = this.Compile(cs, type);
            Console.WriteLine(@"============");
            if (!result.Result)
            {
                Console.WriteLine(result);
                Console.WriteLine(cs);
                return;
            }
            Console.WriteLine(@"______________");

            dynamic executor = Activator.CreateInstanceFrom(result.Output, "Bespoke.Sph.MigrationScripts.Host").Unwrap();
            executor.Execute(item, json.ToString());

        }


        public WorkflowCompilerResult Compile(string code, Type entityType)
        {

            using (var provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider())
            {
                var outputPath = ConfigurationManager.CompilerOutputPath;
                var parameters = new CompilerParameters
                {
                    OutputAssembly = Path.Combine(outputPath, $"migration.{this.WebId}.dll"),
                    GenerateExecutable = false,
                    IncludeDebugInformation = true
                };
                parameters.ReferencedAssemblies.Add(typeof(Entity).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Int32).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(Expression<>).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(INotifyPropertyChanged).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(System.Xml.XmlDocument).Assembly.Location);
                parameters.ReferencedAssemblies.Add(typeof(JObject).Assembly.Location);
                parameters.ReferencedAssemblies.Add(entityType.Assembly.Location);

                if (File.Exists(parameters.OutputAssembly))
                    return new WorkflowCompilerResult
                    {
                        Result = true,
                        Output = parameters.OutputAssembly

                    };

                var result = provider.CompileAssemblyFromSource(parameters, code);
                var cr = new WorkflowCompilerResult
                {
                    Result = true,
                    Output = Path.GetFullPath(parameters.OutputAssembly)
                };
                cr.Result = result.Errors.Count == 0;

                foreach (CompilerError error in result.Errors)
                {
                    Console.WriteLine(error);
                    var guid = $"{DateTime.Now:yyyyMMdd-HHmm-ss}" + Guid.NewGuid();
                    var log = Path.Combine(ConfigurationManager.GeneratedSourceDirectory, @"migration.logs");
                    if (!Directory.Exists(log)) Directory.CreateDirectory(log);

                    var logFile = $"{log}\\{guid}.log";
                    var cs = $"{log}\\{guid}.cs";

                    File.WriteAllText(logFile, error.ToString());
                    File.WriteAllText(cs, code);

                }

                return cr;
            }
        }


        public override string ToString()
        {
            if (IsEmpty) return string.Empty;
            return $@"
{Action}: {WebId}
------------------------
{OldPath} -> {NewPath}
{OldType} -> {NewType}";
        }
    }
}