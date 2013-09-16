namespace Bespoke.Sph.Domain
{
   public interface IDocumentGenerator
   {
       void Generate(string output, params DomainObject[] data);
       void GenerateWithObject(string output, params DomainObject[] data);
    }
}
