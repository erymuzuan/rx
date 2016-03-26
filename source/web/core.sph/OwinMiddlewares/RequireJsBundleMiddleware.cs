using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Microsoft.Owin;

namespace Bespoke.Sph.Web.Controllers
{
    public class RequireJsBundleMiddleware
    {

        private readonly Func<IDictionary<string, object>, Task> m_next;
        private readonly bool m_debug;

        public RequireJsBundleMiddleware(Func<IDictionary<string, object>, Task> next, bool debug)
        {
            m_next = next;
            m_debug = debug;
        }

        public async Task Invoke(IDictionary<string, object> environment)
        {
            var js = new StringBuilder();
            TextJs(js);
            GetDurandalCore(js);
            MainJs(js);
            DurandalPlugings(js);
            DurandalTransitions(js);
            RxShell(js);
            RxServices(js);

            var ctx = new OwinContext(environment);
            ctx.Response.ContentType = "application/javascript";
            await ctx.Response.WriteAsync(js.ToString());
        }

        private static void RxShell(StringBuilder js)
        {
            js.AppendLine(GetScript("SphApp.viewmodels.messages.js")
                .Replace("define([", "define('viewmodels/messages', ["));

            js.AppendLine(GetScript("SphApp.viewmodels.search.js")
                .Replace("define([", "define('viewmodels/search', ["));

            js.AppendLine(GetScript("SphApp.viewmodels.shell.js")
                .Replace("define([", "define('viewmodels/shell', ["));
        }

        private void RxServices(StringBuilder js)
        {
            foreach (var file in Directory.GetFiles(($"{ConfigurationManager.WebPath}/SphApp/services"), "cultures.*"))
            {
                js.AppendLine(File.ReadAllText(file)
                    .Replace("define([",$"define('services/{Path.GetFileNameWithoutExtension(file)}', ["));
            }
            

            js.AppendLine(GetScript("SphApp.services.logger.js")
                .Replace("define([", "define('services/logger', ["));

            js.AppendLine(GetScript("SphApp.services.datacontext.js")
                .Replace("define([", "define('services/datacontext', ["));

            js.AppendLine(GetScript("SphApp.services.jsonimportexport.js")
                .Replace("define([", "define('services/jsonimportexport', ["));

            js.AppendLine(GetScript("SphApp.services.validation.js")
                .Replace("define([", "define('services/validation', ["));

            js.AppendLine(GetScript("SphApp.services.watcher.js")
                .Replace("define([", "define('services/watcher', ["));

            js.AppendLine(GetScript("SphApp.services.system.js")
                .Replace("define([", "define('services/system', ["));
        }

        private static void DurandalTransitions(StringBuilder js)
        {
            js.AppendLine(GetScript("Scripts.durandal.transitions.entrance.js")
                .Replace("define([", "define('transitions/entrance', ["));
        }

        private static void TextJs(StringBuilder js)
        {
            js.AppendLine(GetScript("Scripts.text.js")
                .Replace("define([", "define('text', ["));
        }
        private static void DurandalPlugings(StringBuilder js)
        {
            js.AppendLine(GetScript("Scripts.durandal.plugins.router.js")
                .Replace("define([", "define('plugins/router', ["));

            js.AppendLine(GetScript("Scripts.durandal.plugins.dialog.js")
                .Replace("define([", "define('plugins/dialog', ["));

            js.AppendLine(GetScript("Scripts.durandal.plugins.history.js")
                .Replace("define([", "define('plugins/history', ["));

            js.AppendLine(GetScript("Scripts.durandal.plugins.widget.js")
                .Replace("define([", "define('plugins/widget', ["));

            js.AppendLine(GetScript("Scripts.durandal.plugins.serializer.js")
                .Replace("define([", "define('plugins/serializer', ["));

            js.AppendLine(GetScript("Scripts.durandal.plugins.observable.js")
                .Replace("define([", "define('plugins/observable', ["));

            js.AppendLine(GetScript("Scripts.durandal.plugins.http.js")
                .Replace("define([", "define('plugins/http', ["));
        }

        private void MainJs(StringBuilder js)
        {
            var main = GetScript("SphApp.main.js")
                .Replace("define([", "define('main', [");
            js.AppendLine(main);
        }

        private static void GetDurandalCore(StringBuilder js)
        {
            var durandalComposition = GetScript("Scripts.durandal.composition.js")
                .Replace("define([", "define('durandal/composition', [");
            js.AppendLine(durandalComposition);

            var durandalEvents = GetScript("Scripts.durandal.events.js")
                .Replace("define([", "define('durandal/events', [");
            js.AppendLine(durandalEvents);

            var durandalviewEngine = GetScript("Scripts.durandal.viewEngine.js")
                .Replace("define([", "define('durandal/viewEngine', [");
            js.AppendLine(durandalviewEngine);

            js.AppendLine(GetScript("Scripts.durandal.binder.js")
                .Replace("define([", "define('durandal/binder', ["));

            js.AppendLine(GetScript("Scripts.durandal.activator.js")
                .Replace("define([", "define('durandal/activator', ["));


            var durandalViewLocator = GetScript("Scripts.durandal.viewLocator.js")
                .Replace("define([", "define('durandal/viewLocator', [");
            js.AppendLine(durandalViewLocator);

            var durandalApp = GetScript("Scripts.durandal.app.js")
                .Replace("define([", "define(\"durandal/app\", [");
            js.AppendLine(durandalApp);

            var durandalSystem = GetScript("Scripts.durandal.system.js")
                .Replace("define([", "define('durandal/system', [");
            js.AppendLine(durandalSystem);


            var ko = GetScript("Scripts.knockout-3.4.0.js")
                .Replace("define([", "define('durandal/knockout', [");
            js.AppendLine(ko);

        }

        private static string GetScript(string key)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("Bespoke.Sph.Web." + key);
            if (null != stream)
            {
                stream.Position = 0;
                using (var reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }

            return null;

        }
    }
}