using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http.Routing;

namespace Bespoke.Sph.WebApi
{
    public class GetRouteAttribute : MethodConstraintedRouteAttribute
    {
        public GetRouteAttribute(string template) : base(template, HttpMethod.Get) { }
    }
    public class PostRouteAttribute : MethodConstraintedRouteAttribute
    {
        public PostRouteAttribute(string template) : base(template, HttpMethod.Post) { }
    }
    public class PutRouteAttribute : MethodConstraintedRouteAttribute
    {
        public PutRouteAttribute(string template) : base(template, HttpMethod.Put) { }
    }
    public class PatchRouteAttribute : MethodConstraintedRouteAttribute
    {
        public PatchRouteAttribute(string template) : base(template, new HttpMethod("PATCH")) { }
    }
    public class DeleteRouteAttribute : MethodConstraintedRouteAttribute
    {
        public DeleteRouteAttribute(string template) : base(template, HttpMethod.Delete) { }
    }

    //This class allows adding constraints to the route generated
    public class MethodConstraintedRouteAttribute : RouteFactoryAttribute
    {
        public MethodConstraintedRouteAttribute(string template, HttpMethod method)
            : base(template)
        {
            Method = method;
        }

        public HttpMethod Method
        {
            get;
            private set;
        }

        public override IDictionary<string, object> Constraints
        {
            get
            {
                var constraints = new HttpRouteValueDictionary { { "method", new MethodConstraint(Method) } };
                return constraints;
            }
        }
    }

    public class MethodConstraint : IHttpRouteConstraint
    {
        public HttpMethod Method { get; }

        public MethodConstraint(HttpMethod method)
        {
            Method = method;
        }

        public bool Match(HttpRequestMessage request,
                          IHttpRoute route,
                          string parameterName,
                          IDictionary<string, object> values,
                          HttpRouteDirection routeDirection)
        {
            return request.Method == Method;
        }
    }

}
