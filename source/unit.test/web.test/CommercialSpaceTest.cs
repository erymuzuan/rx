using System;
using System.Data.SqlClient;
using FluentDateTime;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace web.test
{
    [TestFixture]
    public class CommercialSpaceTest : BrowserTest
    {
        public const string CS_REGISTRATION_NO = "BSPK/999999";
        public const string CS_TEMPLATE_NAME = "Cafeteria";

        [Test]
        public void AddCsTemplateAndNewCs()
        {
            _001_AddCsTemplate();
            _002_AddNewCs();
        }

        [Test]
        public void _001_AddCsTemplate()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[CommercialSpaceTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", CS_TEMPLATE_NAME));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([CommercialSpaceTemplateId]) FROM [Sph].[CommercialSpaceTemplate]");

            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + "/Account/Login");
            driver.Login("ruzzaima");

            driver.NavigateToUrl("/#commercialspace.template.list")
                   .NavigateToUrl("/#/template.commercialspace-id.0/0")
                   .Sleep(2.Seconds());

            // add elements
            driver.Value("[name=CommercialSpace-template-category]", CS_TEMPLATE_NAME)
                  .Value("[name=CommercialSpace-template-name]", CS_TEMPLATE_NAME)
                  .Click("[id=template-isactive]")
                  .Value("[id=form-design-name]", CS_TEMPLATE_NAME)
                  .Value("[id=form-design-description]", CS_TEMPLATE_NAME)
                  ;

            //No Daftar
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "No Daftar")
                  .Value("[name=Path]", "RegistrationNo");


            // Bangunan
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Building")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Nama Bangunan")
                  .Value("[name=Path]", "BuildingName");

            // No Lot
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Lot")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Lot")
                  .Value("[name=Path]", "LotName");

            // Nama Cafe
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Nama Cafe")
                  .Value("[name=Path]", "Cafe Name");

            // alamat
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Address")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Alamat")
                  .Value("[name=Path]", "Address")
                  ;

            // sewa
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Sewa dicadangkan")
                  .Value("[name=Path]", "RentalRate");

            // contact
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Pegawai Untuk Dihubungi")
                  .Value("[name=Path]", "ContactPerson")
                  ;

            // IsAvailable
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Checkboxes")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Permohonan Dibuka")
                  .Value("[name=Path]", "IsAvailable")
                  ;

            // IsOnline
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Checkboxes")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Permohonan Online")
                  .Value("[name=Path]", "IsOnline")
                  ;

            driver.Click("#save-button");

            driver.Sleep(TimeSpan.FromSeconds(3));


            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([CommercialSpaceTemplateId]) FROM [Sph].[CommercialSpaceTemplate]");
            Assert.IsTrue(max < latest);

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#commercialspace.template.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }

        [Test]
        public void _002_AddNewCs()
        {

            this.ExecuteNonQuery("DELETE FROM [Sph].[CommercialSpace] WHERE [RegistrationNo] =@No", new SqlParameter("@No", CS_REGISTRATION_NO));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([CommercialSpaceId]) FROM [Sph].[CommercialSpace]");
            var templateId = this.GetDatabaseScalarValue<int>("SELECT [CommercialSpaceTemplateId] FROM [Sph].[CommercialSpaceTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", CS_TEMPLATE_NAME));

            var building =this.GetDatabaseScalarValue<int>("SELECT COUNT(*) FROM [Sph].[Building] WHERE [Name] = @Name",
                    new SqlParameter("@Name", BuildingTest.BUILDING_NAME));

            Assert.AreEqual(1, building, "You'll need to run the AddBuildingTest");

            var driver = this.InitiateDriver();
            driver.NavigateToUrl("/Account/Login",2.Seconds());
            driver.Login("ruzzaima");
            driver.NavigateToUrl("/#/commercialspace");
            driver.NavigateToUrl(String.Format("/#/commercialspace.detail-templateid.{0}/{0}/0/-/0", templateId), 2.Seconds());
            driver
                .Value("[name='RegistrationNo']", CS_REGISTRATION_NO)
                .Click("#select-lot-button")
                .Sleep(TimeSpan.FromSeconds(2))
                .SelectOption("[name='selectedBuilding']", BuildingTest.BUILDING_NAME)
                .Sleep(TimeSpan.FromSeconds(2))
                .SelectOption("[name='selectedFloor']", "Ground Floor")
                .Sleep(TimeSpan.FromSeconds(2))
                .SelectOption("[name='selectedLots']", "Lot 1")
                .Click("#add-lot-button")
                .Value("[name='CustomFieldValueCollection()[0].Value']", "Cafe ABC")
                .Value("[name='address.Street']", "Jalan Permata")
                .Value("[name='address.City']", "Putrajaya")
                .Value("[name='address.Postcode']", "62502")
                .Value("[name='address.State']", "Selangor")
                .Value("[name='RentalRate']", "2500")
                .Value("[name='ContactPerson']", "Mohd Razali")
                .Click("[name='IsOnline']")
                .Click("[name='IsAvailable']")
                .Click("[value='1']")
                .Click("#save-button")
                .Sleep(TimeSpan.FromSeconds(2));


            var id = this.GetDatabaseScalarValue<int>("SELECT [CommercialSpaceId] FROM [Sph].[CommercialSpace] WHERE [RegistrationNo] =@No", new SqlParameter("@No", CS_REGISTRATION_NO));
            Assert.IsTrue(max < id);

            driver.NavigateToUrl("/#/commercialspace");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }
    }
}
