﻿using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using RazorEngine;
using RazorEngine.Templating;

namespace Bespoke.Sph.Templating
{
    public class RazorEngine : ITemplateEngine
    {
        public Task<string> GenerateAsync(string template, dynamic model)
        {
            var directory = ObjectBuilder.GetObject<IDirectoryService>();
            dynamic viewBag = new DynamicViewBag();
            viewBag.BaseUrl = ConfigurationManager.BaseUrl;
            viewBag.ApplicationName = ConfigurationManager.ApplicationName;
            viewBag.ApplicationFullName = ConfigurationManager.ApplicationFullName;
            viewBag.UserName = directory.CurrentUserName;

            if (string.IsNullOrWhiteSpace(template)) return Task.FromResult(string.Empty);
            var body = Razor.Parse(template, model, viewBag, null);
            return Task.FromResult(body);
        }
    }


}
