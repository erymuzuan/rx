namespace Bespoke.Sph.Domain
{
    [PersistenceOption(IsSource = true, IsSqlDatabase = false, IsElasticsearch = false)]
    public partial class WorkersConfig : Entity { }
    [PersistenceOption(IsSource = true, IsSqlDatabase = false, IsElasticsearch = false)]
    public partial class WebServerConfig : Entity { }
    [PersistenceOption(IsSource = true, IsSqlDatabase = false, IsElasticsearch = false)]
    public partial class ElasticsearchConfig : Entity { }
    [PersistenceOption(IsSource = true, IsSqlDatabase = false, IsElasticsearch = false)]
    public partial class DscConfig : Entity { }
    [PersistenceOption(IsSource = true, IsSqlDatabase = false, IsElasticsearch = false)]
    public partial class BrokerConfig : Entity { }
}