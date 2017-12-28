namespace Bespoke.Sph.Domain
{
    public interface IQueryParser
    {
        QueryDsl Parse(string text, string entity);
        string Provider { get; }
        string ContentType { get; }
    }
}