namespace Bespoke.Sph.SqlRepository
{
    public interface IPagingTranslator
    {
        string Tranlate(string sql, int page, int size);
    }
}