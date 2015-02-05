using System;
using System.Threading.Tasks;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    public class DataContextTestFixture : StatementTestFixture
    {

        [Test]
        public async Task LoadOneAsync()
        {
            await AssertAsync<Task>(@"
            var patient;
            return context.loadOneAsync(""Patient"", ""Mrn eq '123'"")
                        .then(function(__temp0) {
                            logger.info(patient.Name());
                        });
            ",
                @"
                var patient = await context.LoadOneAsync<Patient>(x => x.Mrn == ""123"");
                logger.Info(patient.Name); 
            ");
        }
        [Test]
        public async Task LoadAsync()
        {
            await AssertAsync<Task>(@"
            var lo;
            return context.loadAsync(""Patient"", ""Mrn eq '123'"")
                        .then(function(__temp0) {
                            logger.info('Count : ' + lo.itemCollection.length);
                        });
            ",
                @"
                var lo = await context.LoadAsync<Patient>(x => x.Mrn == ""123"");
                logger.Info(""Count : "" + lo.ItemCollection.Count); 
            ");
        }

        [Test]
        public async Task SearchAsync()
        {
            const string QUERY = @"""{
    """"filtered"""" : {
        """"query"""" : {
            """"queryString"""" : {
                """"default_field"""" : """"message"""",
                """"query"""" : """"elasticsearch""""
            }
        },
        """"filter"""" : {
            """"bool"""" : {
                """"must"""" : {
                    """"term"""" : { """"tag"""" : """"wow"""" }
                },
                """"must_not"""" : {
                    """"range"""" : {
                        """"age"""" : { """"from"""" : 10, """"to"""" : 20 }
                    }
                },
                """"should"""" : [
                    {
                        """"term"""" : { """"tag"""" : """"sometag"""" }
                    },
                    {
                        """"term"""" : { """"tag"""" : """"sometagtag"""" }
                    }
                ]
            }
        }
    }
}""";

            const string CODE = @"
                var query = " + "@" + QUERY + @";
                var lo = await context.SearchAsync<Patient>(query);
                logger.Info(""Count : "" + lo.ItemCollection.Count); 
            ";
            Console.WriteLine(CODE);
            await AssertAsync<Task>(@"
            var lo;
            return context.searchAsync('Patient', JSON.stringify({""query-dsl"":""""}))
                        .then(function(__temp0) {
                            lo = __temp0;
                            logger.info(patient.Name());
                        });;
            ",
                CODE);
        }

        [Test]
        public async Task CountAsync()
        {
            await AssertAsync<Task>(@"
            var count;
            return context.getCountAsync(""Patient"", ""Age gt 25"")
                        .then(function(__temp0) {
                            count = __temp0;
                            logger.info('Count : ' + count);
                        });
            ",
                @"
                var count = await context.GetCountAsync<Patient>(x => x.Age > 25);
                logger.Info(""Count : "" + count); 
            ");
        }

        [Test]
        public async Task ListAsync()
        {
            await AssertAsync<Task>(@"
            var names;
            return context.getListAsync(""Patient"", ""Name"", ""Age gt 25"")
                        .then(function(__temp0) {
                            names = __temp0;
                            logger.info('Names ' + names.length);
                        });
            ",
                @"
                var names = await context.GetListAsync<Patient, string>(""Name"", x => x.Age > 25);
                logger.Info(""Names "" + names.Count); 
            ");
        }
        [Test]
        public async Task Scalar()
        {
            await AssertAsync<Task>(@"
            var name;
            return context.getScalarAsync(""Patient"", ""Name"", ""Mrn eq 'ABC123'"")
                        .then(function(__temp0) {
                            name = __temp0;
                            logger.info('Name : ' + name);
                        });
            ",
                @"
                var name = await context.GetScalarAsync<Patient, string>(x => x.Name,  x => x.Mrn == ""ABC123"");
                logger.Info(""Name : "" + name); 
            ");
        }
    }
}