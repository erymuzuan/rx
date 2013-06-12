using System.Threading.Tasks;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Mocks
{
    public class TemplateEngine : ITemplateEngine
    {
        public Task<string> GenerateAsync(string template, dynamic model)
        {
            var no = string.Format("{0}\\{1}\\{2}", model.Contract.ReferenceNo,model.Type, model.MaxId);
            return Task.FromResult(no);
        }
    }
}