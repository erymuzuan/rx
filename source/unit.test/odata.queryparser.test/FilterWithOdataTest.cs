using System;
using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Moq;
using odata.queryparser;
using Xunit;
using Xunit.Abstractions;

namespace Bespoke.Sph.ODataQueryParserTests
{
    /*
     *
4.5. Filter System Query Option ($filter)

A URI with a $filter System Query Option identifies a subset of the Entries from the Collection of Entries identified by the Resource Path section of the URI. The subset is determined by selecting only the Entries that satisfy the predicate expression specified by the query option.

The expression language that is used in $filter operators supports references to properties and literals. The literal values can be strings enclosed in single quotes, numbers and boolean values (true or false) or any of the additional literal representations shown in the Abstract Type System section.

Note: The $filter section of the normative OData specification provides an ABNF grammar for the expression language supported by this query option.

The operators supported in the expression language are shown in the following table.
Operator 	Description 	Example
Logical Operators
Eq 	Equal 	/Suppliers?$filter=Address/City eq 'Redmond'
Neq 	Not equal 	/Suppliers?$filter=Address/City ne 'London'
Gt 	Greater than 	/Products?$filter=Price gt 20
Ge 	Greater than or equal 	/Products?$filter=Price ge 10
Lt 	Less than 	/Products?$filter=Price lt 20
Le 	Less than or equal 	/Products?$filter=Price le 100
And 	Logical and 	/Products?$filter=Price le 200 and Price gt 3.5
Or 	Logical or 	/Products?$filter=Price le 3.5 or Price gt 200
Not 	Logical negation 	/Products?$filter=not endswith(Description,'milk')
Arithmetic Operators
Add 	Addition 	/Products?$filter=Price add 5 gt 10
Sub 	Subtraction 	/Products?$filter=Price sub 5 gt 10
Mul 	Multiplication 	/Products?$filter=Price mul 2 gt 2000
Div 	Division 	/Products?$filter=Price div 2 gt 4
Mod 	Modulo 	/Products?$filter=Price mod 2 eq 0
Grouping Operators
( ) 	Precedence grouping 	/Products?$filter=(Price sub 5) gt 10

In addition to operators, a set of functions are also defined for use with the filter query string operator. The following table lists the available functions. Note: ISNULL or COALESCE operators are not defined. Instead, there is a null literal which can be used in comparisons.
Function 	Example
String Functions
bool substringof(string po, string p1) 	http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=substringof('Alfreds', CompanyName) eq true
bool endswith(string p0, string p1) 	http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=endswith(CompanyName, 'Futterkiste') eq true
bool startswith(string p0, string p1) 	http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=startswith(CompanyName, 'Alfr') eq true
int length(string p0) 	http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=length(CompanyName) eq 19
int indexof(string p0, string p1) 	http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=indexof(CompanyName, 'lfreds') eq 1
string replace(string p0, string find, string replace) 	http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=replace(CompanyName, ' ', '') eq 'AlfredsFutterkiste'
string substring(string p0, int pos) 	http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=substring(CompanyName, 1) eq 'lfreds Futterkiste'
string substring(string p0, int pos, int length) 	http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=substring(CompanyName, 1, 2) eq 'lf'
string tolower(string p0) 	http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=tolower(CompanyName) eq 'alfreds futterkiste'
string toupper(string p0) 	http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=toupper(CompanyName) eq 'ALFREDS FUTTERKISTE'
string trim(string p0) 	http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=trim(CompanyName) eq 'Alfreds Futterkiste'
string concat(string p0, string p1) 	http://services.odata.org/Northwind/Northwind.svc/Customers?$filter=concat(concat(City, ', '), Country) eq 'Berlin, Germany'
Date Functions
int day(DateTime p0) 	http://services.odata.org/Northwind/Northwind.svc/Employees?$filter=day(BirthDate) eq 8
int hour(DateTime p0) 	http://services.odata.org/Northwind/Northwind.svc/Employees?$filter=hour(BirthDate) eq 0
int minute(DateTime p0) 	http://services.odata.org/Northwind/Northwind.svc/Employees?$filter=minute(BirthDate) eq 0
int month(DateTime p0) 	http://services.odata.org/Northwind/Northwind.svc/Employees?$filter=month(BirthDate) eq 12
int second(DateTime p0) 	http://services.odata.org/Northwind/Northwind.svc/Employees?$filter=second(BirthDate) eq 0
int year(DateTime p0) 	http://services.odata.org/Northwind/Northwind.svc/Employees?$filter=year(BirthDate) eq 1948
Math Functions
double round(double p0) 	http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=round(Freight) eq 32d
decimal round(decimal p0) 	http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=round(Freight) eq 32
double floor(double p0) 	http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=round(Freight) eq 32d
decimal floor(decimal p0) 	http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=floor(Freight) eq 32
double ceiling(double p0) 	http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=ceiling(Freight) eq 33d
decimal ceiling(decimal p0) 	http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=floor(Freight) eq 33
Type Functions
bool IsOf(type p0) 	http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=isof('NorthwindModel.Order')
bool IsOf(expression p0, type p1) 	http://services.odata.org/Northwind/Northwind.svc/Orders?$filter=isof(ShipCountry, 'Edm.String')
     * 
     */
    public class FilterWithOdataTest
    {
        public FilterWithOdataTest(ITestOutputHelper console)
        {
            Console = console;

            var cache = new Mock<ICacheManager>(MockBehavior.Strict);
            ObjectBuilder.AddCacheList<ISourceRepository>(SourceRepository);
            ObjectBuilder.AddCacheList(cache.Object);
            ObjectBuilder.AddCacheList<ILogger>(new XunitConsoleLogger(console));
        }

