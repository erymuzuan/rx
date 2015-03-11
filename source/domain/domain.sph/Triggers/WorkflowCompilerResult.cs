﻿namespace Bespoke.Sph.Domain
{
    public class WorkflowCompilerResult
    {
        private readonly ObjectCollection<BuildError> m_errorsCollection = new ObjectCollection<BuildError>();
        public bool Result { get; set; }
        public ObjectCollection<BuildError> Errors

        {
            get { return m_errorsCollection; }
        }

        public string Output { get; set; }
    }
}