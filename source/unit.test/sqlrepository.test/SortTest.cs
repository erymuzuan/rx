using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using sqlrepository.test.Models;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.Tests.Elasticsearch
{
    public class SortTest
    {
        private ITestOutputHelper Console { get; }

        public SortTest(ITestOutputHelper console)
        {
            Console = console;
        }

        [Fact]
        public void NotSortSpecified()
        {
            var query = new QueryDsl();
            var sql = query.CompileToSql<Patient>().WriteObject(Console).OneLine();
            Assert.Contains("ORDER BY [Id]", sql);
        }
        [Fact]
        public void SingleSort()
        {
            var query = new QueryDsl(sorts:new []
            {
                new Sort{Path = "Dob"} 
            });
            var sql = query.CompileToSql<Patient>().WriteObject(Console).OneLine();
            Assert.Contains("ORDER BY [Dob]", sql);
            Assert.DoesNotContain("[Dob] ASC", sql);
            Assert.DoesNotContain("[Dob] DESC", sql);
        }

        [Fact]
        public void SingleSortAsc()
        {
            var query = new QueryDsl(sorts:new []
            {
                new Sort{Path = "Dob", Direction = SortDirection.Asc} 
            });
            var sql = query.CompileToSql<Patient>().WriteObject(Console).OneLine();
            Assert.Contains("ORDER BY [Dob]", sql);
            Assert.DoesNotContain("[Dob] ASC", sql);
            Assert.DoesNotContain("[Dob] DESC", sql);
        }


        [Fact]
        public void SingleSortDesc()
        {
            var query = new QueryDsl(sorts:new []
            {
                new Sort{Path = "Dob", Direction = SortDirection.Desc} 
            });
            var sql = query.CompileToSql<Patient>().WriteObject(Console).OneLine();
            Assert.Contains("ORDER BY [Dob] DESC", sql);
            Assert.DoesNotContain("[Dob] ASC", sql);
            Assert.Contains("[Dob] DESC", sql);
        }
        [Fact]
        public void MultipleSorts()
        {
            var query = new QueryDsl(sorts:new []
            {
                new Sort{Path = "Dob", Direction = SortDirection.Desc} ,
                new Sort{Path = "HomeAddress.State", Direction = SortDirection.Desc}


            });
            var sql = query.CompileToSql<Patient>().WriteObject(Console).OneLine();
            Assert.Contains("ORDER BY [Dob] DESC, [HomeAddress.State] DESC", sql);
            Assert.DoesNotContain("[Dob] ASC", sql);
            Assert.Contains("[Dob] DESC", sql);
        }




    }
}