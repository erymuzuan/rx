using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    public class DurandalJsElementCompiler<T> : FormElementCompiler<T> where T: FormElement
    {
        public override string GenerateDisplay(T element)
        {
            return string.Format(@"<span data-bind=""text:{0}""></span> ", element.Path);
        }
    }
}