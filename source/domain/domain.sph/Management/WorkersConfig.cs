namespace Bespoke.Sph.Domain
{
    [PersistenceOption(IsSource = true, IsSqlDatabase = false, IsElasticsearch = false)]
    public partial class WorkersConfig : Entity { }
}