﻿using System;
using System.Data.SqlClient;
using NUnit.Framework;
using FluentDateTime;

namespace web.test
{
    [TestFixture]
// ReSharper disable InconsistentNaming
// ReSharper disable once InconsistentNaming
    public class _100_BuildingTest : BrowserTest
    {
        public const string BUILDING_NAME = "Bangunan Komersil Di Putrajaya (UJIAN)";
        public const string BUILDING_TEMPLATE_NAME = "Bangunan Komersil";


        private TestUser m_buildingAdmin;

        [SetUp]
        public void Init()
        {
            m_buildingAdmin = new TestUser
            {
                UserName = "buildingadmin",
                FullName = "Building Admin",
                Email = "buildingadmin@bespoke.com.my",
                Department = "Test",
                Designation = "Boss",
                Password = "abcad12334535",
                StartModule = "building.list",
                Roles = new[] { "can_add_space", "can_edit_building_template" }
            };
            this.AddUser(m_buildingAdmin);
        }

        [Test]
        public void AddBuildingAndNavigateToLots()
        {
            _001_AddBuildingTemplate();
            _002_AddBuilding();
            _003_AddLots();
        }

        [Test]
        // ReSharper disable  InconsistentNaming
        public void _001_AddBuildingTemplate()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[BuildingTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", BUILDING_TEMPLATE_NAME));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([BuildingTemplateId]) FROM [Sph].[BuildingTemplate]");


            var driver = this.InitiateDriver();
            driver.Login(m_buildingAdmin);
            driver.NavigateToUrl("/#building.template.list", 2.Seconds())
                  .NavigateToUrl("/#/template.building-id.0/0", 4.Seconds());

            // add elements
            driver.Value("[name=Building-template-category]", BUILDING_TEMPLATE_NAME)
                  .Value("[name=Building-template-name]", BUILDING_TEMPLATE_NAME)
                  .Click("[id=template-isactive]")
                  .Value("[id=form-design-name]", BUILDING_TEMPLATE_NAME)
                  .Value("[id=form-design-description]", BUILDING_TEMPLATE_NAME)
                  ;

            //nama bangunan
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Nama bangunan")
                  .Value("[name=Path]", "Name");

            //owner - Custom Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Nama Konsesi")
                  .Value("[name=Path]", "ConsessionName");

            // Lot NO
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

            // show map button
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Show map button")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Tunjuk Peta Kawasan")
                  .Value("[name=CssClass]", "btn btn-success")
                  .Value("[name=Icon]", "icon-globe")
                  ;

            // Bilangan Tingkat
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Bilangan Tingkat")
                  .Value("[name=Path]", "Floors");
            // floor collection
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Floors Table")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Senarai Tingkat")
                  .Value("[name=Path]", "FloorCollection")
                  ;

            // custom list collection
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "List")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Senarai Pegawai Bertugas")
                  .Value("[name=Path]", "Pegawai Bertugas")
                  ;

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                .Value("input[type=text].custom-list-name", "Nama Penuh")
                .SelectOption("select.custom-list-type", typeof(string).GetShortAssemblyQualifiedName(), 0, false);

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                .Value("input[type=text].custom-list-name", "Umur", 1)
                .SelectOption("select.custom-list-type", typeof(int).AssemblyQualifiedName, 1, false);

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                .Value("input[type=text].custom-list-name", "Dob", 2)
                .SelectOption("select.custom-list-type", typeof(DateTime).AssemblyQualifiedName, 2, false);

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                .Value("input[type=text].custom-list-name", "Tetap", 3)
                .SelectOption("select.custom-list-type", typeof(bool).AssemblyQualifiedName, 3, false);

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                .Value("input[type=text].custom-list-name", "Gaji", 4)
                .SelectOption("select.custom-list-type", typeof(decimal).AssemblyQualifiedName, 4, false);

            driver.Click("#save-button")
                .Sleep(5.Seconds());


            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([BuildingTemplateId]) FROM [Sph].[BuildingTemplate]");
            Assert.IsTrue(max < latest);

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#building.template.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }

        [Test]
        public void _002_AddBuilding()
        {

            this.ExecuteNonQuery("DELETE FROM [Sph].[Building] WHERE [Name] =@Name", new SqlParameter("@Name", BUILDING_NAME));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([BuildingId]) FROM [Sph].[Building]");
            var templateId = this.GetDatabaseScalarValue<int>("SELECT [BuildingTemplateId] FROM [Sph].[BuildingTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", BUILDING_TEMPLATE_NAME));


            var driver = this.InitiateDriver();
            driver.Login(m_buildingAdmin);
            driver.NavigateToUrl("/#/building.list", 2.Seconds());

            driver.NavigateToUrl(String.Format("/#/building.detail-templateid.{0}/{0}/0", templateId), 5.Seconds());

            driver.Value("[name='ConsessionName']", "Putrajaya Holding");

            driver
                .Value("[name='Name']", BUILDING_NAME)
                .Value("[name='LotNo']", "12-001")
                .Value("[name='Size']", "112991.02")
                .Value("[name='address.Street']", "Jalan Cempaka")
                .Value("[name='address.City']", "KB")
                .Value("[name='address.Postcode']", "15210")
                .SelectOption("[name='address.State']", "Kelantan")
                .Value("[name='Floors']", "2");


            driver.Click("[name='add-floor-button']")
            .Sleep(200.Milliseconds(), "Add floor")
            .Value(".input-floor-no", "G1")
            .Value(".input-floor-name", "1st Floor")
            .Value(".input-floor-size", "48500");



            driver.Click("[name='add-floor-button']")
            .Sleep(200.Milliseconds(), "Add floor")
            .Value(".input-floor-no", "G2", 1)
            .Value(".input-floor-name", "2nd Floor", 1)
            .Value(".input-floor-size", "48500", 1);



            driver.Click("#save-button")
            .Sleep(TimeSpan.FromSeconds(2))
            ;
            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([BuildingId]) FROM [Sph].[Building]");
            Assert.IsTrue(max < latest);

            driver.NavigateToUrl("/#/building.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");

            driver.Quit();
        }

        [Test]
        public void _003_AddLots()
        {
            var id = this.GetDatabaseScalarValue<int>("SELECT [BuildingId] FROM [Sph].[Building] WHERE [Name] =@Name", new SqlParameter("@Name", BUILDING_NAME));
            var templateId = this.GetDatabaseScalarValue<int>("SELECT [BuildingTemplateId] FROM [Sph].[BuildingTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", BUILDING_TEMPLATE_NAME));

            var driver = this.InitiateDriver();
            driver.Login(m_buildingAdmin)
                .NavigateToUrl(String.Format("/#/building.detail-templateid.{0}/{0}/{1}", templateId, id),2.Seconds());

            driver
                .NavigateToUrl(String.Format("/#/lotdetail/{0}/1st Floor", id), 1.Seconds());


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
            driver.NavigateToUrl(String.Format("/#/lotdetail/{0}/1st Floor", id));
            driver.Sleep(TimeSpan.FromSeconds(3), "See the result");

            driver.Quit();
        }
    }
}
