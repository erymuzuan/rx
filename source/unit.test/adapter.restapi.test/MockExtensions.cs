using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Moq;

namespace adapter.restapi.test
{
    public static class MockExtensions
    {
        public static void SetupAndReturnDoc(this Mock<IBinaryStore> store, string id)
        {

            store.Setup(x => x.GetContentAsync(id))
                .Returns(() =>
                {
                    var doc = new BinaryStore
                    {
                        Content = File.ReadAllBytes(id),
                        Id = id,
                        Extension = ".txt",
                        FileName = id
                    };
                    return Task.FromResult(doc);
                });
        }
    }
}