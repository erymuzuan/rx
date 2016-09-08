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
            var path = $@"{System.IO.Path.GetTempPath()}/file-drop-receive-location{Id}/";
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            var zip = path + ".zip";
            var store = ObjectBuilder.GetObject<IBinaryStore>();

            // copy the output, receive port, domain, topshelf, newton, filehelpers
            File.Copy($"{ConfigurationManager.CompilerOutputPath}\\{this.AssemblyName}", path);

            if (File.Exists(zip))
                File.Delete(zip);
            ZipFile.CreateFromDirectory(path, zip);
            var zd = new BinaryStore
            {
                Id = SequentialGuid.NewSequentialGuid().ToString(),
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
