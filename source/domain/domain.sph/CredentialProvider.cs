using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;

namespace Bespoke.Sph.Domain
{
    public static class CredentialProvider
    {
        private static Dictionary<string, string> m_cookies;
        public static WebRequest SetCredential(this WebRequest request)
        {
            if (null == m_cookies)
            {
                m_cookies = new Dictionary<string, string>();
                foreach (var c in HttpContext.Current.Request.Cookies.AllKeys)
                {
                    var val = HttpContext.Current.Request.Cookies[c];
                    if (!m_cookies.ContainsKey(c) && null != val)
                        m_cookies.Add(c, val.Value);
                }
            }
            var sb = new StringBuilder();
            foreach (var cookie in m_cookies.Keys)
            {
                sb.AppendFormat("{0}={1};", cookie, m_cookies[cookie]);
            }
            request.Headers.Add("Cookie", sb.ToString());
            return request;
        }


        public static void Initiliaze(Dictionary<string, string> cookies)
        {
            m_cookies = cookies;
        }
    }
}
