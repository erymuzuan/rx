using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WebJavascriptUtils
{
    [Export("CodeLanguageProvider", typeof(ICodeLanguageProvider))]
    public class JavascriptLanguageProvider : ICodeLanguageProvider
    {
        public string Name => "Javascript-knockout";
        public string Language => "javascript";
        public string Version => "1.0";

        public async Task<Dictionary<string, string>> GenerateCodeAsync(EntityDefinition ed)
        {
            var code = await ed.GenerateCustomXsdJavascriptClassAsync();
            return new Dictionary<string, string>
            {
                {$"{ed.Id}.js", code}
            };
        }
    }
}