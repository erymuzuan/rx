using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.RabbitMqPublisher;
using Bespoke.Sph.SqlRepository;
using Bespoke.SphCommercialSpaces.Domain;
using Bespoke.SphCommercialSpaces.Domain.QueryProviders;
using Contract = Bespoke.SphCommercialSpaces.Domain.Contract;
using Rent = Bespoke.SphCommercialSpaces.Domain.Rent;

namespace Bespoke.Scheduler.Sph.Rental
{
    public class RentalScheduler
    {
        static void Main()
        {
            Console.WriteLine("Generate rental invoice for today");
            RegisterServices();
            var date = DateTime.Now;
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
               .Wait(TimeSpan.FromMinutes(5));

        }

        private static async Task CreateInvoices(DateTime date)
        {
            var context = new SphDataContext();
            var contractQuery = context.Contracts.Where(c => c.EndDate >= date);
            var contractLoadOperation = await context.LoadAsync(contractQuery);
            var contracts = contractLoadOperation.ItemCollection;
            var rentCollection = new List<Rent>();
            foreach (var c in contracts)
            {
                var c1 = c;
                decimal? accumulatedAccrued = decimal.Zero;
                decimal? accrued = decimal.Zero;
                var prvMonthRent = await context.LoadOneAsync<Rent>(r => r.Month == (date.Month - 1) && r.Year == date.Year && r.ContractId == c1.ContractId && r.TenantId == c1.Tenant.TenantId);
                if (null != prvMonthRent)
                {
                    accumulatedAccrued = prvMonthRent.AccumulatedAccrued + c1.CommercialSpace.RentalRate - prvMonthRent.TotalPayment;
                    accrued = prvMonthRent.AccumulatedAccrued;
                }
                var rent = new Rent
                            {
                                Date = date,
                                Month = date.Month,
                                Year = date.Year,
                                Amount = c1.CommercialSpace.RentalRate,
                                TenantId = c1.Tenant.TenantId,
                                ContractId = c1.ContractId,
                                ContractNo = c1.ReferenceNo,
                                AccumulatedAccrued = accumulatedAccrued,
                                Accrued = accrued,
                                InvoiceNo = string.Format("{0}/{1:MMyyyy}", c1.ReferenceNo, date),
                                IsPaid = false,
                                Type = "Rental",
                                Tenant = c1.Tenant
                            };
                rentCollection.Add(rent);
            }


            using (var session = context.OpenSession())
            {
                session.Attach(rentCollection.Cast<Entity>().ToArray());
                await session.SubmitChanges();
            }
        }

        private static void RegisterServices()
        {
            var broker = new DefaultBrokerConnection
            {
                Host = "localhost",
                VirtualHost = "i90009000",
                Password = "guest",
                Username = "guest",
                Port = 5672
            };
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
            ObjectBuilder.AddCacheList<IEntityChangePublisher>(new ChangePublisherClient(broker));
            ObjectBuilder.AddCacheList<IPagingTranslator>(new Sql2008PagingTranslator());
            ObjectBuilder.AddCacheList<IRepository<Contract>>(new SqlRepository<Contract>(conn));
            ObjectBuilder.AddCacheList<IRepository<Rent>>(new SqlRepository<Rent>(conn));
            ObjectBuilder.AddCacheList<IRepository<Tenant>>(new SqlRepository<Tenant>(conn));
        }
    }
}
