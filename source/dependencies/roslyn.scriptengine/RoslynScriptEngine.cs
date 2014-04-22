using System;
using System.IO;
using System.Text;
using Bespoke.Sph.Domain;
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
                var domain = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof (Entity).Assembly.Location);
                var dll = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, typeof(RoslynScriptEngine).Assembly.Location);
                var argDll = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, arg1.GetType().Assembly.Location);

                session.Execute("#r \"" + domain + "\"");
                session.Execute("#r \"" + dll + "\"");
                session.Execute("#r \"" + argDll + "\"");
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
            if (!block.EndsWith(";"))
                block = string.Format("return {0};", script);
            var code = string.Format("using System;\r\n" +
                                     "using Bespoke.Sph.Domain;\r\n" +
                                     "using System.Linq;\r\n" +
                                     "" +
                                     "public {2} Evaluate()\r\n" +
                                     "{{\r\n" +
                                     "var item = Item as {1};\r\n" +
                                     customScript + "\r\n" +
                                        "{0}\r\n" +
                                     "}}", block, arg1.GetType().FullName, typeof(T).FullName);
            try
            {
                session.Execute(code);
            }
            catch (CompilationErrorException e)
            {
                throw new Exception("Error compiling this code : \r\n" + block + "\r\n The full code is \r\n" + code,e);
            }
            catch (Exception e)
            {
                throw new Exception("Error compiling this code : \r\n" + block + "\r\n The full code is \r\n" + code,e);
            }
            var result = session.Execute("Evaluate();");

            return (T)result;

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
