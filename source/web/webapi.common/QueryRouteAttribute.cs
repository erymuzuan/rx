using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace Bespoke.Sph.WebApi
{
    public class QueryRouteAttribute : RouteFactoryAttribute
    {
        public QueryRouteAttribute(string template) : base(template) { }
        public override IDictionary<string, object> Constraints
        {
            get
            {
                var constraints = new HttpRouteValueDictionary
                {
                    {"query", new QueryConstraint(true)}
                };
                return constraints;
            }
        }
    }
    public class QueryConstraint : IHttpRouteConstraint
    {
        private readonly bool m_query;

        public QueryConstraint(bool query)
        {
            m_query = query;
        }

        public bool Match(HttpRequestMessage request,
                          IHttpRoute route,
                          string parameterName,
                          IDictionary<string, object> values,
                          HttpRouteDirection routeDirection)
        {
            return request.Method == HttpMethod.Get && !route.RouteTemplate.Contains("{id}") && m_query;
        }
    }
}