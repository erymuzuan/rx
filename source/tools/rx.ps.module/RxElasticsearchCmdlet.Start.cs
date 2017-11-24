using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;
using System.Threading;
using Bespoke.Sph.RxPs.Domain;
using Bespoke.Sph.RxPs.Extensions;
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global
// ReSharper disable UnusedMember.Global

namespace Bespoke.Sph.RxPs
{
    [Cmdlet(VerbsLifecycle.Start, "RxElasticsearch")]
    public class StartRxElasticsearchCmdlet : RxCmdlet
    {
        public const string VERSION_1_7_5 = "1.7.5";
        public const string VERSION_6_0_0 = "6.0.0";
        //TODO : set this to the what ever environment variable so we can use Stop-RxElasticsearch
        public int ElasticSearchId { get; private set; }
        private Process m_elasticProcess;
        [Parameter]
        public string ElasticsearchHome { get; set; } = Path.GetFullPath(".\\elasticsearch");
        [Parameter]
        public string JavaHome { get; set; } = Environment.GetEnvironmentVariable("JAVA_HOME");

        [Parameter]
        public string ElasticsearchJar { get; set; } = " .\\elasticsearch\\lib\\elasticsearch-1.7.5.jar";
        [Parameter]
        public int HttpPort { get; set; } = 9200;

        [Parameter]
        public string Version { get; set; } = VERSION_1_7_5;


        protected override void ProcessRecord()
        {
            var esHome = Path.GetDirectoryName(Path.GetDirectoryName(this.ElasticsearchJar));
            var version = this.ElasticsearchJar.RegexSingleValue(@"elasticsearch-(?<version>\d.\d.\d).jar", "version");
            WriteVerbose("Elasticsearch Home " + esHome);
            WriteVerbose("Version :" + version);

            var arg = "";
            switch (this.Version)
            {
                case VERSION_1_7_5:
                    arg = BuildCommandLineArgsVersion175();
                    break;
                case VERSION_6_0_0:
                    arg = BuildCommandLineArgsVersion600();
                    break;
                default:
                    WriteError(new ErrorRecord(new Exception("Your version is not supported"), "", ErrorCategory.FromStdErr, this));
                    break;
            }

            var started = StartElasticsearch(arg);
            if (started) return;

            CheckElasticsearchStatus();
        }

        private bool StartElasticsearch(string arg)
        {
            var info = new ProcessStartInfo
            {
                FileName = this.JavaHome + @"\bin\java.exe",
                Arguments = arg,
                WorkingDirectory = ConfigurationManager.Home,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            WriteVerbose(info.FileName);
            WriteVerbose(arg);
            if (!File.Exists(info.FileName))
            {
                WriteWarning("Cannot find Java in " + this.JavaHome);
                return true;
            }

            m_elasticProcess = Process.Start(info);
            if (null == m_elasticProcess)
            {
                WriteWarning("Cannot start elastic search");
                return true;
            }
            return false;
        }

        private void CheckElasticsearchStatus()
        {
            // verify that elasticsearch started successfully
            var connected = SendHttpRequest();
            if (connected)
            {
                WriteVerbose("ElasticSearch... [STARTED]");
                WriteVerbose("Started : " + m_elasticProcess.Id);
                ElasticSearchId = m_elasticProcess.Id;
            }
            else
            {
                WriteWarning("Cannot start your Elasticsearch");
            }
        }

        private bool SendHttpRequest()
        {
            var connected = false;
            var uri = new Uri($"http://localhost:{this.HttpPort}");
            using (var client = new HttpClient { BaseAddress = uri })
            {
                WriteVerbose($"Initiating HTTP request to http://localhost:{HttpPort}..");
                for (var i = 0; i < 20; i++)
                {
                    WriteVerbose($"Sending {i} tries HTTP Get request to http://localhost:{HttpPort}..");
                    try
                    {
                        var ok = client.GetStringAsync("/").Result;
                        connected = ok.Contains("tagline");
                        WriteVerbose("HTTP response " + ok);
                        break;
                    }
                    catch
                    {
                        WriteVerbose("Fail to get a response http://localhost:" + HttpPort);
                        // ignored
                        Thread.Sleep(500);
                    }
                }
            }
            return connected;
        }

        private string BuildCommandLineArgsVersion600()
        {
            // ReSharper disable InconsistentNaming
            var JAVA_OPTS = string.Join(" ", File.ReadAllLines($@"{ElasticsearchHome}\config\jvm.options").Where(x => !string.IsNullOrWhiteSpace(x) && !x.Trim().StartsWith("#")).Select(x => x.Trim()));
            var CLASS_PATH = $"{ElasticsearchHome}/lib/*";
            var ES_PARAMS = $@"-Delasticsearch -Des.path.home=""{ElasticsearchHome}""";
            // ReSharper restore InconsistentNaming

            var arg = $@"{JAVA_OPTS} {ES_PARAMS} -cp ""{CLASS_PATH}"" ""org.elasticsearch.bootstrap.Elasticsearch""";
            return arg;
        }

        private string BuildCommandLineArgsVersion175()
        {
            const string JAVA_OPTS =
                "-Xms256m -Xmx1g -Xss256k -XX:+UseParNewGC -XX:+UseConcMarkSweepGC -XX:CMSInitiatingOccupancyFraction=75 -XX:+UseCMSInitiatingOccupancyOnly -XX:+HeapDumpOnOutOfMemoryError";
            // ReSharper disable InconsistentNaming

            var es = Path.GetFullPath(this.ElasticsearchHome);
            //NOTE : V2 CLASS_PATH is a little different than V1
            var ES_PARAMS = $@"-Delasticsearch -Des-foreground=yes -Des.path.home=""{es}""";
            var CLASS_PATH = $"\";{es}/lib/elasticsearch-{VERSION_1_7_5}.jar;{es}/lib/*;{es}/lib/sigar/*\"";
            // ReSharper restore InconsistentNaming
            var arg = $"{JAVA_OPTS} {ES_PARAMS} -cp {CLASS_PATH}  \"org.elasticsearch.bootstrap.Elasticsearch\"";

            return arg;
        }
    }
}