        private ITestOutputHelper Console { get; }
        private MockSourceRepository SourceRepository { get; } = new MockSourceRepository();

        private static EntityDefinition CreateEmployeeEntityDefinition(string name = "Employee")
        {
            var ed = new EntityDefinition { Name = name, Plural = "Employees", RecordName = "No", Id = "employee" };
            ed.AddSimpleMember<int>("No");
            ed.AddSimpleMember<string>("FirstName");
            ed.AddSimpleMember<string>("LastName");
            ed.AddSimpleMember<string>("Description");
            ed.AddSimpleMember<string>("Gender");
            ed.AddSimpleMember<int>("Age");
            ed.AddSimpleMember<DateTime>("DateOfBirth");
            ed.AddSimpleMember<DateTime>("HireDate");

            var address = new ComplexMember { Name = "HomeAddress", TypeName = "Address" };
            address.AddMember<string>("Street1");
            address.AddMember<string>("Street2");
            address.AddMember<string>("City");
            address.AddMember<string>("State");
            address.AddMember<string>("Country");
            address.AddMember<int>("Postcode");
            ed.MemberCollection.Add(address);

            var contacts = new ComplexMember { Name = "Contacts", TypeName = "Contact", AllowMultiple = true };
            contacts.Add(new Dictionary<string, Type> { { "Name", typeof(string) }, { "Telephone", typeof(string) } });
            ed.MemberCollection.Add(contacts);

            return ed;
        }

        [Fact]
        public void ParseFilterCompound()
        {
            const string TEXT = "$filter=No gt 50 or (FirstName eq 'Scott' and Age lt 40)";
            const string ENTITY = "Employee";

            var ed = CreateEmployeeEntityDefinition();
            SourceRepository.AddOrReplace(ed);
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var noGt50 = query.Filters.SingleOrDefault(x => x.Term == "No");
            var firstNameEqScott = query.Filters.SingleOrDefault(x => x.Term == "FirstName");
            var ageLt40 = query.Filters.SingleOrDefault(x => x.Term == "Age");
            Assert.NotNull(noGt50);
            Assert.NotNull(firstNameEqScott);
            Assert.NotNull(ageLt40);

            Assert.Equal(Operator.Eq, firstNameEqScott.Operator);
            Assert.IsType<ConstantField>(firstNameEqScott.Field);
            Assert.Equal(Operator.Gt, noGt50.Operator);
            Assert.IsType<ConstantField>(noGt50.Field);
            Assert.Equal(Operator.Lt, ageLt40.Operator);
            Assert.IsType<ConstantField>(ageLt40.Field);
        }

