using System;
using System.IO;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace mapping.test.runner
{
    public class Program
    {
        private static readonly string m_outputFolder = $"{ConfigurationManager.GeneratedSourceDirectory}\\TransformDefinition.test-output\\";
        public static int Main(string[] args)
        {
            if (!Directory.Exists(m_outputFolder))
                Directory.CreateDirectory(m_outputFolder);

            var id = args.FirstOrDefault();
            while (string.IsNullOrWhiteSpace(id))
            {

                Console.WriteLine(@"Select your TransformDefinition");
                var files = Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\TransformDefinition",
                    "*.json");
                Console.WriteLine($@"| #   | {"id".ToFixString(30)} | {"input".ToFixString(40)} | {"output".ToFixString(40)} |");
                Console.WriteLine(new string('-', 120));
                for (var i = 0; i < files.Length; i++)
                {
                    try
                    {
                        var m = files[i].DeserializeFromJsonFile<TransformDefinition>();
                        Console.WriteLine($@"| {i.ToFixString(3)} | {m.Id.ToFixString(30)} | {m.InputTypeName.ToFixString(40)} | {m.OutputTypeName.ToFixString(40)} | ");
                    }
                    catch (Exception e)
                    {
                        Error(new LogEntry(e));
                    }
                }
                var index = Console.ReadLine();
                int idx;
                if (int.TryParse(index, out idx))
                    id = Path.GetFileNameWithoutExtension(files[idx]);
            }

            var source = $"{ConfigurationManager.SphSourceDirectory}\\TransformDefinition\\{id}.json";
            var map = source.DeserializeFromJsonFile<TransformDefinition>();


            string file = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(TransformDefinition)}\\{id}.test";
            if (!File.Exists(file))
                return NotFound("You have yet to create an input for the test map");

            var inputType = map.GetInputType();
            if (null == inputType)
                return NotFound($"Cannot instantiate input type : {map.InputTypeName}");

            var mapType = map.GetMapType();
            if (null == mapType)
                return NotFound($"Cannot load your mapping type: {map.FullTypeName} from {map.AssemblyName}.dll");

            var outputType = map.GetOutputType();
            if (null == outputType)
                return NotFound($"Cannot load output type :{map.OutputTypeName}");

            var json = File.ReadAllText(file);
            dynamic input = JsonConvert.DeserializeObject(json, inputType);
            dynamic mapping = Activator.CreateInstance(mapType);

            try
            {
                dynamic output = mapping.TransformAsync(input).Result;
                return Json(output);
            }
            catch (Exception e)
            {
                var entry = new LogEntry(e);
                return Error(entry);
            }
        }

        private static int Error(LogEntry entry)
        {
            Console.Error.WriteLine(entry.Details);
            File.WriteAllText($"{m_outputFolder}\\error.json", entry.Details);
            return -1;
        }

        private static int NotFound(string message)
        {
            Console.Error.WriteLine(message);
            File.WriteAllText($"{m_outputFolder}\\error.json", message, Encoding.ASCII);
            return -1;
        }

        private static int Json<T>(T result)
        {
            var ticks = (int)DateTime.Now.Ticks;
            File.WriteAllText($"{m_outputFolder}\\{ticks}.json", result.ToJson(), Encoding.ASCII);
            return ticks;
        }
    }
}
