using System.Linq;

namespace Bespoke.Sph.Domain
{
    public partial class EntityForm : Entity
    {
        public  BuildValidationResult ValidateBuild(EntityDefinition ed)
        {
            var errors = from f in this.FormDesign.FormElementCollection
                         where f.IsPathIsRequired
                             && string.IsNullOrWhiteSpace(f.Path) && (f.Name != "HTML Section")

                         select new BuildError
                         (
                             this.WebId,
                             string.Format("[Input] : {0} => '{1}' does not have path", this.Name, f.Label)
                         );
            var elements = from f in this.FormDesign.FormElementCollection
                           let err = f.ValidateBuild(ed)
                           where null != err
                           select err;

            var result = new BuildValidationResult();
            result.Errors.AddRange(errors);
            result.Errors.AddRange(elements.SelectMany(v => v));
            result.Result = result.Errors.Count == 0;
            
            return result;
        }
    }
}