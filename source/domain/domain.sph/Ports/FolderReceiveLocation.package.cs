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

            // copy the output, receive port, domain, topshelf, newton, filehelpers
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{this.AssemblyName}", path + "\\" + this.AssemblyName, true);
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{this.PdbName}",      path + "\\" + this.PdbName, true);

            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{port.AssemblyName}", path + "\\" + port.AssemblyName, true);
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{port.PdbName}",      path + "\\" + port.PdbName, true);
            
            File.Copy($"{ConfigurationManager.WebPath}\\bin\\domain.sph.dll",      path + "\\domain.sph.dll", true);
            File.Copy($"{ConfigurationManager.WebPath}\\bin\\domain.sph.pdb",      path + "\\domain.sph.pdb", true);

            File.Copy($"{ConfigurationManager.WebPath}\\bin\\Newtonsoft.Json.dll",      path + "\\Newtonsoft.Json.dll", true);
            File.Copy($"{ConfigurationManager.WebPath}\\bin\\FileHelpers.dll",      path + "\\FileHelpers.dll", true);
            File.Copy($"{ConfigurationManager.Home}\\subscribers.host\\Topshelf.dll",      path + "\\Topshelf.dll", true);



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
    }
}
