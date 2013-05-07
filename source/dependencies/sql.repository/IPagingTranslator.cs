namespace Bespoke.Station.SqlRepository
{
    public interface IPagingTranslator
    {
        string Tranlate(string sql, int page, int size);
    }
}