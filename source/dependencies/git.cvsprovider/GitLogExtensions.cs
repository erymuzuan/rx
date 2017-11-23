using System;
using System.IO;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.CvsProviders
{
    public static class GitLogExtensions
    {
        /// <summary>
        /// File name is case sensitive with Git tools
        /// </summary>
        /// <param name="file"></param>
        /// <returns>Proper case for file, as it's on the file system</returns>
        public static string GetFullFileName(this string file)
        {
            if (File.Exists(file)) return null;
            var fi = new FileInfo(file);
            return fi.FullName;
        }
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