using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Net.Http;
using System.Threading;
using Bespoke.Sph.RxPs.Extensions;

namespace Bespoke.Sph.RxPs
{
    [Cmdlet(VerbsLifecycle.Start, "RxElasticsearch")]
    public class StartRxElasticsearchCmdlet : RxCmdlet
    {
        private int m_elasticSearchId;
        private Process m_elasticProcess;
        [Parameter]
        public string ElasticsearchHome { get; set; } = Path.GetFullPath(".\\elasticsearch");
        [Parameter]
        public string JavaHome { get; set; } = Environment.GetEnvironmentVariable("JAVA_HOME");
        [Parameter]
        public string Home { get; set; } = Path.GetFullPath(".");
        [Parameter]
        public string ElasticsearchJar { get; set; }
        [Parameter]
        public int ElasticsearchHttpPort { get; set; } = 9200;

        protected override void ProcessRecord()
        {
            var esHome = Path.GetDirectoryName(Path.GetDirectoryName(this.ElasticsearchJar));
            var version = this.ElasticsearchJar.RegexSingleValue(@"elasticsearch-(?<version>\d.\d.\d).jar", "version");
            WriteVerbose("Elasticsearch Home " + esHome);
            WriteVerbose("Version :" + version);

            // ReSharper disable InconsistentNaming
            var JAVA_OPTS = string.Join(" ", File.ReadAllLines($@"{ElasticsearchHome}\config\jvm.options").Where(x => !string.IsNullOrWhiteSpace(x) && !x.Trim().StartsWith("#")).Select(x => x.Trim()));
            var CLASS_PATH = $"{ElasticsearchHome}/lib/*";
            var ES_PARAMS = $@"-Delasticsearch -Des.path.home=""{ElasticsearchHome}""";
            // ReSharper restore InconsistentNaming

            var arg = $@"{JAVA_OPTS} {ES_PARAMS} -cp ""{CLASS_PATH}"" ""org.elasticsearch.bootstrap.Elasticsearch""";
            var info = new ProcessStartInfo
            {
                FileName = this.JavaHome + @"\bin\java.exe",
                Arguments = arg,
                WorkingDirectory = this.Home,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };

            WriteWarning(info.FileName);
            WriteWarning(arg);
            if (!File.Exists(info.FileName))
            {

                WriteWarning("Cannot find Java in " + this.JavaHome);

                return;
            }

            m_elasticProcess = Process.Start(info);
            if (null == m_elasticProcess)
            {
                WriteWarning("Cannot start elastic search");
                return;
            }


            var connected = false;
            // verify that elasticsearch started successfully
            var uri = new Uri($"http://localhost:{this.ElasticsearchHttpPort}");
            using (var client = new HttpClient() { BaseAddress = uri })
            {
                for (var i = 0; i < 20; i++)
                {
                    try
                    {
                        var ok = client.GetStringAsync("/").Result;
                        connected = ok.Contains("tagline");
                        break;
                    }
                    catch
                    {
                        // ignored
                        Thread.Sleep(500);
                    }
                }
            }
            if (connected)
            {
                WriteVerbose("ElasticSearch... [STARTED]");
                WriteVerbose("Started : " + m_elasticProcess.Id);
                m_elasticSearchId = m_elasticProcess.Id;

            }
            else
            {
                WriteWarning("Cannot start your Elasticsearch");

            }

        }

    }
}