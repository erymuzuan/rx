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
    public class AjaxRequestTextFixture
    {
        public string Schema { get; set; }
        public HttpAdapter Adapter { get; set; }

        public AjaxRequestTextFixture()
        {

            this.Schema = "UnitTest";
            this.Adapter = new HttpAdapter
            {
                Name = "__BphTempahanRpp",
                Schema = this.Schema,
                Har = @".\bph.tempahan.rpp.20140726.har",
                Tables = new AdapterTable[] { },
                BaseAddress = "http://peranginan.bph.gov.my/",
                Timeout = 5000,
                TimeoutInterval = "Milliseconds"
            };
        }

        private async Task OpenAsync()
        {
            await this.Adapter.OpenAsync();
            foreach (var op in this.Adapter.OperationDefinitionCollection.OfType<HttpOperationDefinition>())
            {
                op.Name = op.Url.Replace(this.Adapter.BaseAddress, "")
                    .Replace("%20", "_")
                    .Replace(" ", "_")
                    .Replace("/", "_")
                    .Replace("?", "_")
                    .Replace("&", "_")
                    .Replace("=", "_")
                    .Replace(".", "_") + "_";
                Console.WriteLine("METHOD => " + op.Name);
                op.IsLoginRequired = false;
            }

        }
        private async Task<string> CompileAsync()
        {
            if (this.Adapter.OperationDefinitionCollection.Count == 0)
                await this.OpenAsync();
            var result = await this.Adapter.CompileAsync();
            Assert.IsTrue(File.Exists(result.Output));
            return result.Output;
        }


        [TestMethod]
        public async Task GetPreviousBooking()
        {
            var dll = Assembly.LoadFile(await CompileAsync());
            var type = dll.GetType(string.Format("Dev.Adapters.{0}.{1}.{1}", Adapter.Schema, Adapter.Name));
            var requestType = dll.GetType("Dev.Adapters.UnitTest." + Adapter.Name + ".PostBookingGetPreviousBookingRequest");
            dynamic peranginan = Activator.CreateInstance(type);

            dynamic request = Activator.CreateInstance(requestType);
            request.id = "780909-09-0909";

            var response = await peranginan.PostBookingGetPreviousBookingAsync(request);
            Assert.AreEqual("F32", response.Grade);
        }

        [TestMethod]
        public async Task PostWithArrayResponse()
        {
            var dll = Assembly.LoadFile(await CompileAsync());
            var type = dll.GetType(string.Format("Dev.Adapters.{0}.{1}.{1}", Adapter.Schema, Adapter.Name));
            var requestType = dll.GetType("Dev.Adapters.UnitTest." + Adapter.Name + ".PostBookingGetServiceTypeJsonRequest");
            dynamic peranginan = Activator.CreateInstance(type);

            dynamic request = Activator.CreateInstance(requestType);
            request.serviceType = "Kakitangan Awam Persekutuan";


            var response = await peranginan.PostBookingGetServiceTypeJsonAsync(request);
            Assert.AreEqual(5, response.list.Count);
        }

        [TestMethod]
        public async Task GetQueryStringRouteResponse()
        {
            var dll = Assembly.LoadFile(await CompileAsync());
            var type = dll.GetType(string.Format("Dev.Adapters.{0}.{1}.{1}", Adapter.Schema, Adapter.Name));
            var requestType = dll.GetType("Dev.Adapters.UnitTest." + Adapter.Name + ".GetBookingSelectRoomRequest");
            dynamic peranginan = Activator.CreateInstance(type);

            dynamic request = Activator.CreateInstance(requestType);
            request.id = "3";
            request.bph_secure_hash = "7116B1BF9AEE53748E3925FB2C75AE42";


            var response = await peranginan.GetBookingSelectRoomAsync(request);
            StringAssert.Contains(response.ResponseText, "Permohonan Rumah Peranginan Online");
        }


        [TestMethod]
        public async Task GetMethodWithoutRoute()
        {
            var adapter = this.Adapter;
            await adapter.OpenAsync();
            foreach (var op in adapter.OperationDefinitionCollection.OfType<HttpOperationDefinition>())
            {
                Console.WriteLine("{0}=>{1}", op.HttpMethod, op.Url);
            }
            var getBooking =
                adapter.OperationDefinitionCollection.OfType<HttpOperationDefinition>()
                    .Single(x => x.HttpMethod == "GET"
                                 && x.Url == "/booking");
            Assert.IsFalse(getBooking.Url.Contains("?"));
            Assert.IsTrue(string.IsNullOrWhiteSpace(getBooking.RequestRouting));
            Assert.AreEqual(0, getBooking.RequestMemberCollection.Count);
        }
        [TestMethod]
        public async Task PostMethodWithoutRoute()
        {
            var adapter = this.Adapter;
            await adapter.OpenAsync();
            foreach (var op in adapter.OperationDefinitionCollection.OfType<HttpOperationDefinition>())
            {
                Console.WriteLine("{0}=>{1}", op.HttpMethod, op.Url);
            }
            var getBooking =
                adapter.OperationDefinitionCollection.OfType<HttpOperationDefinition>()
                    .Single(x => x.HttpMethod == "POST"
                                 && x.Url == "/Booking/GetPreviousBooking");
            Assert.IsFalse(getBooking.Url.Contains("?"));
            Assert.IsTrue(string.IsNullOrWhiteSpace(getBooking.RequestRouting));
            Assert.AreEqual(1, getBooking.RequestMemberCollection.Count);
        }

        [TestMethod]
        public async Task GetMethodQueryStringRoute()
        {
            var adapter = this.Adapter;
            await adapter.OpenAsync();
            foreach (var op in adapter.OperationDefinitionCollection.OfType<HttpOperationDefinition>())
            {
                Console.WriteLine("{0}=>{1}", op.HttpMethod, op.Url);
            }
            var getBooking =
                adapter.OperationDefinitionCollection.OfType<HttpOperationDefinition>()
                    .Single(x => x.HttpMethod == "GET"
                                 && x.Url == "/Booking/SelectRoom/");
            Assert.IsFalse(getBooking.Url.Contains("?"));
            Assert.IsFalse(string.IsNullOrWhiteSpace(getBooking.RequestRouting));
            Assert.AreEqual(2, getBooking.RequestMemberCollection.Count);
        }
    }
}
