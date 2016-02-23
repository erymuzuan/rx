namespace Bespoke.Sph.Domain
{
    public partial class ValueObjectVariable : Variable
    {
        public override string GeneratedCode(WorkflowDefinition workflowDefinition)
        {
            var context = new SphDataContext();
            var vod = context.LoadOneFromSources<ValueObjectDefinition>(x =>x.Name == this.ValueObjectDefinition);

            return vod.ToString();
        }
    }
}