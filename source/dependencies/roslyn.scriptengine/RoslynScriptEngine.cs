﻿using System;
using System.IO;
using System.Text;
using Bespoke.Sph.Domain;
using Roslyn.Compilers;
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
                var domain = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof (Entity).Assembly.Location);
                var dll = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(RoslynScriptEngine).Assembly.Location);
                Console.WriteLine("#r \"" + domain + "\"");
                session.Execute("#r \"" + domain + "\"");
                session.Execute("#r \"" + dll + "\"");
            }
            catch (Exception e)
            {
                var message = new StringBuilder("Error adding reference to domain.commercialspace.dll");
                message.AppendLine("Script Engine base directory = " + scriptEngine.BaseDirectory);
                message.AppendLine("AppDomain base directory = " + AppDomain.CurrentDomain.BaseDirectory);
                message.AppendLine("Actual exception :");
                message.AppendLine(e.Message);
                throw new Exception(message.ToString());
            }

            var customScript = string.Empty;
            var itemCustomScript = item as ICustomScript;
            if (null != itemCustomScript)
                customScript = itemCustomScript.Script;

            var block = script;
            if (!block.EndsWith(";"))
                block = string.Format("return {0};", script);
            var code = string.Format("using System;\r\n" +
                                     "using Bespoke.Sph.Domain;\r\n" +
                                     "using System.Linq;\r\n" +
                                     "" +
                                     "public object Evaluate()\r\n" +
                                     "{{\r\n" +
                                     "var item = Item as {1};\r\n" +
                                     customScript + "\r\n" +
                                        "{0}\r\n" +
                                     "}}", block, item.GetType().Name);
            try
            {
                session.Execute(code);
            }
            catch (CompilationErrorException e)
            {
                throw new Exception("Error compiling this code : \r\n" + block + "\r\n The full code is \r\n" + code,e);
            }
            var result = session.Execute("Evaluate();");

            return result;

        }
    }
}
