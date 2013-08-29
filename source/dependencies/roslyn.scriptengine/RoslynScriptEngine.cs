using System;
using System.Diagnostics;
using System.Text;
using Bespoke.SphCommercialSpaces.Domain;
using Roslyn.Scripting.CSharp;

namespace roslyn.scriptengine
{
    public class RoslynScriptEngine : IScriptEngine
    {

        public object Evaluate(string script, Entity item)
        {
            var host = new HostObject
            {
                Item = item
            };
            var scriptEngine = new ScriptEngine();
            var session = scriptEngine.CreateSession(host);

            session.AddReference("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            session.AddReference("System.Core, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            session.AddReference("mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            try
            {
                session.Execute("#r \".\\domain.commercialspace.dll\"");
                session.Execute("#r \".\\roslyn.scriptengine.dll\"");
            }
            catch (Exception e)
            {
                var message = new StringBuilder("Error adding reference to domain.commercialspace.dll");
                message.AppendLine("Script Engine base directory = " + scriptEngine.BaseDirectory);
                message.AppendLine("AppDomain base directory = " + AppDomain.CurrentDomain.BaseDirectory);
                throw new Exception(message.ToString(), e);
            }

            var customScript = string.Empty;
            var itemCustomScript = item as ICustomScript;
            if (null != itemCustomScript)
                customScript = itemCustomScript.Script;

            var block = script;
            if (!block.EndsWith(";"))
                block = string.Format("return {0};", script);
            var code = string.Format("using System;\r\n" +
                                     "using Bespoke.SphCommercialSpaces.Domain;\r\n" +
                                     "using System.Linq;\r\n" +
                                     "" +
                                     "public object Evaluate()\r\n" +
                                     "{{\r\n" +
                                     "var item = Item as {1};\r\n" +
                                     customScript + "\r\n" +
                                        "{0}\r\n" +
                                     "}}", block, item.GetType().Name);
            session.Execute(code);
            var result = session.Execute("Evaluate();");

            return result;

        }
    }
}
