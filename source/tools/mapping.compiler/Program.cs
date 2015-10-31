using System;
using System.Linq;
using System.IO;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Controllers;
using System.Reflection;

namespace mapping.compiler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("             Transform Definition Compiler ");
            Console.WriteLine("Usage : ");
            Console.WriteLine("mapping.compiler <path.to.transfordefinition.json> <path.to.modified csharp files>");
            Console.WriteLine("if the csharp file is not valid, it will generate the source code");
            Console.WriteLine("This alllow you to modify the file and continue with the compilation");

            if (args.Length != 2) return;
            if (!File.Exists(args[0])) return;
            Console.WriteLine("Reading {0}", Path.GetFileName(args[0]));

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            var json = File.ReadAllText(args[0]);
            var mapping = json.DeserializeFromJson<TransformDefinition>();
            var options = new CompilerOptions();
            options.AddReference<Controller>();
            options.AddReference<TransformDefinitionController>();
            options.AddReference<Newtonsoft.Json.JsonConverter>();

            var cs = args[1];
            if (!File.Exists(cs))
            {
                var codes = mapping.GenerateCode();
                File.WriteAllText(cs, codes[codes.Keys.First()]);
                Console.WriteLine("Your file has been generated, now you can modify the file and press [ENTER] to continue");
                Console.ReadLine();
            }
            Console.WriteLine("compiling.....");
            mapping.OutputType = Strings.GetType(mapping.OutputTypeName);
            Console.WriteLine("Output type : {0} from {1}", mapping.OutputType, mapping.OutputTypeName);
            
            if(!string.IsNullOrWhiteSpace(mapping.InputTypeName))
                Console.WriteLine(mapping.InputType);
            else
            {
                foreach (var p in mapping.InputCollection)
                {
                    var type = Strings.GetType(p.TypeName);
                    Console.WriteLine(type);
                }
            }
            var result = mapping.CompileAsync(options, cs).Result;
            if (!result.Result)
            {
                foreach (var err in result.Errors)
                {
                    Console.WriteLine(err);
                }
                return;
            }
            Console.WriteLine("Successfully compile to {0}", result.Output);


        }

        static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var dlld = Path.Combine(ConfigurationManager.CompilerOutputPath, args.Name + ".dll");
            
            Console.WriteLine("Loading {0}", dlld);
            if (File.Exists(dlld))
                return Assembly.LoadFile(dlld);
            return null;
        }
    }
}
