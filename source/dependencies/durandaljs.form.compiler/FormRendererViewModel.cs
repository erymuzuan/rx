using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public class FormRendererViewModel
    {
        public IForm Form { get; set; }
        public IProjectProvider Project { get; set; }
        public FormCompiler Compiler { get; set; }
    }
}