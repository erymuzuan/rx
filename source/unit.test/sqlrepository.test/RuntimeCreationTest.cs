using System;
using System.Linq;
using System.Reflection;
using Bespoke.Sph.Domain;
using NUnit.Framework;

namespace sqlrepository.test
{
    [TestFixture]
    public class RuntimeCreationTest
    {
        [Test]
        public void Create()
        {
            var assembly = Assembly.Load("sql.repository");
            Assert.IsNotNull(assembly,"Assembly is null");

            var sql = assembly.GetType("Bespoke.Sph.SqlRepository.SqlRepository`1");
            Assert.IsNotNull(sql, "SqlRepository<T> is null");

            var ce = typeof (Designation);

            var reposType = sql.MakeGenericType(ce);
            Assert.IsNotNull(reposType);
            var repository = Activator.CreateInstance(reposType);
            Assert.IsNotNull(repository);

            var ff = typeof(IRepository<>).MakeGenericType(new[] { ce });

            ObjectBuilder.AddCacheList(ff, repository);

            var dep = ObjectBuilder.GetObject(ff);
            Assert.IsNotNull(dep);

        }
    }
}
