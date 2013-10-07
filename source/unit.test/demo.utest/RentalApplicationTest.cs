using System;
using System.Data.SqlClient;
using FluentDateTime;
using NUnit.Framework;

namespace web.test
{
    [TestFixture]
    public class RentalApplicationTest : BrowserTest
    {
        private TestUser m_appTemplateAdmin;
        public const string RA_COMPANYREGISTRATION_NO = "001390153-U";
        public const string RA_IC_NO = "800212-02-9651";
        public const string APP_TEMPLATE_NAME = "Permohonan Baru";
        public const string CS_REGISTRATION_NO = "BSPK/130925001";

        [SetUp]
        public void SetUp()
        {
            //can_edit_application_template

            m_appTemplateAdmin = new TestUser
            {
                UserName = "app_template_admin",
                FullName = "App Template Admin",
                Email = "app_template_admin@bespoke.com.my",
                Department = "Unit Hartanah",
                Designation = "Pegawai Permohonan",
                Password = "123456",
                StartModule = "application.template.list",
                Roles = new[] { "can_edit_application_template" }
            };
            this.AddUser(m_appTemplateAdmin);
        }

        [Test]
        public void SubmitApplicationFlowTest()
        {
            _001_AddApplicationTemplate();
            _002_SubmitNewIndividualRentalApplication();
            _003_SubmitNewCompanyRentalApplication();
        }

        [Test]
        // ReSharper disable InconsistentNaming
        public void _001_AddApplicationTemplate()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[ApplicationTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", APP_TEMPLATE_NAME));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([ApplicationTemplateId]) FROM [Sph].[ApplicationTemplate]");


            var driver = this.InitiateDriver();
            driver.Login(m_appTemplateAdmin);
            driver.NavigateToUrl("/#application.template.list", 2.Seconds())
                  .NavigateToUrl("/#/template.application-id.0/0", 3.Seconds());

            // add elements
            driver.Value("[name=RentalApplication-template-category]", APP_TEMPLATE_NAME)
                  .Value("[name=RentalApplication-template-name]", APP_TEMPLATE_NAME)
                  .Click("[id=template-isactive]")
                  .Value("[id=form-design-name]", APP_TEMPLATE_NAME)
                  .Value("[id=form-design-description]", APP_TEMPLATE_NAME)
                  ;

            //IsCompany
            driver.ClickFirst("a", e => e.Text == "Add a field")
                   .ClickFirst("a", e => e.Text == "Checkboxes")
                   .ClickFirst("a", e => e.Text == "Fields settings")
                   .Value("[name=Label]", "Permohonan Syarikat")
                   .Value("[name=Path]", "IsCompany");

            //Company Name
            driver.ClickFirst("a", e => e.Text == "Add a field")
                   .ClickFirst("a", e => e.Text == "Single line text")
                   .ClickFirst("a", e => e.Text == "Fields settings")
                   .Value("[name=Label]", "Nama Syarikat")
                   .Value("[name=Path]", "CompanyName")
                   .Value("[name=Visible]", "IsCompany");

            //Company SSM No
            driver.ClickFirst("a", e => e.Text == "Add a field")
                   .ClickFirst("a", e => e.Text == "Single line text")
                   .ClickFirst("a", e => e.Text == "Fields settings")
                   .Value("[name=Label]", "SSM No")
                   .Value("[name=Path]", "CompanyRegistrationNo")
                   .Value("[name=Visible]", "IsCompany");

            //Header
            driver.ClickFirst("a", e => e.Text == "Add a field")
                   .ClickFirst("a", e => e.Text == "Section")
                   .ClickFirst("a", e => e.Text == "Fields settings")
                   .Value("[name=Label]", "Section")
                   .Value("[name=Visible]", "IsCompany");

            //contact
            driver.ClickFirst("a", e => e.Text == "Add a field")
                   .ClickFirst("a", e => e.Text == "Contact");

            // address
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Address")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Alamat")
                  .Value("[name=Path]", "Address");

            driver.Click("#save-button");

            driver.Sleep(TimeSpan.FromSeconds(3));


            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([ApplicationTemplateId]) FROM [Sph].[ApplicationTemplate]");
            Assert.IsTrue(max < latest);

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#application.template.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }


