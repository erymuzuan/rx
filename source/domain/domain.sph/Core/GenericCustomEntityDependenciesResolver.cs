using System;
using System.Reflection;

namespace Bespoke.Sph.Domain
{
    public class GenericCustomEntityDependenciesResolver : ICustomEntityDependenciesResolver
    {
        public string RepositoryTypeName { get; set; }
        public string RepositoryAssemblyName { get; set; }
        public string ReadOnlyRepositoryTypeName { get; set; }
        public string ReadOnlyRepositoryAssemblyName { get; set; }
        

        public (Type DependencyType, dynamic Implementation) ResolveRepository(EntityDefinition ed)
        {
            var edType = GetEntityDefinitionType(ed);
            var implementedType = GetRepositoryImplementationType();

            var dependencyType = typeof(IRepository<>).MakeGenericType(edType);

            var implementedRepositoryType = implementedType.MakeGenericType(edType);
            var repository = Activator.CreateInstance(implementedRepositoryType);
            return (dependencyType, repository);
        }

        public (Type DependencyType, dynamic Implementation) ResolveReadOnlyRepository(EntityDefinition ed)
        {
            var edType = GetEntityDefinitionType(ed);
            var implementedType = GetReadOnlyRepositoryImplementationType();

            var dependencyType = typeof(IReadOnlyRepository<>).MakeGenericType(edType);

            var implementedReadOnlyType = implementedType.MakeGenericType(edType);
            var readOnly = Activator.CreateInstance(implementedReadOnlyType);
            return (dependencyType, readOnly);
        }

        private Type m_repositoyImplementationType;
        private Type m_readOnlyRepositoyImplementationType;

        private Type GetRepositoryImplementationType()
        {
            if (null != m_repositoyImplementationType) return m_repositoyImplementationType;
           
            var reposAssembly = Assembly.Load(this.RepositoryAssemblyName);
            m_repositoyImplementationType = reposAssembly.GetType(this.RepositoryTypeName);
            return m_repositoyImplementationType;
        }
        private Type GetReadOnlyRepositoryImplementationType()
        {
            if (null != m_readOnlyRepositoyImplementationType) return m_readOnlyRepositoyImplementationType;

            var readOnlyAssembly = Assembly.Load(this.ReadOnlyRepositoryAssemblyName);
            m_readOnlyRepositoyImplementationType = readOnlyAssembly.GetType(this.ReadOnlyRepositoryTypeName);
            return m_readOnlyRepositoyImplementationType;
        }

        private static Type GetEntityDefinitionType(EntityDefinition ed)
        {
            // TODO : get this entity defintion type from loaded assembly
            var edAssembly = Assembly.Load($"{ConfigurationManager.ApplicationName}.{ed.Name}");
            var edTypeName = $"{ed.CodeNamespace}.{ed.Name}";
            var edType = edAssembly.GetType(edTypeName);
            if (null == edType)
                Console.WriteLine("Cannot create type " + edTypeName);
            return edType;
        }
    }
}