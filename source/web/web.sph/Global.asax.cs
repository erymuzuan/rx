using System;
using System.Collections.Generic;
using System.Linq;
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

namespace web.sph
{
    public class MvcApplication : HttpApplication
    {
        private ApplicationHelper m_webApplicationHelper;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes).Wait();
            //BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            ModelBinders.Binders.Add(typeof(IEnumerable<Rule>), new RuleModelBinder());

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
        protected void Application_AuthenticateRequest(Object sender, EventArgs e)
        {
            var headers = Request.Headers.GetValues("Authorization");
            if (headers == null) return;
            var token = headers.FirstOrDefault();
            if (null == token) return;

            try
            {
                var json = (new Encryptor()).Decrypt(token.Replace("Bearer ", ""));
                var st = json.DeserializeFromJson<SphSecurityToken>();
                if (st.Expired < DateTime.Now) return;

                var context = new SphDataContext();
                var setting = context.LoadOne<Setting>(x => x.Id == st.Id);
                if (null == setting) return;

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


    }

}
