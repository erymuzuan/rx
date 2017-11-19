using System;
using System.Reflection;
using Bespoke.Sph.Domain;
using Xunit;

namespace sqlrepository.test
{
    public class RuntimeCreationTest
    {
        [Fact]
        public void Create()
        {
            var assembly = Assembly.Load("sql.repository");
            Assert.NotNull(assembly);

            var sql = assembly.GetType("Bespoke.Sph.SqlRepository.SqlRepository`1");
            Assert.NotNull(sql);

            var ce = typeof (Designation);

            var reposType = sql.MakeGenericType(ce);
            Assert.NotNull(reposType);
            var repository = Activator.CreateInstance(reposType);
            Assert.NotNull(repository);

            var ff = typeof(IRepository<>).MakeGenericType(new[] { ce });

            ObjectBuilder.AddCacheList(ff, repository);

            var dep = ObjectBuilder.GetObject(ff);
            Assert.NotNull(dep);

        }
    }
}
