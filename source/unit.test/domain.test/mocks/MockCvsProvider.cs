using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace domain.test.mocks
{
    public class MockCvsProvider : ICvsProvider
    {
        private readonly CommitLog[] m_logs;

        public MockCvsProvider(CommitLog[] logs)
        {
            m_logs = logs;
        }
        public Task<string> GetCommitIdAsync(string file)
        {
            var log = m_logs.OrderByDescending(x => x.DateTime).FirstOrDefault(x => x.Files.Contains(file));
            return Task.FromResult(log?.CommitId);
        }

        public Task<string> GetCommitCommentAsync(string file)
        {
            var log = m_logs.OrderByDescending(x => x.DateTime).FirstOrDefault(x => x.Files.Contains(file));
            return Task.FromResult(log?.Comment);
        }

        public Task<LoadOperation<CommitLog>> GetCommitLogsAsync(string file, int page, int size)
        {
            var lo = new LoadOperation<CommitLog>();
            lo.ItemCollection.AddRange(m_logs);
            lo.TotalRows = m_logs.Length;

            return Task.FromResult(lo);
        }
    }
}