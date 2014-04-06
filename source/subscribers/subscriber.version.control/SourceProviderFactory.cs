using System;
using Bespoke.Sph.Domain;

namespace subscriber.version.control
{
    public static class SourceProviderFactory
    {
        public static dynamic Create(Type type)
        {
            if (type == typeof (ReportDefinition))
                return new ReportDefinitionSourceProvider();

            if (type == typeof (DocumentTemplate))
                return new DocumentTemplateSourceProvider();

            if (type == typeof(WorkflowDefinition))
                return new WorkflowSourceProvider();

            if (type == typeof(EntityDefinition))
                return new EntityDefinitionSourceProvider();

            if (type == typeof(EntityView))
                return new EntityViewSourceProvider();

            if (type == typeof(EntityForm))
                return new EntityFormSourceProvider();

            return new EntitySourceProvider();
        }
    }
}