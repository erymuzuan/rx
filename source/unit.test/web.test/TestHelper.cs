using System;
using FluentDateTime;
using OpenQA.Selenium;

namespace web.test
{
    public static class TestHelper
    {
        public static TestUser CreateSpaceAdmin()
        {
            var spaceAdminUser = new TestUser
            {
                UserName = "ruang-admin",
                FullName = "Ruang Admin",
                Email = "ruang.admin@bespoke.com.my",
                Department = "Test",
                Designation = "Space Admin Manager",
                Password = "abcad12334535",
                StartModule = "space.list",
                Roles = new[] { "can_add_space", "can_edit_space", "can_edit_space_template", "can_edit_building_template" }
            };
            return spaceAdminUser;
        }

        public static void CreateBuildingTemplate(IWebDriver driver, string templateName)
        {
            driver.NavigateToUrl("/#building.template.list", 2.Seconds())
                  .NavigateToUrl("/#/template.building-id.0/0", 4.Seconds());

            // add elements
            driver.Value("[name=Building-template-category]", templateName)
                  .Value("[name=Building-template-name]", templateName)
                  .Click("[id=template-isactive]")
                  .Value("[id=form-design-name]", templateName)
                  .Value("[id=form-design-description]", templateName)
                  ;

            //nama bangunan
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Nama bangunan")
                  .Value("[name=Path]", "Name");

            //descripsi bangunan - custom field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Paragrapah text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Deskripsi")
                  .Value("[name=Path]", "Descripsi") 
                  .Value("[name=Tooltip]", "Maklumat terperinci berkenaan bangunan")
                  .SelectOption("[name=Size]", "XL"); 
            
            //IsOversea bangunan - custom field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Checkboxes")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Bangunan Di Luar Negara")
                  .Value("[name=Path]", "IsOversea") 
                  .Value("[name=Tooltip]", "Bangunan di luar atau dalam negara"); 
            
            //Nama negara - jika bangunan di luar negara
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Select list")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Negara")
                  .Value("[name=Path]", "Country")
                  .Value("[name=Visible]", "CustomField('IsOversea')");

            //isikan nama negara
            driver
                  .Value(".input-combobox-value", "Singapura")
                  .Value(".input-combobox-caption", "Singapura") 
                  .Value(".input-combobox-value", "Indonesia",1)
                  .Value(".input-combobox-caption", "Indonesia", 1)
                  .Value(".input-combobox-value", "Brunei",2)
                  .Value(".input-combobox-caption", "Brunei", 2);

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: $root.addComboBoxOption");
            driver.Value(".input-combobox-value", "Thailand",3)
                  .Value(".input-combobox-caption", "Thailand", 3);

            //Tarikh Dibina - Custom Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Tarikh")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Tarikh Dibina")
                  .Value("[name=Path]", "DevelopmentDate"); 
            
            //Jumlah Kontraktor - Nombor Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Nombor")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Jumlah Kontraktor")
                  .Value("[name=Path]", "TotalContractor"); 
            
            //Email - Emel Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Emel")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Email")
                  .Value("[name=Path]", "Email");

            //Website - Website Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Website")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Website")
                  .Value("[name=Path]", "Website"); 
        

            //owner - Custom Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Nama Konsesi")
                  .Value("[name=Path]", "ConsessionName");

            // Lot No
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "No. Lot")
                  .Value("[name=Path]", "LotNo");

            // Size
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Saiz")
                  .Value("[name=Path]", "Size");

