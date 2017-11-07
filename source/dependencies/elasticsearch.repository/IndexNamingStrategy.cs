namespace Bespoke.Sph.ElasticsearchRepository
{
    public enum IndexNamingStrategy
    {
        None,
        Hourly,
        Daily,
        YearAndWeek,
        YearAndMonth,
        Year
    }
}