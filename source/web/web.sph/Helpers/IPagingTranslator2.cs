namespace Bespoke.Sph.WebSph.Helpers
{
    public interface IPagingTranslator2
    {
        string Tranlate(string sql, int page, int size);
    }
}