        [Theory]
        [InlineData("FirstName", "$filter=Age gt 50 and FirstName eq 'Scott'", "Employee", Operator.Eq, "Scott")]
        [InlineData("Age", "$filter=Age gt 50 and FirstName eq 'Scott'", "Employee", Operator.Gt, 50)]
        [InlineData("Description", "$filter=Description eq 'this or that but not those'", "Employee", Operator.Eq, "this or that but not those")]
        [InlineData("State", "$filter=contains(HomeAddress/State, 'Wilayah Persekutuan')", "Employee", Operator.Substringof, "Wilayah")]
        [InlineData("FirstName", "$filter=Age gt 50 or (FirstName eq 'Scott' and Age lt 40)", "Employee", Operator.Eq, "Scott")]
        public void ParseFilter(string name, string filter, string entity, Operator expectedOperator,
            object expectedContantValue)
        {
            var ed = CreateEmployeeEntityDefinition(entity);
            SourceRepository.AddOrReplace(ed);

            var parser = new OdataQueryParser();
            var query = parser.Parse(filter, entity);

            var ft = query.Filters.SingleOrDefault(x => x.Term == name);
            Assert.NotNull(ft);
            Assert.Equal(expectedOperator, ft.Operator);
            Assert.Equal(expectedContantValue, ft.Field.GetValue(default));

            Console.WriteLine($"filter : {ft.Term} {ft.Operator} {ft.Field.GetValue(default)}");
        }


        [Theory]
        [InlineData("$filter=Age gt 50 or (FirstName eq 'Scott' and Age lt 40)", "Employee", 2)]
        public void ParseCpmpoundOrFilter(string filter, string entity, int childFilters)
        {
            var ed = CreateEmployeeEntityDefinition(entity);
            SourceRepository.AddOrReplace(ed);

            var parser = new OdataQueryParser();
            var query = parser.Parse(filter, entity);

            var ft = query.Filters.OfType<BinaryOrFilter>().FirstOrDefault();
            Assert.NotNull(ft);
            Assert.IsType<BinaryOrFilter>(ft);
            Assert.Equal(childFilters, ft.Filters.Count);

            Console.WriteLine($"filter : {ft.Term} {ft.Operator} {ft.Field.GetValue(default)}");
        }

        [Fact]
        public void ParseFilterDateTimeField()
        {
            var utcNow = DateTime.UtcNow;
            var text = "$filter=DateOfBirth lt " + utcNow.ToString("o");
            const string ENTITY = "Employee";

            var ed = CreateEmployeeEntityDefinition();
            SourceRepository.AddOrReplace(ed);
            var parser = new OdataQueryParser();
            var query = parser.Parse(text, ENTITY);

            var dateOfBirthLtNow = query.Filters.SingleOrDefault(x => x.Term == "DateOfBirth");
            Assert.NotNull(dateOfBirthLtNow);
            Assert.Equal(Operator.Lt, dateOfBirthLtNow.Operator);
            Assert.IsType<ConstantField>(dateOfBirthLtNow.Field);
            Assert.Equal(utcNow.ToString("MM/dd/yyyy h:mm:ss tt +00:00"), dateOfBirthLtNow.Field.GetValue(default));
        }

        [Fact]
        public void ParseFilterComplexTypeProperty()
        {
            const string TEXT = "$filter=HomeAddress/City eq 'Kuala Lumpur'";
            const string ENTITY = "Employee";

            var ed = CreateEmployeeEntityDefinition();
            SourceRepository.AddOrReplace(ed);
            var parser = new OdataQueryParser();
            var query = parser.Parse(TEXT, ENTITY);

            var cityEqKualaLumpur = query.Filters.SingleOrDefault(x => x.Term == "City");
            Assert.NotNull(cityEqKualaLumpur);
            Assert.Equal(Operator.Eq, cityEqKualaLumpur.Operator);
            Assert.IsType<ConstantField>(cityEqKualaLumpur.Field);
            Assert.Equal("Kuala Lumpur", cityEqKualaLumpur.Field.GetValue(default));
        }
    }
}