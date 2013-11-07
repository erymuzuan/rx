namespace Bespoke.Sph.Domain
{
    public class WorkflowCompilerResult
    {
        private readonly ObjectCollection<CompilerMessage> m_errorsCollection = new ObjectCollection<CompilerMessage>();
        public bool Result { get; set; }
        public ObjectCollection<CompilerMessage> Errors

        {
            get { return m_errorsCollection; }
        }

        public string Output { get; set; }
    }
}