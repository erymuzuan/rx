using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebApi
{
    public class GetOneRouteAttribute : RouteFactoryAttribute
    {
        public GetOneRouteAttribute(string template) : base(template) { }
        public override IDictionary<string, object> Constraints
        {
            get
            {
                var constraints = new HttpRouteValueDictionary
                {
                    {"getone", new GetOneConstraint()}
                };
                return constraints;
            }
        }
    }
    public class GetOneConstraint : IHttpRouteConstraint
    {
        private static string[] m_queryRoute;
        public GetOneConstraint()
        {
            if (null == m_queryRoute)
            {
                var cm = ObjectBuilder.GetObject<ICacheManager>();
                const string KEY = "all-query-endpoints-route";
                var routes = cm.Get<string[]>(KEY);
                if (null == routes)
                {
                    var context = new SphDataContext();
                    var queries = context.LoadFromSources<QueryEndpoint>();
                    routes = queries.Select(x => x.Route.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries).LastOrDefault())
                        .Where(x => !string.IsNullOrWhiteSpace(x))
                        .Select(x => x.ToLowerInvariant())
                        .ToArray();

                    cm.Insert(KEY, routes);

                }

                m_queryRoute = routes;
            }
        }

        public bool Match(HttpRequestMessage request,
                          IHttpRoute route,
                          string parameterName,
                          IDictionary<string, object> values,
                          HttpRouteDirection routeDirection)
        {
            if (!values.ContainsKey("id")) return false;
            var id = values["id"] as string;
            if (null == id) return false;
            if (m_queryRoute.Contains(id.ToLowerInvariant())) return false;

            return request.Method == HttpMethod.Get;
        }
    }
}