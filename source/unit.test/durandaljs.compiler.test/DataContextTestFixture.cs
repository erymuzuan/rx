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
            return context.loadOneAsync('Patient', 'Mrn eq \'123\')
                        .then(function(__temp0) {
                            logger.info(patient.Name());
                        });;
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
            var patients;
            return context.loadAsync('Patient', 'Mrn eq \'123\')
                        .then(function(__temp0) {
                            logger.info(patient.Name());
                        });;
            ",
                @"
                var patients = await context.LoadAsync<Patient>(x => x.Mrn == ""123"");
                logger.Info(patients.ItemCollection.Count); 
            ");
        }

        [Test]
        public async Task SearchAsync()
        {
            await AssertAsync<Task>(@"
            var patients;
            return context.loadAsync('Patient', 'Mrn eq \'123\')
                        .then(function(__temp0) {
                            logger.info(patient.Name());
                        });;
            ",
                @"
                var patients = await context.SearchAsync<Patient>(""{query_dsl:}"");
                logger.Info(patients.ItemCollection.Count); 
            ");
        }

        [Test]
        public async Task CountAsync()
        {
            await AssertAsync<Task>(@"
            var patients;
            return context.loadAsync('Patient', 'Mrn eq \'123\')
                        .then(function(__temp0) {
                            logger.info(patient.Name());
                        });;
            ",
                @"
                var patients = await context.CountAsync<Patient>(""{query_dsl:}"");
                logger.Info(patients.ItemCollection.Count); 
            ");
        }

        [Test]
        public async Task ListAsync()
        {
            await AssertAsync<Task>(@"
            var patients;
            return context.loadAsync('Patient', 'Mrn eq \'123\')
                        .then(function(__temp0) {
                            logger.info(patient.Name());
                        });;
            ",
                @"
                var patients = await context.CountAsync<Patient>(""Name"", ""{query_dsl:}"");
                logger.Info(patients.ItemCollection.Count); 
            ");
        }
    }
}