using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web;
using Bespoke.Sph.Web.App_Start;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.ViewModels;
using Newtonsoft.Json;
using ConfigurationManager = Bespoke.Sph.Domain.ConfigurationManager;

namespace web.sph
{
    public class MvcApplication : HttpApplication
    {
        private ApplicationHelper m_webApplicationHelper;

        protected void Application_Start()
        {
            ConfigurationManager.AddConnectionString();
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AuthConfig.RegisterAuth();

            ModelBinders.Binders.Add(typeof(IEnumerable<Rule>), new RuleModelBinder());
            ObjectBuilder.GetObject<ILogger>().Log(new LogEntry
            {
                Message = "Web application starts",
                Severity = Severity.Critical,
                Log = EventLog.WebServer,
                Operation = "Starts",
                Source = "Application"

            });

        }

     

        protected void Application_Stop()
        {
            ObjectBuilder.GetObject<ILogger>().Log(new LogEntry
            {
                Message = "Web application stopped",
                Severity = Severity.Critical,
                Log = EventLog.WebServer,
                Operation = "Stop",
                Source = "Application"

            });
        }

        public ApplicationHelper WebApplicationHelper
        {
            get
            {
                return m_webApplicationHelper ?? (m_webApplicationHelper = new ApplicationHelper { Application = this });
            }
            set { m_webApplicationHelper = value; }
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Console.WriteLine("ERROR*-***/*/*/*/*");
            WebApplicationHelper.Application_Error();
            Console.WriteLine("ERROR*******************");
        }
        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {
            var headers = Request.Headers.GetValues("Authorization");
            var token = headers?.FirstOrDefault();
            if (token == null) return;

            try
            {
                var json = (new Encryptor()).Decrypt(token.Replace("Bearer ", ""));
                var st = json.DeserializeFromJson<SphSecurityToken>();
                if (st.Expired < DateTime.Now) return;

                var found = FindToken(st.Id);
                if (!found) return;

                var roles = st.Roles;
                IIdentity id = new GenericIdentity(st.Username);
                IPrincipal principal = new GenericPrincipal(id, roles);
                Context.User = principal;
            }
            catch (CryptographicException)
            {
            }
            catch (JsonReaderException)
            {
            }
        }

        private bool FindToken(string id)
        {
            var url = string.Format("{0}_sys/setting/{1}",
                ConfigurationManager.ApplicationName.ToLowerInvariant(),
                id);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);

                var response = client.GetAsync(url).Result;
                return response.StatusCode == HttpStatusCode.OK;

            }
        }


    }

}
