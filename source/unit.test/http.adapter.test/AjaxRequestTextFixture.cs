﻿using System;
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
                Name = "BphTempahanRpp",
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
            var type = dll.GetType(string.Format("Dev.Adapters.{0}.{1}", Adapter.Schema, Adapter.Name));
            var requestType = dll.GetType("Dev.Adapters.UnitTest.PostBookingGetpreviousbookingRequest");
            dynamic peranginan = Activator.CreateInstance(type);
            
            dynamic request = Activator.CreateInstance(requestType);
            request.id = "780909-09-0909";



            var response = await peranginan.PostBookingGetpreviousbookingAsync(request);
            Assert.AreEqual("F32", response.Grade);
        }

        [TestMethod]
        public async Task PostWithArrayResponse()
        {
            var dll = Assembly.LoadFile(await CompileAsync());
            var type = dll.GetType(string.Format("Dev.Adapters.{0}.{1}", Adapter.Schema, Adapter.Name));
            var requestType = dll.GetType("Dev.Adapters.UnitTest.PostBookingGetservicetypejsonRequest");
            dynamic peranginan = Activator.CreateInstance(type);
            
            dynamic request = Activator.CreateInstance(requestType);
            request.serviceType = "Kakitangan Awam Persekutuan";


            var response = await peranginan.PostBookingGetservicetypejsonAsync(request);
            Assert.AreEqual(5, response.list.Count);
        }
    }
}
