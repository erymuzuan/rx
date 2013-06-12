using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bespoke.SphCommercialSpaces.Domain;
using OfficeOpenXml;

namespace Bespoke.SphCommercialSpace.LedgerMsxl
{
    public class ExcelEppExport : ILedgerExport
    {
        public string GenerateLedger(Contract contract, IEnumerable<Rent> rents, string filename)
        {

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
            ws.Cells["B12"].Value = contract.Tenant.PhoneNo;

            ws.Cells["A16"].Value = contract.CommercialSpace.LotName;
            ws.Cells["A17"].Value = contract.CommercialSpace.BuildingName;
            ws.Cells["A18"].Value = contract.CommercialSpace.City + ", " + contract.Tenant.Address.Postcode;
            ws.Cells["B19"].Value = contract.CommercialSpace.State;


            ws.Cells["F3"].Value = contract.ReferenceNo;
            ws.Cells["F4"].Value = contract.CommercialSpace.RentalRate;

            ws.Cells["F6"].Value = contract.StartDate;
            ws.Cells["F7"].Value = contract.EndDate;

            ws.Cells["A24"].Value = contract.StartDate;
            ws.Cells["B24"].Value = contract.CommercialSpace.RentalRate;
            ws.Cells["C24"].Value = contract.CommercialSpace.RentalRate;
            // ws.Cells["F24"].Value = contract.Deposit.ReceiptNo;

            var i = 0;
            foreach (var rt in rents
                .OrderBy(d => d.Year)
                .ThenBy(d => d.Month)
                .ThenBy(d => d.Quarter)
                .ThenBy(d => d.Half)
                )
            {

                ws.Cells[31 + i, 1].Value = string.Format("{0}{1}{2}-{3}", rt.Month, rt.Quarter, rt.Half, rt.Year);
                //ws.Cells[31 + i, 2].Value = rt.PaymentDistributionCollection.Sum(p => p.Amount);
                ws.Cells[31 + i, 3].Value = rt.Amount;
                //ws.Cells[31 + i, 4].Value = rt.Amount - rt.PaymentDistributionCollection.Sum(p => p.Amount);
                //ws.Cells[31 + i, 6].Value = string.Join(",", rt.PaymentDistributionCollection.Select(p => p.ReceiptNo).ToArray());
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
