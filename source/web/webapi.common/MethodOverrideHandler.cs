﻿using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bespoke.Sph.WebApi
{
    public class MethodOverrideHandler : DelegatingHandler
    {
        private readonly string[] m_methods = { "DELETE", "HEAD", "PUT", "PATCH" };
        private const string Header = "X-HTTP-Method-Override";

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (request.Method == HttpMethod.Post && request.Headers.Contains(Header))
            {
                var method = request.Headers.GetValues(Header).First();
                if (m_methods.Contains(method, StringComparer.InvariantCultureIgnoreCase))
                {
                    // Change the request method.
                    request.Method = new HttpMethod(method);
                }
            }
            return base.SendAsync(request, cancellationToken);
        }
    }
}
