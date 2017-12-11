using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using odata.queryparser;
using Xunit;

namespace Bespoke.Sph.QueryParserTests
{
    public class FilterTest
    {


        [Fact]
        public void Term()
        {
            var ed = new EntityDefinition{ Name = "Customer"};
            ed += ("FullName", typeof(string));

            var text = @"$filter=HomeAddress/State eq 'Kelantan'";
            var query = new OdataQueryParser().Parse(text, ed);

            Assert.Single(query.Filters);
            var term = query.Filters.Single();
            Assert.Equal("HomeAddress.State", term.Term);
            Assert.Equal(Operator.Eq, term.Operator);
            Assert.IsType<ConstantField>(term.Field);
            Assert.Equal(1, term.Field.GetValue(default));
            Console.WriteLine(term);
        }
    }
}
