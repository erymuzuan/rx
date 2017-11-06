using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using Bespoke.Sph.RxPs.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.RxPs
{

    /// <summary>
    /// <para type="synopsis">List the selected assets deployment status</para>
    /// <para type="description">The compiled version  of the dll is compared to the latest source version.</para>
    /// <para type="description">Revision no. or tag/commitid tells if the deployed version of the dll is up to date the asset source</para>
    /// </summary>
    [Cmdlet(VerbsCommon.Get, "Rx" + nameof(DeploymentStatus))]
    [OutputType(typeof(DeploymentStatus))]
    public class GetRxDeploymentStatus : RxCmdlet
    {
        public const string PARAMETER_SET_ID = "Id";
        public const string PARAMETER_SET_NAME = "Names";

        [Parameter(ParameterSetName = PARAMETER_SET_NAME), ArgumentCompleter(typeof(AssetNameCompleter<EntityDefinition>))]
        public string Name { set; get; }


        [Parameter(ParameterSetName = PARAMETER_SET_ID), ArgumentCompleter(typeof(AssetIdCompleter<EntityDefinition>))]
        public string Id { get; set; }


        [Parameter(ParameterSetName = PARAMETER_SET_NAME)]
        [Parameter(ParameterSetName = PARAMETER_SET_ID)]
        public ICvsProvider CvsProvider { get; set; } = new GitCvsProvider();

        protected override void ProcessRecord()
        {
            if (this.ParameterSetName == PARAMETER_SET_ID)
            {
                var source = $@"{ConfigurationManager.SphSourceDirectory}\{nameof(EntityDefinition)}\{Id}.json";
                WriteVerbose($"Source {source} [Exist]{File.Exists(source)}");
                if (!File.Exists(source))
                {
                    WriteWarning($"Source {source} does not exist");
                    return;
                }

                var sourceJson = JObject.Parse(File.ReadAllText(source));
                var sourceName = sourceJson.SelectToken("$.Name").Value<string>();
                var sourceId = sourceJson.SelectToken("$.Id").Value<string>();
                var sourceChangedDate = sourceJson.SelectToken("$.ChangedDate").Value<DateTime>();

                var lo = this.CvsProvider.GetCommitLogsAsync(source, 1, 1).Result;
                var sourceLog = lo.ItemCollection.FirstOrDefault() ?? new CommitLog
                {
                    CommitId = "NA"
                };
                var output = new FileVersion($@"{ConfigurationManager.CompilerOutputPath}\{RxApplicationName}.{sourceName}.dll");
                WriteVerbose($"output - {output} [Exist]{File.Exists(output.FullName)}");
                WriteObject(new DeploymentStatus(WriteVerbose, output, sourceLog.CommitId, lo.TotalRows)
                {
                    Name = sourceName,
                    Id = sourceId,
                    SourceChangedOn = sourceChangedDate,
                    Location = ConfigurationManager.CompilerOutputPath,
                    Type = nameof(EntityDefinition)

                });
                var web = new FileVersion($@"{ConfigurationManager.WebPath}\bin\{RxApplicationName}.{sourceName}.dll");
                WriteVerbose($"Web - {web} [Exist]{File.Exists(web.FullName)}");
                WriteObject(new DeploymentStatus(WriteVerbose, web, sourceLog.CommitId, lo.TotalRows)
                {
                    Name = sourceName,
                    Id = sourceId,
                    SourceChangedOn = sourceChangedDate,
                    Location = ConfigurationManager.WebPath,
                    Type = nameof(EntityDefinition)

                });

                var subscribers = ConfigurationManager.SubscriberPath.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var subFolder in subscribers)
                {
                    var sub = new FileVersion($@"{subFolder}\{RxApplicationName}.{sourceName}.dll");
                    WriteVerbose($"Sub - {sub} [Exist]{File.Exists(sub.FullName)}");
                    WriteObject(new DeploymentStatus(WriteVerbose, sub, sourceLog.CommitId, lo.TotalRows)
                    {
                        Name = sourceName,
                        Id = sourceId,
                        SourceChangedOn = sourceChangedDate,
                        Location = subFolder,
                        Type = nameof(EntityDefinition)
                    });

                }
            }
        }
    }



    public class DeploymentStatus
    {
        public Action<string> WriteVerbose { get; }
        public DeploymentStatus(Action<string> writeVerbose, FileVersion deployedVersion, string sourceCommitId, int? sourceRevision)
        {
            WriteVerbose = writeVerbose;
            if (File.Exists(deployedVersion.FullName))
            {
                var versionNumbers = deployedVersion.VersionNumber.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                var productInformations = deployedVersion.ProductVersion.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                if (int.TryParse(
                versionNumbers.LastOrDefault(),
                out var deployedRevision))
                    CompiledRevision = deployedRevision;

                if (int.TryParse(versionNumbers[2], out var days))
                    CompiledOn = new DateTime(2012, 1, 1).AddDays(days);

                CompiledTag = productInformations.LastOrDefault();
            }
            SourceTag = sourceCommitId;
            SourceRevision = sourceRevision;
        }


        public string Type { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string CompiledTag { get; set; }
        public string SourceTag { get; set; }
        public int? CompiledRevision { get; set; }
        public int? SourceRevision { get; set; }
        public DateTime CompiledOn { get; set; }
        public DateTime SourceChangedOn { get; set; }
        public string Location { get; set; }
    }


    public class FileVersion
    {
        public FileVersion(string fullName)
        {
            FullName = fullName;
            if (!File.Exists(fullName)) return;

            var version = FileVersionInfo.GetVersionInfo(fullName);
            ProductVersion = version.ProductVersion;
            VersionNumber = version.FileVersion;
        }

        public string Description { get; set; }
        public string FullName { get; set; }
        public string ProductVersion { get; set; }
        public string VersionNumber { get; set; }

        public override string ToString()
        {
            return $"{FullName} VersionNumber : {VersionNumber}, ProductVersion : {ProductVersion}";
        }
    }


    public interface ICvsProvider
    {
        Task<string> GetCommitIdAsync(string file);
        Task<string> GetCommitCommentAsync(string file);
        Task<LoadOperation<CommitLog>> GetCommitLogsAsync(string file, int page, int size);
    }

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

    public class LoadOperation<T>
    {
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
        public List<T> ItemCollection { get; } = new List<T>();
        public int? TotalRows { get; set; }
    }

    public class CommitLog
    {
        public DateTime DateTime { get; set; }
        public string Comment { get; set; }
        public string Commiter { get; set; }
        public string CommitId { get; set; }
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
