namespace Bespoke.Sph.Domain
{
    public partial class Page : Entity
    {
        public void ChangeWorkflowDefinitionVersion(int oldId, int id)
        {
            var page = this;
            this.PageId = 0;
            this.Code = page.Code.Replace("Workflows_" + oldId, "Workflows_" + id);
            this.Tag = page.Tag.Replace("wf_" + oldId, "wf_" + id);
            this.VirtualPath = page.VirtualPath.Replace("Workflow_" + oldId, "Workflow_" + id);
        }
    }
}