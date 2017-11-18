using System;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public interface ICvsProvider
    {
        Task<string> GetCommitIdAsync(string file);
        Task<string> GetCommitCommentAsync(string file);
        Task<LoadOperation<CommitLog>> GetCommitLogsAsync(string file, int page, int size);
    }

    public class CommitLog : DomainObject
    {
        public string CommitId { get; set; }
        public string Comment { get; set; }
        public DateTime DateTime { get; set; }
        public string[] Files { get; set; }
        public string Branch { get; set; }
        public string Commiter { get; set; }
    }
}
