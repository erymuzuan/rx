using System;
using System.IO;
using System.Text;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Roslyn.Compilers;
using Roslyn.Scripting.CSharp;

namespace Bespoke.Sph.RoslynScriptEngines
{
    public class RoslynScriptEngine : IScriptEngine
    {
        public T Evaluate<T, T1>(string script, T1 arg1)
        {
            var host = new HostObject<T1>
            {
                Item = arg1
            };
            var scriptEngine = new ScriptEngine();
            var session = scriptEngine.CreateSession(host);

            session.AddReference("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            session.AddReference("System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            session.AddReference("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            try
            {
                var domain = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(Entity).Assembly.Location);
                var dll = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(RoslynScriptEngine).Assembly.Location);
                var argDll = arg1.GetType().Assembly.Location;

                session.Execute("#r \"" + domain + "\"");
                session.Execute("#r \"" + dll + "\"");
                session.Execute($"#r \"{argDll}\"");
                session.Execute($"#r \"{typeof(JsonConvert).Assembly.Location}\"");
            }
            catch (Exception e)
            {
                var msg = new StringBuilder("Error adding reference to domain.sph.dll");
                msg.AppendLine();
                msg.AppendLine("Script Engine base directory = " + scriptEngine.BaseDirectory);
                msg.AppendLine("AppDomain base directory = " + AppDomain.CurrentDomain.BaseDirectory);
                msg.AppendLine("Actual exception :");
                msg.AppendLine(e.Message);
                throw new Exception(msg.ToString());
            }

            var customScript = string.Empty;
            var itemCustomScript = arg1 as ICustomScript;
            if (null != itemCustomScript)
                customScript = itemCustomScript.Script;

            var block = script;
            if (!block.EndsWith(";")) block = $"return {script};";


            var argTypeName = arg1.GetType().FullName;
            var code = $@"
using System;
using Bespoke.Sph.Domain;
using System.Linq;
public {typeof(T).ToCSharp()} Evaluate()
{{
    if(null == Item)throw new InvalidOperationException(""Item is null"");
    var item = Item as {argTypeName};
    if(null == item) 
    {{
        var json = item.ToJsonString();
        item = json.DeserializeFromJson<{argTypeName}>();
    }};
    if(null == item) throw new Exception(""Cannot cast Item("" + Item.GetType().FullName + "") to [{argTypeName}]"");
    {customScript}
    {block}
}}";
            try
            {
                session.Execute(code);
                var result = session.Execute("Evaluate();");

                return (T)result;
            }
            catch (CompilationErrorException e)
            {
                throw new Exception("Error compiling this code : \r\n" + block + "\r\n The full code is \r\n" + code, e);
            }
            catch (Exception e)
            {
                throw new Exception("Error compiling this code : \r\n" + block + "\r\n The full code is \r\n" + code, e);
            }
            finally
            {
                session = null;
                scriptEngine = null;
                host = null;
            }

        }


        public T Evaluate<T, T1, T2>(string script, T1 arg1, T2 arg2)
        {
            throw new NotImplementedException();
        }

        public T Evaluate<T, T1, T2, T3>(string script, T1 arg1, T2 arg2, T3 arg3)
        {
            throw new NotImplementedException();
        }
    }
}
