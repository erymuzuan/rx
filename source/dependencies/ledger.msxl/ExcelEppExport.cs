using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bespoke.Sph.Domain;
using OfficeOpenXml;

namespace Bespoke.Sph.LedgerMsxl
{
    public class ExcelEppExport : ILedgerExport
    {
        public string GenerateLedger(Contract contract, IEnumerable<Invoice> invoices, IEnumerable<Rebate> rebates, IEnumerable<Payment> payments,
                                    string filename)
        {


            var entries = new ObjectCollection<JournalEntry>();
            var invoiceEntries = from v in invoices
                                 select new JournalEntry
                                     {
                                         Date = v.Date,
                                         CreditAmount = v.Amount,
                                         Description = string.Format("Invoice : {0}", v.InvoiceNo)
                                     };
            var rebateEntries = from v in rebates
                                 select new JournalEntry
                                     {
                                         Date = v.StartDate,
                                         DebitAmount = v.Amount,
                                         Description = string.Format("Rebate : {0}", v.ContractNo)
                                     };
            var paymentEntries = from v in payments
                                 select new JournalEntry
                                     {
                                         Date =v.Date,
                                         DebitAmount = v.Amount,
                                         Description = string.Format("Bayaran : {0}", v.ContractNo)
                                     };

            entries.AddRange(invoiceEntries);
            entries.AddRange(rebateEntries);
            entries.AddRange(paymentEntries);

            var sortedEntries = entries.OrderBy(e => e.Date);
            var balance = 0m;
            foreach (var e in sortedEntries)
            {
                e.Balance = balance + e.DebitAmount - e.CreditAmount;
                balance = e.Balance;
            }

            var input = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ruang.komersil.utility.ledger.xlsx");
            if (!File.Exists(input))
            {
                throw new InvalidOperationException(
                    "Please copy the ruang.komersil.utility.ledger.xlsx to the base directory"
                    , new FileNotFoundException("Cannot find the ledger template", input));
            }
            var template = new FileInfo(input);
            var pck = new ExcelPackage(template);
            var ws = pck.Workbook.Worksheets["0"];
            ws.Name = contract.Tenant.Name.ToUpper();

            ws.Cells["B3"].Value = contract.Tenant.Name;
            ws.Cells["B4"].Value = contract.Tenant.IdSsmNo;

            ws.Cells["A9"].Value = contract.Tenant.Address.Street;
            ws.Cells["A11"].Value = contract.Tenant.Address.City + ", " + contract.Tenant.Address.Postcode;
            ws.Cells["B12"].Value = contract.Tenant.Phone;

            ws.Cells["A16"].Value = contract.Space.LotName;
            ws.Cells["A17"].Value = contract.Space.BuildingName;
            ws.Cells["A18"].Value = contract.Space.City + ", " + contract.Tenant.Address.Postcode;
            ws.Cells["B19"].Value = contract.Space.State;


            ws.Cells["F3"].Value = contract.ReferenceNo;
            ws.Cells["F4"].Value = contract.Space.RentalRate;

            ws.Cells["F6"].Value = contract.StartDate;
            ws.Cells["F7"].Value = contract.EndDate;

            ws.Cells["A24"].Value = contract.StartDate;
            ws.Cells["B24"].Value = contract.Space.RentalRate;
            ws.Cells["C24"].Value = contract.Space.RentalRate;

            var i = 0;
            foreach (var e in sortedEntries)
            {
                ws.Cells[31 + i, 1].Value = e.Date.ToString("d");
                ws.Cells[31 + i, 2].Value = e.Description;
                ws.Cells[31 + i, 3].Value = e.DebitAmount;
                ws.Cells[31 + i, 4].Value = e.CreditAmount;
                ws.Cells[31 + i, 5].Value = e.Balance;
                i++;
            }
            var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var outputpath = Path.Combine(desktop, filename);
            using (var stream = new FileStream(outputpath, FileMode.Create))
            {
                pck.SaveAs(stream);
            }
            return outputpath;
        }

       
    }
}
