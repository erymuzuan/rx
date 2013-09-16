using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Bespoke.Sph.Domain;
using OfficeOpenXml;

namespace Bespoke.SphCommercialSpace.LedgerMsxl
{
    public class ExcelEppExport : IWorkOrderExport
    {
        public string GenerateWorkOrder(Maintenance maintenance, string filename)
        {

            var input = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ruang.komersil.senggara.arahankerja.xlsx");
            if (!File.Exists(input))
            {
                throw new InvalidOperationException(
                    "Please copy the ruang.komersil.senggara.arahankerja.xlsx to the base directory"
                    , new FileNotFoundException("Cannot find the work order template", input));
            }
            var template = new FileInfo(input);
            var pck = new ExcelPackage(template);
            var ws = pck.Workbook.Worksheets["0"];
            ws.Name = maintenance.Officer.ToUpper();

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
