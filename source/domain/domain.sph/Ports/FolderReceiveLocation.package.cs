using System;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class FolderReceiveLocation

    {
        public override async Task<string> PackageAsync()
        {
            var path = $@"{System.IO.Path.GetTempPath()}file-drop-receive-location{Id}";
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            var zip = path + ".zip";
            var store = ObjectBuilder.GetObject<IBinaryStore>();

            var context = new SphDataContext();
            var port = context.LoadOneFromSources<ReceivePort>(x => x.Id == this.ReceivePort);


            AddSupportingFiles(path);
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{this.AssemblyName}", path + "\\" + this.AssemblyName, true);
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{this.PdbName}", path + "\\" + this.PdbName, true);

            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{port.AssemblyName}", path + "\\" + port.AssemblyName, true);
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{port.PdbName}", path + "\\" + port.PdbName, true);


            Action<string, bool> copyFromWeb = (name, includePdb) =>
            {
                File.Copy($"{ConfigurationManager.WebPath}\\bin\\{name}.dll", $"{path}\\{name}.dll", true);
                var pdb = $"{ConfigurationManager.WebPath}\\bin\\{name}.pdb";
                if (includePdb && File.Exists(pdb))
                {
                    File.Copy(pdb, $"{path}\\{name}.pdb", true);
                }
            };
            Action<string, string, string> copyFromPackages = (name, version, clr) =>
            {
                File.Copy(ConfigurationManager.GetPackage(name, version, clr).Location, $"{path}\\{name}.dll", true);
            };

            copyFromWeb("Newtonsoft.Json", false);
            copyFromWeb("domain.sph", true);

            copyFromPackages("FileHelpers", "3.1.5", "net45");
            copyFromPackages("Topshelf", "4.0.2", "net452");
            copyFromPackages("Polly", "4.2.4", "net45");
            copyFromPackages("NLog", "4.3.9", "net45");
            copyFromPackages("Topshelf.NLog", "4.0.2", "net452");



            if (File.Exists(zip))
                File.Delete(zip);
            ZipFile.CreateFromDirectory(path, zip);
            var zd = new BinaryStore
            {
                Id = Guid.NewGuid().ToString(),
                Content = File.ReadAllBytes(zip),
                Extension = ".zip",
                FileName = $"file-drop-receive-location-{Id}.zip",
                WebId = Guid.NewGuid().ToString()
            };
            await store.AddAsync(zd);

            return zd.Id;
        }

        private void AddSupportingFiles(string zipPath)
        {

            var config = $@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration>
  <configSections>
    <section name=""nlog"" type=""NLog.Config.ConfigSectionHandler, NLog""/>
  </configSections>
 <appSettings>
    <add key=""sph:ApplicationName"" value=""{ConfigurationManager.ApplicationName}"" />
  </appSettings>
  <startup>
    <supportedRuntime version=""v4.0"" sku="".NETFramework,Version=v4.6.1"" />
  </startup>
  <nlog
      xmlns=""http://www.nlog-project.org/schemas/NLog.xsd""
      xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">
    <targets>
        <!-- Log in a separate thread, possibly queueing up to
        5000 messages. When the queue overflows, discard any
        extra messages -->

        <target name=""file"" xsi:type=""AsyncWrapper"" queueLimit=""5000"" overflowAction=""Discard"">
            <target xsi:type=""File"" fileName=""${{basedir}}/logs/${{shortdate}}.log"" />
        </target>
    
        <target name=""error"" type=""File"" fileName=""${{basedir}}/logs/${{shortdate}}.error.log""/>
        <target name=""console"" type=""Console"" />
    </targets>
    <rules>
      <logger name=""*"" minlevel=""Trace"" writeTo=""console"" />
      <logger name=""*"" minlevel=""Info"" maxlevel=""Warn"" writeTo=""file"" />
      <logger name=""*"" minlevel=""Error"" writeTo=""error"" />
    </rules>
  </nlog>
</configuration>";


            var ps1 = $@"
Param(
       [switch]$Install = $false,
       [switch]$Uninstall = $false,
       [string]$DropFolder = '{Path}'
     )
$env:RX_{ConfigurationManager.ApplicationName}_{Name}_ArchiveLocation=""{ArchiveLocation}""
$env:RX_{ConfigurationManager.ApplicationName}_{Name}_JwtToken=""{JwtToken}""
$env:RX_{ConfigurationManager.ApplicationName}_{Name}_Path=$DropFolder
& .\{AssemblyName} run
";

            var readme = $@"
# Quick start guide for testing and deploying your receive location

## To run in console
start your command prompt
.\{AssemblyName} run

You may have to stop your service, if it's running


## Installing
start your command prompt with Administrator account, or elevated command prompt
.\{AssemblyName} install

## Uinstalling
start your command prompt with Administrator account, or elevated command prompt
.\{AssemblyName} uinstall

## Configuration
Change your {AssemblyName}.config file to add additional logger, or change the path to default file logger
Refer to NLog at ![https://github.com/NLog/NLog](NLog GitHub page) for more info on logging with NLog

";
            File.WriteAllText($"{zipPath}\\{AssemblyName}.config", config);
            File.WriteAllText($"{zipPath}\\start.ps1", ps1);
            File.WriteAllText($"{zipPath}\\README.txt", readme);
        }


    }
}
