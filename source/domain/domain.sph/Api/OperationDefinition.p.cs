namespace Bespoke.Sph.Domain.Api
{
    public partial class OperationDefinition
    {
        public string Name { get; set; }
        public string MethodName { get; set; }
        public bool IsOneWay { get; set; }
        public bool IsEvent { get; set; }
        public ParameterDefinition RequestDefinition { get; set; }
        public ParameterDirection ResponseDefinition { get; set; }
        public string CodeNamespace { get; set; }
    }
}
