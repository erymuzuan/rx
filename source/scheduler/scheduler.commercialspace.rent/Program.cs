using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.SqlRepository;
using Bespoke.SphCommercialSpaces.Domain;
using Bespoke.SphCommercialSpaces.Domain.QueryProviders;
using Contract = Bespoke.SphCommercialSpaces.Domain.Contract;

namespace scheduler.commercialspace.rent
{
    class Program
    {
        static void Main(string[] args)
        {
            RegisterServices();
            var date = DateTime.Today.AddMonths(1);
            var dateArg = args.SingleOrDefault(a => a.Contains("/date:"));
            if (null != dateArg)
            {
                DateTime.TryParse(dateArg.Replace("/date:", string.Empty), out date);
            }

            const int minute = 5;
            CreateInvoices(date)
                .ContinueWith(
                    _ =>
                    {
                        if (_.IsFaulted)
                        {
                            Console.WriteLine("*****************ERROR*************");
                            Console.WriteLine(_.Exception);
                        }
                        else
                        {
                            Console.WriteLine("Schedule pembayaran berjaya dijana");
                        }
                    })
                .Wait(TimeSpan.FromMinutes(2));


            if (args.Any(a => a.Contains("/debug")))
            {
                Console.Write("Press [ENTER] to exit");
                Console.ReadLine();
            }
        }

        private static async Task CreateInvoices(DateTime date)
        {
            var context = new SphDataContext();
            var contractQuery = context.Contracts.Where(c => c.EndDate >= date);
            var contractLoadOperationTask = context.LoadAsync(contractQuery);

            var rentQuery = context.Rents
                .Where(r => r.Month == date.Month)
                .Where(r => r.Year == date.Year);
            var rentLoadOperationTask = context.LoadAsync(rentQuery);

            await Task.WhenAll(contractLoadOperationTask, rentLoadOperationTask);
            var contracts = (await contractLoadOperationTask).ItemCollection;
            var rents = (await rentLoadOperationTask).ItemCollection;

            var billableMonthlyContracts = (from ctr in contracts
                                            where
                                            ctr.CommercialSpace.RentalType == "Monthly"
                                            && !rents.Any(r =>
                                                               r.ContractId == ctr.ContractId
                                                               && r.Month == date.Month
                                                               && r.Year == date.Year
                                                       )
                                            select ctr).ToList();

            var billableQuarterlyContracts = (from ctr in contracts
                                              where
                                              ctr.CommercialSpace.RentalType == "Quarterly"
                                              && !rents.Any(r =>
                                                                 r.ContractId == ctr.ContractId
                                                                 && r.Quarter == "Q1"
                                                                 && r.Year == date.Year
                                                         )
                                              select ctr).ToList();

            var billableHalfYearContracts = (from ctr in contracts
                                             where
                                             ctr.CommercialSpace.RentalType == "Half Year"
                                             && !rents.Any(r =>
                                                                r.ContractId == ctr.ContractId
                                                                && r.Half == "H1"
                                                                && r.Year == date.Year
                                                        )
                                             select ctr).ToList();

            var billableContracts =
                billableMonthlyContracts.Concat(billableHalfYearContracts).Concat(billableQuarterlyContracts)
                .ToArray();

            var date1 = date;
            var invoicesTasks = from ctr in billableContracts
                                select ctr.CreateInvoice(date1);
            using (var session = context.OpenSession())
            {
                foreach (var inv in invoicesTasks)
                {
                    session.Attach(await inv);
                }
                await session.SubmitChanges();
            }

        }

        private static void RegisterServices()
        {
            const string conn = "Data Source=.\\katmai;Initial Catalog=Sph;Integrated Security=True;MultipleActiveResultSets=True";
            var cul = CultureInfo.CreateSpecificCulture("ms-MY");
            cul.DateTimeFormat.ShortDatePattern = "d/M/yyyy";
            cul.DateTimeFormat.ShortestDayNames = new[] { "Ahd", "Isn", "Sel", "Rab", "Kha", "Jum", "Sab" };
            cul.DateTimeFormat.ShortTimePattern = "h:m tt";
            cul.DateTimeFormat.LongDatePattern = "dd MMM yyyy";
            cul.DateTimeFormat.ShortTimePattern = "HH:mm";
            cul.DateTimeFormat.LongTimePattern = "hh:mm tt";

            cul.NumberFormat.CurrencyDecimalDigits = 2;
            cul.NumberFormat.CurrencySymbol = "RM ";
            cul.NumberFormat.CurrencyDecimalSeparator = ".";
            cul.NumberFormat.CurrencyGroupSeparator = " ";
            cul.NumberFormat.NumberDecimalSeparator = ".";
            cul.NumberFormat.NumberDecimalDigits = 2;

            Thread.CurrentThread.CurrentCulture = cul;
            Thread.CurrentThread.CurrentUICulture = cul;

            ObjectBuilder.AddCacheList<QueryProvider>(new SqlQueryProvider());
            ObjectBuilder.AddCacheList<IPersistence>(new SqlPersistence(conn));
            ObjectBuilder.AddCacheList<IRepository<Contract>>(new SqlRepository<Contract>(conn));
            ObjectBuilder.AddCacheList<IRepository<Rent>>(new SqlRepository<Rent>(conn));
        }
    }

      
}
