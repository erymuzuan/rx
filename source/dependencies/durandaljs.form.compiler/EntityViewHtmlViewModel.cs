using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public class EntityViewHtmlViewModel
    {
        public EntityView View { get; set; }
        public EntityDefinition Definition { get; set; }
        public string FilterDsl { get; set; }
        public string SortDsl { get; set; }
        public string Routes { get; set; }
        public string PartialArg { get; set; }
        public string PartialPath { get; set; }
    }
}