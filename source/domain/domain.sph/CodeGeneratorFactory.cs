using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface ICodeLanguageProvider
    {
        string Name { get; }
        string Language { get; }
        string Version { get; }

        // Task<Dictionary<string, string>> GenerateCodeAsync(ICompilationUnit cu);
        // TODO : should make EntityDefinition and all assets implements ICompilationUnit
        Task<Dictionary<string, string>> GenerateCodeAsync(EntityDefinition ed);
    }

    public class CodeGeneratorFactory
    {
        private static CodeGeneratorFactory m_instance;
        private static readonly object m_lock = new object();

        private CodeGeneratorFactory()
        {
        }

        public static CodeGeneratorFactory Instance
        {
            get
            {
                lock (m_lock)
                {
                    if (null == m_instance)
                    {
                        m_instance = new CodeGeneratorFactory();
                        ObjectBuilder.ComposeMefCatalog(m_instance);
                    }
                    if (null == m_instance.LanguageProviders || m_instance.LanguageProviders.Length == 0)
                        ObjectBuilder.ComposeMefCatalog(m_instance);
                    if (null == m_instance.LanguageProviders || m_instance.LanguageProviders.Length == 0)
                        throw new Exception("Fail to load any CodeGenerator");
                    return m_instance;
                }
            }
        }

        [ImportMany("CodeLanguageProvider", typeof(ICodeLanguageProvider), AllowRecomposition = true)]
        public ICodeLanguageProvider[] LanguageProviders { get; set; }
    }
}