using System;
using System.Threading.Tasks;
using Bespoke.SphCommercialSpaces.Domain;
using RazorEngine;

namespace razor.template
{
    public class TemplateEngine : ITemplateEngine
    {
        public Task<string> GenerateAsync(string template, dynamic model)
        {
            if (string.IsNullOrWhiteSpace(template)) throw new ArgumentNullException("template", "whoaaaaa");
            var body = Razor.Parse(template, model);
            return Task.FromResult(body);
        }
    }
}
