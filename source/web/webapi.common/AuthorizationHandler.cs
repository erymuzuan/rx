using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JWT;

namespace Bespoke.Sph.WebApi
{
    public class AuthHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                    CancellationToken cancellationToken)
        {
            HttpResponseMessage errorResponse = null;

            try
            {
                IEnumerable<string> authHeaderValues;
                request.Headers.TryGetValues("Authorization", out authHeaderValues);


                if (authHeaderValues == null)
                    return base.SendAsync(request, cancellationToken); // cross fingers

                var bearerToken = authHeaderValues.ElementAt(0);
                var token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;

                //var secret = ConfigurationManager.AppSettings.Get("jwtKey");
                //var secret = "secretKey";

                //Thread.CurrentPrincipal = ValidateToken(
                //    token,
                //    secret,
                //    true
                //    );

                //if (HttpContext.Current != null)
                //{
                //    HttpContext.Current.User = Thread.CurrentPrincipal;
                //}
            }
            catch (SignatureVerificationException ex)
            {
                errorResponse = request.CreateErrorResponse(HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (Exception ex)
            {
                errorResponse = request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }


            return errorResponse != null
                ? Task.FromResult(errorResponse)
                : base.SendAsync(request, cancellationToken);
        }

    }
}
