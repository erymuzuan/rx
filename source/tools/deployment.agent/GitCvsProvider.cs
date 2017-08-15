using System.Diagnostics;

namespace Bespoke.Sph.Mangements
{
    public class  GitCvsProvider
    {
        private readonly string m_gitPath;

        public GitCvsProvider()
        {
            
        }

        public GitCvsProvider(string gitPath)
        {
            m_gitPath = gitPath;
        }
        public string GetCommitId()
        {
            //git rev-parse --short HEAD 
            var path = string.IsNullOrWhiteSpace(m_gitPath) ? "git.exe" : m_gitPath;
            var git = new ProcessStartInfo(path, "rev-parse --short HEAD")
            {
                UseShellExecute = false,
                RedirectStandardOutput = true
            };
            var p = Process.Start(git);
            var output = p?.StandardOutput.ReadToEnd();
            p?.WaitForExit();
            return output;
        }

        public string GetCommitComment()
        {
            //git log -1
            var path = string.IsNullOrWhiteSpace(m_gitPath) ? "git.exe" : m_gitPath;
            var git = new ProcessStartInfo(path, "log -1")
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
            return output;
        }
    }
}
