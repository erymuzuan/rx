using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.ViewModels
{
    public class WorkflowDefinitionVisualViewModel 
    {
        private readonly ObjectCollection<Activity> m_toolboxElements = new ObjectCollection<Activity>();

        public ObjectCollection<Activity> ToolboxElements
        {
            get { return m_toolboxElements; }
        }
    }
}