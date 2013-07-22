using System;
using System.IO;
using System.Text;
using Bespoke.SphCommercialSpaces.Domain;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using Roslyn.Scripting.CSharp;

namespace roslyn.scriptengine
{
    public class RoslynScriptEngine : IScriptEngine
    {
        private ScriptEngine m_scriptEngine;

        public RoslynScriptEngine()
        {
            m_scriptEngine = new ScriptEngine();
            var domain = typeof(Entity).Assembly.FullName;
            Console.WriteLine(domain);
            m_scriptEngine.AddReference(MetadataReference.CreateAssemblyReference("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089"));
        }

        public object Evaluate(string script, Entity item)
        {
            m_scriptEngine = new ScriptEngine();
            var session = m_scriptEngine.CreateSession();
            session.AddReference("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            session.AddReference("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");

            var temp = Path.GetTempFileName() + ".xml";
            using (var stream = new FileStream(temp, FileMode.Create))
            {
                XmlSerializerService.ToXmlStream(stream, item);
            }

            var block = script;
            if (!block.EndsWith(";"))
                block = string.Format("return {0};", script);
            var code = string.Format("using System;" +
                                     "using Bespoke.SphCommercialSpaces.Domain;" +
                                     "" +
                                     "public object Evaluate()" +
                                     "{{" +
                                            "var xml = System.IO.File.ReadAllText(@\"{1}\");\r\n" +
                                            "var item = XmlSerializerService.DeserializeFromXml<{2}>(xml); \r\n" +

                                            "{0}\r\n" +
                                     "}}", block, temp, item.GetType().Name);
            session.Execute("#r \".\\domain.commercialspace.dll\"");
            session.Execute(code);


            var result = session.Execute("Evaluate();");
            Console.WriteLine("result :" + result);

            if (File.Exists(temp))
                File.Delete(temp);

            return result;

        }
    }
}
