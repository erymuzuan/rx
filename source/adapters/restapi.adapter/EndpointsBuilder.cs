using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Integrations.Adapters
{
    public static class EndpointsBuilderFactory
    {
        public static EndpointsBuilder Create(string storeId)
        {
            return new HarEndpointsBuilder(storeId);
        }
    }
    public class HarEndpointsBuilder : EndpointsBuilder
    {
        private readonly string m_storeId;

        public HarEndpointsBuilder(string storeId)
        {
            m_storeId = storeId;
        }
        public static Encoding GetFileEncoding(string srcFile)
        {
            // *** Use Default of Encoding.Default (Ansi CodePage)
            Encoding enc = Encoding.Default;

            // *** Detect byte order mark if any - otherwise assume default
            byte[] buffer = new byte[5];
            FileStream file = new FileStream(srcFile, FileMode.Open);
            file.Read(buffer, 0, 5);
            file.Close();

            if (buffer[0] == 0xef && buffer[1] == 0xbb && buffer[2] == 0xbf)
                enc = Encoding.UTF8;
            else if (buffer[0] == 0xfe && buffer[1] == 0xff)
                enc = Encoding.Unicode;
            else if (buffer[0] == 0 && buffer[1] == 0 && buffer[2] == 0xfe && buffer[3] == 0xff)
                enc = Encoding.UTF32;
            else if (buffer[0] == 0x2b && buffer[1] == 0x2f && buffer[2] == 0x76)
                enc = Encoding.UTF7;
            else if (buffer[0] == 0xFE && buffer[1] == 0xFF)
                // 1201 unicodeFFFE Unicode (Big-Endian)
                enc = Encoding.GetEncoding(1201);
            else if (buffer[0] == 0xFF && buffer[1] == 0xFE)
                // 1200 utf-16 Unicode
                enc = Encoding.GetEncoding(1200);


            return enc;
        }
        public override async Task<IEnumerable<RestApiOperationDefinition>> BuildAsync()
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var har = await store.GetContentAsync(m_storeId);
            var temp = System.IO.Path.GetTempFileName();
            File.WriteAllBytes(temp, har.Content);
            var text = File.ReadAllText(temp);

            var json = JObject.Parse(text);
            var entries = json.SelectToken("$.log.entries").Select(e => new RestApiOperationDefinition(e)).ToList();
            var tasks = entries.Select(x => x.BuildAsync());
            await Task.WhenAll(tasks);
            return entries;
        }
    }
    public abstract class EndpointsBuilder
    {

        public abstract Task<IEnumerable<RestApiOperationDefinition>> BuildAsync();
    }


}
