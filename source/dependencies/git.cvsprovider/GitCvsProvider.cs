using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.CvsProviders
{
    public class GitCvsProvider : ICvsProvider
    {
        private readonly string m_gitPath;

        public GitCvsProvider()
        {

        }

        public GitCvsProvider(string gitPath)
        {
            m_gitPath = gitPath;
        }

        public Task<string> GetCommitIdAsync(string file)
        {//git rev-parse --short HEAD 
            var path = string.IsNullOrWhiteSpace(m_gitPath) ? "git.exe" : m_gitPath;
            var git = new ProcessStartInfo(path, "log -n 1 --pretty=format:%h " + file)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            var p = Process.Start(git);
            var output = p?.StandardOutput.ReadToEnd();
            p?.WaitForExit();
            if (string.IsNullOrWhiteSpace(output)) return null;
            var commit = output.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();

            return Task.FromResult(commit);
        }

        public Task<string> GetCommitCommentAsync(string file)
        {
            //git log -1
            var path = string.IsNullOrWhiteSpace(m_gitPath) ? "git.exe" : m_gitPath;
            var git = new ProcessStartInfo(path, "log -n 1 --pretty=format:%s " + file)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            // Redirect the output stream of the child process.
            var p = Process.Start(git);
            var output = p?.StandardOutput.ReadToEnd();
            p?.WaitForExit();
            if (string.IsNullOrWhiteSpace(output)) return null;
            var comment = output.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
            return Task.FromResult(comment);
        }

        public Task<LoadOperation<CommitLog>> GetCommitLogsAsync(string file, int page, int size)
        {
            var lo = new LoadOperation<CommitLog>
            {
                CurrentPage = page,
                PageSize = size,
                TotalRows = GetTotalCommitCount(file)
            };
            var git = new ProcessStartInfo("git.exe", $"log --skip {(page - 1) * size} -n {size}  --pretty=format:%h^-^%an^-^%aI^-^%s " + file)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            // Redirect the output stream of the child process.
            var p = Process.Start(git);
            var output = p?.StandardOutput.ReadToEnd();
            p?.WaitForExit();
            if (string.IsNullOrWhiteSpace(output)) return Task.FromResult(lo);

            var commitLogs = from ln in output.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries)
                             let log = ln.Parse()
                             where log != null
                             select log;

            lo.ItemCollection.AddRange(commitLogs);
            return Task.FromResult(lo);
        }

        private static int? GetTotalCommitCount(string file)
        {
            var git = new ProcessStartInfo("git.exe", "rev-list --all --count " + file)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };

            // Redirect the output stream of the child process.
            var p = Process.Start(git);
            var output = p?.StandardOutput.ReadToEnd();
            p?.WaitForExit();
            if (string.IsNullOrWhiteSpace(output)) return default;
            if (int.TryParse(output, out var total))
                return total;
            return default;

        }
    }

    public static class GitLogExtensions
    {
        public static CommitLog Parse(this string ln, string delimiter = "^-^")
        {
            var logs = ln.Split(new[] { delimiter }, StringSplitOptions.None);
            try
            {
                var log = new CommitLog
                {
                    Comment = logs[3],
                    DateTime = DateTime.Parse(logs[2]),
                    Commiter = logs[1],
                    CommitId = logs[0]
                };

                return log;

            }
            catch (Exception)
            {
                return null;
            }


        }
    }
}