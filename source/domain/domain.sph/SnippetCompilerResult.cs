using Microsoft.CodeAnalysis;

namespace Bespoke.Sph.Domain
{

    public class SnippetCompilerResult : DomainObject
    {
        private bool m_success;

        public bool Success
        {
            get { return m_success; }
            set
            {
                m_success = value;
                RaisePropertyChanged();
            }
        }

        private readonly ObjectCollection<Diagnostic> m_diagnosticCollection = new ObjectCollection<Diagnostic>();

        public ObjectCollection<Diagnostic> DiagnosticCollection
        {
            get { return m_diagnosticCollection; }
        }

        public string Code { get; set; }
    }
}