        [Test]
        // ReSharper disable once InconsistentNaming
        public void _002_SubmitNewIndividualRentalApplication()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[RentalApplication] WHERE [ContactIcNo] =@No", new SqlParameter("@No", RA_IC_NO));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([RentalApplicationId]) FROM [Sph].[RentalApplication]");
            var templateId = this.GetDatabaseScalarValue<int>("SELECT [ApplicationTemplateId] FROM [Sph].[ApplicationTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", APP_TEMPLATE_NAME));
            var spaceId = this.GetDatabaseScalarValue<int>("SELECT [SpaceId] FROM [Sph].[Space] WHERE [RegistrationNo] =@No", new SqlParameter("@No", CS_REGISTRATION_NO));

            var driver = this.InitiateDriver();
            driver
                .NavigateToUrl("/#/rentalapplication.selectspace", 3.Seconds());

            driver
                .NavigateToUrl(string.Format("/#/application.detail-templateid.{0}/{1}", templateId, spaceId), 3.Seconds());
            driver
                .Value("[name='Name']", "WAN HUDA BIN WAN ALI")
                .Value("[name='IcNo']", RA_IC_NO)
                .Value("[name='OfficeNo']", "013-6312237")
                .Value("[name='MobileNo']", "013-6312237")
                .Value("[name='Email']", "huda@hotmail.com")
                .Value("[name='address.Street']", "Jalan SS20/27")
                .Value("[name='address.City']", "Petaling Jaya")
                .Value("[name='address.Postcode']", "47300")
                .SelectOption("[name='address.State']", "Selangor");

            //add-on
            driver.ClickFirst("a", e => e.Text == "Fasiliti")
                .Click(".btn-add-feature")
                .SelectOption(".input-feature-available-quantity", "2",1)
                .Click(".btn-add-feature", 1)
                .Click(".btn-add-feature", 2)
                .Click("#btn-remove-feature", 2)
                ;
            driver.ClickFirst("button", e => e.Text == "Hantar Permohonan")
           .Sleep(5.Seconds());


            var current = this.GetDatabaseScalarValue<int>("SELECT MAX([RentalApplicationId]) FROM [Sph].[RentalApplication]");
            Assert.IsTrue(max < current);
            driver.Sleep(5.Seconds(), "See the result").Quit();
        }

        [Test]
        // ReSharper disable once InconsistentNaming
        public void _003_SubmitNewCompanyRentalApplication()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[RentalApplication] WHERE [CompanyRegistrationNo] =@No", new SqlParameter("@No", RA_COMPANYREGISTRATION_NO));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([RentalApplicationId]) FROM [Sph].[RentalApplication]");
            var templateId = this.GetDatabaseScalarValue<int>("SELECT [ApplicationTemplateId] FROM [Sph].[ApplicationTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", APP_TEMPLATE_NAME));
            var csId = this.GetDatabaseScalarValue<int>("SELECT [SpaceId] FROM [Sph].[Space] WHERE [RegistrationNo] =@No", new SqlParameter("@No", CS_REGISTRATION_NO));

            var driver = this.InitiateDriver();
            driver
                .LogOff()
                .NavigateToUrl("/#/rentalapplication.selectspace", 2.Seconds())
                .NavigateToUrl(string.Format("/#/application.detail-templateid.{0}/{1}", templateId, csId), 3.Seconds());

            driver
                .Click("[name='IsCompany']")
                .Value("[name='CompanyName']", "CEKAL WIBAWA RESOURCES")
                .Value("[name='CompanyRegistrationNo']", RA_COMPANYREGISTRATION_NO)
                .Value("[name='Name']", "CEKAL WIBAWA RESOURCES")
                .Value("[name='IcNo']", RA_IC_NO)
                .Value("[name='OfficeNo']", "03-51227872")
                .Value("[name='MobileNo']", "019-2655002")
                .Value("[name='Email']", "cekalwibawa03@yahoo.com")
                .Value("[name='address.Street']", "NO. 8B,  JLN ANGGERIK VANILLA AD 31/AD, KOTA KEMUNING")
                .Value("[name='address.City']", "SHAH ALAM")
                .Value("[name='address.Postcode']", "40460")
                .SelectOption("[name='address.State']", "Selangor");
             //add-on
            driver.ClickFirst("a", e => e.Text == "Fasiliti")
                .Click(".btn-add-feature")
                .SelectOption(".input-feature-available-quantity", "2",1)
                .Click(".btn-add-feature", 1)
                .Click(".btn-add-feature", 2)
                .Click("#btn-remove-feature", 2)
                ;
                driver.ClickFirst("button", e => e.Text == "Hantar Permohonan")
                .Sleep(TimeSpan.FromSeconds(5));


            var current = this.GetDatabaseScalarValue<int>("SELECT MAX([RentalApplicationId]) FROM [Sph].[RentalApplication]");
            Assert.IsTrue(max < current);
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }

    }
}
