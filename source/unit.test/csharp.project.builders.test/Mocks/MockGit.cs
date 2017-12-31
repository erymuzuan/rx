using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Tests.Mocks
{
    public class MockGit : ICvsProvider
    {
        public Task<string> GetCommitIdAsync(string file)
        {
            return Task.FromResult("abc");
        }

        public Task<string> GetCommitCommentAsync(string file)
        {
            throw new System.NotImplementedException();
        }

        public Task<LoadOperation<CommitLog>> GetCommitLogsAsync(string file, int page, int size)
        {
            throw new System.NotImplementedException();
        }
    }
}