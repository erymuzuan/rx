using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.Codes
{
    public class AssemblyInfoClass
    {
        private AssemblyInfoClass()
        {

        }
        public List<string> Attributes { get; } = new List<string>();
        public string FileName { get; private set; }

        public static async Task<AssemblyInfoClass> GenerateAssemblyInfoAsync<T>(T asset, bool autoSave = false, string folder = null) where T : Entity
        {
            dynamic assetD = asset;
            if (string.IsNullOrWhiteSpace(folder))
                folder = assetD.Name;
            var cvs = ObjectBuilder.GetObject<ICvsProvider>();
            var source = $@"{ConfigurationManager.SphSourceDirectory}\{typeof(T).Name}\{asset.Id}.json";
            var logs = await cvs.GetCommitLogsAsync(source, 1, 1);

            var fileInfo = new FileInfo(source);
            var y2012 = new DateTime(2012, 1, 1);
            var commit = logs.ItemCollection.FirstOrDefault() ?? new CommitLog
            {
                CommitId = "NA",
                DateTime = fileInfo.LastWriteTime,
                Commiter = "NA",
                Comment = "NA"
            };
            var version = $"{ConfigurationManager.MajorVersion}.{ConfigurationManager.MinorVersion}.{Convert.ToInt32((commit.DateTime - y2012).TotalDays)}.{logs.TotalRows}";

            var @class = new AssemblyInfoClass();
            @class.Attributes.Add($@"[assembly:System.Reflection.AssemblyInformationalVersion(""{version}-{commit.CommitId}"")]");
            @class.Attributes.Add($@"[assembly:System.Reflection.AssemblyFileVersion(""{version}"")]");

            @class.FileName = $"{ConfigurationManager.GeneratedSourceDirectory}\\{folder}\\AssemblyInfo.cs";

            if (autoSave)
            {
                var srcFolder = Path.GetDirectoryName(@class.FileName) ?? $"{ConfigurationManager.GeneratedSourceDirectory}\\{@class.FileName}";
                if (!Directory.Exists(srcFolder))
                    Directory.CreateDirectory(srcFolder);
                File.WriteAllText(@class.FileName, @class.ToString());
            }

            return @class;

        }

        public override string ToString()
        {
            return this.Attributes.ToString("\r\n");
        }
        public string Save(string folder)
        {
            var dir = $"{ConfigurationManager.GeneratedSourceDirectory}\\{folder}\\";
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            var file = $"{dir.Replace(@"\\", @"\")}{FileName}{ (FileName.EndsWith(".cs") ? "" : ".cs")}";
            if (Path.IsPathRooted(FileName))
                file = FileName;
            File.WriteAllText(file, this.ToString());


            return file;
        }
    }
}