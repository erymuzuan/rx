namespace Bespoke.Sph.Domain
{
    public interface IQueryParser
    {
        QueryDsl Parse(string text);
        string Provider { get; }
        string Version { get; }
    }
}