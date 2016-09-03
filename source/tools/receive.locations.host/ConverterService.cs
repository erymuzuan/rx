using System.IO;

namespace receive.locations.host
{
    public interface IReceiveLocation
    {
        bool Start();
        bool Stop();
        void Pause();
        void Resume();
    }

    public class ConverterService : IReceiveLocation
    {
        private FileSystemWatcher m_watcher;
        private bool m_paused;

        public bool Start()
        {
            m_watcher = new FileSystemWatcher(@"c:\temp\a", "*_in.txt");
            m_watcher.Created += FileCreated;
            m_watcher.IncludeSubdirectories = false;
            m_watcher.EnableRaisingEvents = true;

            return true;
        }

        private void FileCreated(object sender, FileSystemEventArgs e)
        {
            if (m_paused) return;
            var content = File.ReadAllText(e.FullPath);
            var upperContent = content.ToUpperInvariant();
            var dir = Path.GetDirectoryName(e.FullPath);
            var convertedFileName = Path.GetFileName(e.FullPath) + ".converted";
            var convertedPath = Path.Combine(dir, convertedFileName);
            File.WriteAllText(convertedPath, upperContent);
        }

        public bool Stop()
        {
            m_watcher.Dispose();
            return true;
        }

        public void Pause()
        {
            m_paused = true;
        }

        public void Resume()
        {
            m_paused = false;
        }
    }
}