            // address
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Address")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Alamat")
                  .Value("[name=Path]", "Address");

            //// show map button
            //driver.ClickFirst("a", e => e.Text == "Add a field")
            //      .ClickFirst("a", e => e.Text == "Show map button")
            //      .ClickFirst("a", e => e.Text == "Fields settings")
            //      .Value("[name=Label]", "Tunjuk Peta Kawasan")
            //      .Value("[name=CssClass]", "btn btn-success")
            //      .Value("[name=Icon]", "icon-globe");

            //// Bilangan Tingkat
            //driver.ClickFirst("a", e => e.Text == "Add a field")
            //      .ClickFirst("a", e => e.Text == "Single line text")
            //      .ClickFirst("a", e => e.Text == "Fields settings")
            //      .Value("[name=Label]", "Bilangan Tingkat")
            //      .Value("[name=Path]", "Floors");

            //// floor collection
            //driver.ClickFirst("a", e => e.Text == "Add a field")
            //      .ClickFirst("a", e => e.Text == "Floors Table")
            //      .ClickFirst("a", e => e.Text == "Fields settings")
            //      .Value("[name=Label]", "Senarai Tingkat")
            //      .Value("[name=Path]", "FloorCollection");

            // blocks table
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Blocks Table")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Senarai Blok")
                  .Value("[name=Path]", "BlockCollection");

            //// custom list collection
            //driver.ClickFirst("a", e => e.Text == "Add a field");
            //driver.ClickFirst("a", e => e.Text == "List");
            //driver.ClickFirst("a", e => e.Text == "Fields settings");
            //driver.Value("[name=Label]", "Senarai Pegawai Bertugas");
            //      driver.Value("[name=Path]", "Pegawai Bertugas");

            //driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
            //    .Value("input[type=text].custom-list-name", "Nama Penuh")
            //    .SelectOption("select.custom-list-type", typeof(string).GetShortAssemblyQualifiedName(), 0, false);

            //driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField");
            //driver.Value("input[type=text].custom-list-name", "Umur", 1);
            //    driver.SelectOption("select.custom-list-type", "Int", 1);

            //driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
            //    .Value("input[type=text].custom-list-name", "Dob", 2)
            //    .SelectOption("select.custom-list-type", "DateTime", 2);

            //driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
            //    .Value("input[type=text].custom-list-name", "Tetap", 3)
            //    .SelectOption("select.custom-list-type", "Bool", 3);

            //driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
            //    .Value("input[type=text].custom-list-name", "Gaji", 4)
            //    .SelectOption("select.custom-list-type","Decimal", 4);

            //HTML - HTML Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "HTML")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=html-text]", "Sila isi semua maklumat sebelom simpan maklumat bangunan");

            //List - List Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "HTML")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=html-text]", "Sila isi semua maklumat sebelom simpan maklumat bangunan");

            driver.Click("#save-button")
                .Sleep(5.Seconds());

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#building.template.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
        }

        public static void CreateBuilding(IWebDriver driver, int templateId, string buildingName)
        {
            driver.NavigateToUrl(string.Format("/#/building.detail-templateid.{0}/{0}/0", templateId), 5.Seconds());

            driver
                .Sleep(5.Seconds(), "waiting maps scripts downloaded...")
                .Value("[name='Name']", buildingName)
                .Value("[name='Descripsi']", "Bangunan konsesi putrajaya holding mempunyai 1 blok")
                .Click("[name='IsOversea']")
                .Sleep(20.Milliseconds())
                .Value("[name='DevelopmentDate']", "14/03/1980")
                .Value("[name='TotalContractor']", "2")
                .Value("[name='Email']", "contractor@gmail.com")
                .Value("[name='Website']", "www.cidb.com.my")
                .Value("[name=ConsessionName]", "Putrajaya Holding")
                .Value("[name='LotNo']", "12-001")
                .Value("[name='Size']", "112991.02")
                .Value("[name='address.Street']", "Jalan Cempaka")
                .Value("[name='address.City']", "KB")
                .Value("[name='address.Postcode']", "15210")
                .SelectOption("[name='address.State']", "Kelantan");

            //driver.Click("[name='add-floor-button']")
            //.Sleep(200.Milliseconds(), "Add floor")
            //.Value(".input-floor-no", "G1")
            //.Value(".input-floor-name", "1st Floor")
            //.Value(".input-floor-size", "48500");

            //driver.Click("[name='add-floor-button']")
            //.Sleep(200.Milliseconds(), "Add floor")
            //.Value(".input-floor-no", "G2", 1)
            //.Value(".input-floor-name", "2nd Floor", 1)
            //.Value(".input-floor-size", "48500", 1);

            driver.Click("[name='add-block-button']")
                  .Sleep(200.Milliseconds(), "Add block")
                  .Value("input.input-block-name", "A")
                  .Value("input.input-block-description", "Block A")
                  .Value("input.input-block-size", "5000");

            driver.Click(".button-block-floor")
                  .Sleep(100.Milliseconds(), "Add floor")
                  .Value("input.input-floor-no", "1")
                  .Value("input.input-floor-name", "G")
                  .Value("input.input-floor-size", "1000");

            driver.Click("input[data-bind='click: okClick']")
                  .Sleep(TimeSpan.FromSeconds(1));

            driver.Click("#save-button")
            .Sleep(TimeSpan.FromSeconds(2));

            driver.NavigateToUrl("/#/building.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
        }

        public static void CreateBuildingLots(IWebDriver driver, int templateId, int buildingId, string blockName, string floorName)
        {
            var url = String.Format("/#/building.detail-templateid.{0}/{0}/{1}", templateId, buildingId);
            Console.WriteLine(url);

            url = String.Format("/#/lotdetail/{0}/{1}/{2}", buildingId, blockName, floorName);
            Console.WriteLine(url);
            driver.NavigateToUrl(url, 5.Seconds());

            driver.Click("#add-new-lot")
            .Value(".input-lot-name", "Lot 1")
            .Value(".input-lot-size", "2500")
            .Value(".input-lot-usage", "Cafeteria")
            .Sleep(200.Milliseconds());

            driver.Click("#add-new-lot");
            driver.Value(".input-lot-name", "Lot 2", 1);
            driver.Value(".input-lot-size", "2300", 1);
            driver.Value(".input-lot-usage", "Dobi", 1)
            .Sleep(200.Milliseconds());

            driver
            .Click("#add-new-lot")
            .Value(".input-lot-name", "Lot 3", 2)
            .Value(".input-lot-size", "500", 2)
            .Value(".input-lot-usage", "ATM", 2)
            .Sleep(TimeSpan.FromSeconds(2));

            driver.Click("#save-button");

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#/building.list");
            driver.NavigateToUrl(String.Format("/#/lotdetail/{0}/1st Floor", buildingId));
        }
    }
}
