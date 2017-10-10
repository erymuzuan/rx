using System;
using System.Diagnostics;
using System.Linq;

namespace Bespoke.Sph.Mangements
{
    public class GitCvsProvider
    {
        private readonly string m_gitPath;

        public GitCvsProvider()
        {

        }

        public GitCvsProvider(string gitPath)
        {
            m_gitPath = gitPath;
        }
        public string GetCommitId(string file)
        {
            //git rev-parse --short HEAD 
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
            return output.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        }

        public string GetCommitComment(string file)
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
            // Do not wait for the child process to exit before
            // reading to the end of its redirected stream.
            // p.WaitForExit();
            // Read the output stream first and then wait.
            var output = p?.StandardOutput.ReadToEnd();
            p?.WaitForExit();
            if (string.IsNullOrWhiteSpace(output)) return null;
            return output.Split(new[] { "\n", "\r\n", "\r" }, StringSplitOptions.RemoveEmptyEntries).FirstOrDefault();
        }
    }
}
