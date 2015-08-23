using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Host;
using Roslyn.Utilities;

namespace domain.test.triggers
{
    internal class TestWorkspace : Workspace
    {
        // Forces serialization of mutation calls. Must take this lock before taking stateLock.
        private readonly NonReentrantLock m_serializationLock = new NonReentrantLock();

        public TestWorkspace(HostServices hostServices = null)
            : base(hostServices ?? new AdhocWorkspace().Services.HostServices, "Test")
        {
        }

        public void AddProject(ProjectId projectId, string projectName, string language = LanguageNames.CSharp)
        {
            using (this.m_serializationLock.DisposableWait())
            {
                var oldSolution = this.CurrentSolution;
                var newSolution = this.SetCurrentSolution(oldSolution.AddProject(projectId, projectName, projectName, language));

                this.RaiseWorkspaceChangedEventAsync(WorkspaceChangeKind.ProjectAdded, oldSolution, newSolution, projectId);
            }
        }

        public ProjectId AddProject(string projectName, string languageName = LanguageNames.CSharp)
        {
            ProjectId id = ProjectId.CreateNewId(debugName: projectName);
            this.AddProject(id, projectName, languageName);
            return id;
        }

        public T GetService<T>()
            where T : class, IWorkspaceService
        {
            return this.Services.GetService<T>();
        }
    }
}