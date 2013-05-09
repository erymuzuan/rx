using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bespoke.Sph.Windows.Helpers
{
    internal static class WebRequestHelper
    {


        public static HttpWebRequest CreateHttpWebRequest(string url, object postData = null, string method = "POST", bool json = true)
        {
            // call the existing ajax
            string fullurl = string.Format("{0}/{1}", ConfigurationManager.AppSettings["DataServiceUrl"], url);
            var request = (HttpWebRequest)WebRequest.Create(fullurl);
            request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            if (json)
            {
                request.Accept = "application/json, text/javascript, */*; q=0.01";
                request.ContentType = "application/json; charset=utf-8";
            }
           
            request.Headers.Add("Accept-Encoding: gzip, deflate");// = "gzip, deflate";
            request.Method = method;
            if (method == "POST" && null != postData && json)
            {
                request.SetPostData(JsonConvert.SerializeObject(postData));
            }

            if (method == "POST" && null != (postData as string) && !json)
            {
                request.SetPostData(postData as string);
            }

            return request;
        }

        public static void SetPostData(this WebRequest request, string data)
        {
            var postBuffer = Encoding.GetEncoding(1252).GetBytes(data);
            request.ContentLength = postBuffer.Length;

            var postData = request.GetRequestStream();
            postData.Write(postBuffer, 0, postBuffer.Length);
            postData.Close();
        }

        public async static Task<WebRequest> SetPostDataAsync(this WebRequest request, string data)
        {
            var postBuffer = Encoding.GetEncoding(1252).GetBytes(data);
            request.ContentLength = postBuffer.Length;

            var postData = await request.GetRequestStreamAsync();
            postData.Write(postBuffer, 0, postBuffer.Length);
            postData.Close();
            return request;
        }

        public async static Task<WebRequest> SetPostJsonDataAsync(this WebRequest request, object data)
        {
            var json = JsonConvert.SerializeObject(data);
            var postBuffer = Encoding.GetEncoding(1252).GetBytes(json);
            request.ContentLength = postBuffer.Length;

            var postData = await request.GetRequestStreamAsync();
            postData.Write(postBuffer, 0, postBuffer.Length);
            postData.Close();
            return request;
        }


        public static HttpWebRequest SetProperties(this HttpWebRequest request, string method, CookieContainer container)
        {
            //
            request.KeepAlive = true;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/4.0; SLCC2; .NET CLR 2.0.50727; .NET CLR 3.5.30729; .NET CLR 3.0.30729; Media Center PC 6.0; InfoPath.2; .NET4.0C; .NET4.0E)";
            request.ContentType = "application/x-www-form-urlencoded";
            request.Method = method;
            if (m_cookies.ContainsKey("saplb_*") && m_cookies.ContainsKey("JSESSIONID"))
                request.Headers.Add("Cookie",
                    string.Format("saplb_*={0}; JSESSIONID={1}", m_cookies["saplb_*"], m_cookies["JSESSIONID"]));
            else
                request.CookieContainer = container;

            return request;


        }


        private static Dictionary<string, string> m_cookies = new Dictionary<string, string>();
        public async static Task<string> DownloadContent(this WebRequest request, CookieContainer cookieContainer = null)
        {
            var response = (HttpWebResponse)(await request.GetResponseAsync());
            var responseStream = new StreamReader(response.GetResponseStream(), Encoding.GetEncoding(1252));
            var html = await responseStream.ReadToEndAsync();

            if (null != cookieContainer)
            {
                foreach (Cookie cookie in response.Cookies)
                {
                    Console.WriteLine(cookie);
                }
                m_cookies = response.Cookies.Cast<Cookie>().Select(c => c).ToDictionary(c => c.Name, c => c.Value);
                //cookieContainer.Add(response.Cookies);
            }
            response.Close();
            responseStream.Close();



            return html;
        }
    
    }
}
