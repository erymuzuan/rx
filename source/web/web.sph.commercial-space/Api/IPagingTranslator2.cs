namespace Bespoke.Sph.Commerspace.Web.Api
{
    public interface IPagingTranslator2
    {
        string Tranlate(string sql, int page, int size);
    }
}