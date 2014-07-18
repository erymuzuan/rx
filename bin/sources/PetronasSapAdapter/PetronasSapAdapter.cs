using Bespoke.Sph.Domain;
using System;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Net;
using System.Xml.Serialization;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;
using Dev.Adapters.Sap;

namespace Dev.Adapters.Sap
{
   public class PetronasSapAdapter
   {
       private CookieContainer m_cookieContainer = new CookieContainer();
       const string BASE_ADDRESS = "https://eservice.mystation.com.my/";
       public async Task<GetLoginDoResponse> GetLoginDoAsync(GetLoginDoRequest request)
       {
           const string url = "myicss/login.do";
           using (var handler = new HttpClientHandler { CookieContainer = m_cookieContainer })
           using(var client = new HttpClient(handler){BaseAddress = new Uri(BASE_ADDRESS)})
           {
               var response = await client.GetAsync(url);
               var result =  new GetLoginDoResponse();
               await result.LoadAsync(response);
               return result;

           }
       }
       public async Task<PostLoginDoResponse> PostLoginDoAsync(PostLoginDoRequest request)
       {
           const string url = "myicss/login.do";
           using (var handler = new HttpClientHandler { CookieContainer = m_cookieContainer })
           using(var client = new HttpClient(handler){BaseAddress = new Uri(BASE_ADDRESS)})
           {
               var requestMessage = new  HttpRequestMessage(HttpMethod.Post,url);
               requestMessage.Content = new StringContent(request.PostData, Encoding.UTF8);
               requestMessage.Content.Headers.Remove("Content-Type");
               requestMessage.Content.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
               var response = await client.SendAsync(requestMessage);
               
               
               var result =  new PostLoginDoResponse();
               await result.LoadAsync(response);
               return result;

           }
       }
       public async Task<GetDisorderitemDoResponse> GetDisorderitemDoAsync(GetDisorderitemDoRequest request)
       {
           const string url = "myicss/disorderitem.do";
           using (var handler = new HttpClientHandler { CookieContainer = m_cookieContainer })
           using(var client = new HttpClient(handler){BaseAddress = new Uri(BASE_ADDRESS)})
           {
               var response = await client.GetAsync(url);
               var result =  new GetDisorderitemDoResponse();
               await result.LoadAsync(response);
               return result;

           }
       }
       public async Task<PostDisorderitemDoResponse> PostDisorderitemDoAsync(PostDisorderitemDoRequest request)
       {
           const string url = "myicss/disorderitem.do";
           using (var handler = new HttpClientHandler { CookieContainer = m_cookieContainer })
           using(var client = new HttpClient(handler){BaseAddress = new Uri(BASE_ADDRESS)})
           {
               var requestMessage = new  HttpRequestMessage(HttpMethod.Post,url);
               requestMessage.Content = new StringContent(request.PostData, Encoding.UTF8);
               requestMessage.Content.Headers.Remove("Content-Type");
               requestMessage.Content.Headers.TryAddWithoutValidation("Content-Type", "application/x-www-form-urlencoded");
               var response = await client.SendAsync(requestMessage);
               
               
               var result =  new PostDisorderitemDoResponse();
               await result.LoadAsync(response);
               return result;

           }
       }
   }
}
