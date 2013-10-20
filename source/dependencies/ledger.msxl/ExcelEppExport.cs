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
                                     Date = v.Date,
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

            var input = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ledger.xlsx");
            if (!File.Exists(input))
            {
                throw new InvalidOperationException(
                    "Please copy the 'ledger.xlsx' to the base directory"
                    , new FileNotFoundException("Cannot find the ledger template", input));
            }
            var template = new FileInfo(input);
            var pck = new ExcelPackage(template);
            var ws = pck.Workbook.Worksheets["0"];
            ws.Name = contract.Tenant.Name.ToUpper();

            //info after header//
            ws.Cells["B3"].Value = contract.Tenant.Name;
            ws.Cells["B4"].Value = contract.Tenant.IdSsmNo;
            ws.Cells["F3"].Value = contract.ReferenceNo;
            ws.Cells["F4"].Value = contract.StartDate;
            ws.Cells["F5"].Value = contract.EndDate;

            //address tenant//
            if (null == contract.Tenant.Address.UnitNo && null == contract.Tenant.Address.Block)
            {
                ws.Cells["B8"].Value = contract.Tenant.Address.UnitNo + contract.Tenant.Address.Block;
            }
            else
            {
                ws.Cells["B8"].Value = contract.Tenant.Address.Street;
            }

            ws.Cells["B9"].Value = contract.Tenant.Address.City + ", " + contract.Tenant.Address.Postcode;
            ws.Cells["B10"].Value = contract.Tenant.Address.State;
            ws.Cells["B11"].Value = contract.Tenant.Phone;

            //address space//
            ws.Cells["B16"].Value = contract.Space.BuildingName;
            if (null == contract.Space.Address.UnitNo && null == contract.Space.Address.Block)
            {
                ws.Cells["B17"].Value = contract.Space.Address.UnitNo + contract.Space.Address.Block;
            }
            else
            {
                ws.Cells["B17"].Value = contract.Space.Address.Street;
            }

            ws.Cells["B18"].Value = contract.Space.City + ", " + contract.Tenant.Address.Postcode;
            ws.Cells["B19"].Value = contract.Space.State;

            //deposit//
            ws.Cells["A25"].Value = contract.StartDate;
            ws.Cells["B25"].Value = contract.Space.RentalRate;
            ws.Cells["C25"].Value = contract.Space.RentalRate;

            //sewa//
            var i = 0;
            foreach (var e in sortedEntries)
            {
                ws.Cells[32 + i, 1].Value = e.Date.ToString("d");
                ws.Cells[32 + i, 2].Value = e.Description;
                ws.Cells[32 + i, 3].Value = e.DebitAmount;
                ws.Cells[32 + i, 4].Value = e.CreditAmount;
                ws.Cells[32 + i, 5].Value = e.Balance;
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
