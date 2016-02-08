namespace Bespoke.Sph.Domain
{
    public interface IOdataPagingProvider
    {
        string Tranlate(string sql, int page, int size);
        string SkipTop(string sql, int skip, int top);
    }
}