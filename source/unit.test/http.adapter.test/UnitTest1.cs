using System;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Integrations.Adapters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace http.adapter.test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task TestMethod1()
        {
            var adapter = new HttpAdapter
            {
                Name = "httptest",
                Har = @".\localhost.har",
                Tables = new AdapterTable[]{}
            };

            var result = await adapter.CompileAsync();
            Assert.IsTrue(File.Exists(result.Output));
        }
    }
}
