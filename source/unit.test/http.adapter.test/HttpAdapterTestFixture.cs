using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace http.adapter.test
{
    [TestClass]
    public class HttpAdapterTestFixture
    {
        [TestMethod]
        public async Task GetMethod()
        {
            var adapter = new HttpAdapter
            {
                Name = "HttpLocalhostTestAdapter",
                Schema = "UnitTest",
                Har = @".\localhost.har",
                Tables = new AdapterTable[] { }
            };
           
            await adapter.OpenAsync();
            foreach (var op in adapter.OperationDefinitionCollection.OfType<HttpOperationDefinition>())
            {
                op.Name = op.Url.Replace("https://eservice.mystation.com.my/myicss/", "")
                    .Replace(".", "_");
            }
            var result = await adapter.CompileAsync();
            Assert.IsTrue(File.Exists(result.Output));

            var dll = Assembly.LoadFile(result.Output);
            var type = dll.GetType(string.Format("Dev.Adapters.{0}.{1}", adapter.Schema, adapter.Name));
            var requestType = dll.GetType(string.Format("Dev.Adapters.{0}.GETlogin_doRequest", adapter.Schema));
            dynamic localhost = Activator.CreateInstance(type);
            dynamic request = Activator.CreateInstance(requestType);

            var response = await localhost.GETlogin_doAsync(request);
            StringAssert.Contains(response.ResponseText, "inUserName");
        }
    }
}
