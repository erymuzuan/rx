using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace domain.test.triggers
{
    internal class MockTemplateEnging : ITemplateEngine
    {
        public Task<string> GenerateAsync(string template, dynamic model)
        {
            if (string.IsNullOrWhiteSpace(template))
                return Task.FromResult(string.Empty);
            var result = template.Replace("@@", "@");
            return Task.FromResult("MOCK-TEMPLATE-" + result);
        }
    }
}