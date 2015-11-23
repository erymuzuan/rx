namespace Bespoke.Sph.Domain
{
    [StoreAsSource]
    public partial class Page : Entity
    {
        public void ChangeWorkflowDefinitionVersion(string oldId, string id)
        {
            var page = this;
            this.Id = string.Empty;
            this.Code = page.Code.Replace("Workflows_" + oldId, "Workflows_" + id);
            this.Tag = page.Tag.Replace("wf_" + oldId, "wf_" + id);
            this.VirtualPath = page.VirtualPath.Replace("Workflow_" + oldId, "Workflow_" + id);
        }
    }
}