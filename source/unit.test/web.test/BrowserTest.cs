﻿using System;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Web.Security;
using System.Xml.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace web.test
{

    public class BrowserTest
    {

        public const string WEB_RUANG_KOMERCIAL_URL = "http://localhost:4436";
        public const string WEB_DRIVER_PATH = @"D:\project\work\quarters\webdrivers";
        public const string DUMMY_USER_NAME = "[Test] Dummy User";


        protected IWebDriver InitiateDriver(string agent = null)
        {

            IWebDriver driver = new FirefoxDriver();
            return driver;
        }

        protected void PayWithMigs(IWebDriver driver)
        {
            Assert.IsTrue(driver.PageSource.Contains("Sila pilih cara bayaran"));
            driver
                .Click("#migLinkButton")
                .Sleep(10000, "MIGS website to show up");
        }

        protected void InsertMasterCardDetails(IWebDriver driver)
        {
            Assert.IsTrue(driver.PageSource.Contains("TEST MODE"));
            driver.FindElement(By.Name("MasterCard")).Click();

            driver
                .Wait(200, "Selecting mastercard for test mode")
                .SetText("#CardNumber", "5123456789012346")
                .SetText("#CardMonth", "05")
                .SetText("#CardYear", "13")
                .SetText("#Securecode", "100")
                .Click("#Paybutton")
                .Wait(5000, "Paying the bill... at MIGS");
        }

        protected void InitDatabase()
        {

        }

    

        public void AddUser(TestUser user)
        {
            if (Membership.GetUser(user.UserName) != null)
            {
                Membership.DeleteUser(user.UserName);
            };
            BuildingTest.SPH_DATABASE.ExecuteNonQuery("DELETE FROM [Sph].[UserProfile] WHERE [UserName] = @UserName", new SqlParameter("@UserName", user.UserName));
            var u = Membership.CreateUser(user.UserName, user.Password, user.Email);
            var id =
                BuildingTest.SPH_DATABASE.GetDatabaseScalarValue<int>(
                    "SELECT MAX([UserProfileId]) + 1 FROM [Sph].[UserProfile]");


            // add roles
            Roles.AddUserToRoles(user.UserName, user.Roles);


            var x =
                string.Format(
                    "<UserProfile xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                    "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.bespoke.com.my/\" " +
                    "WebId=\"{0}\" " +
                    "CreatedDate=\"2013-01-01T00:00:00\" " +
                    "ChangedDate=\"2013-01-01T00:00:00\" " +
                    "Username=\"{1}\" " +
                    "FullName=\"{2}\" " +
                    "Designation=\"{3}\" " +
                    "Telephone=\"{4}\" " +
                    "RoleTypes=\"{5}\"" +
                    " StartModule=\"{6}\" " +
                    "Email=\"{7}\" " +
                    "UserProfileId=\"{8}\" Department=\"{9}\" />",
                    Guid.NewGuid(),
                    user.UserName,
                    user.FullName,
                    user.Designation,
                    user.Telephone,
                    string.Join(",", user.Roles),
                    user.StartModule,
                    user.Email,
                    id,
                    user.Department
                    );

            BuildingTest.SPH_DATABASE.ExecuteNonQuery("INSERT INTO [Sph].[UserProfile]([UserName],[Data],[FullName], [Email], [Designation], [Department]," +
                                                      "[CreatedDate],[ChangedDate],[CreatedBy],[ChangedBy]) " +
                                                      "VALUES(@UserName, @Data,@FullName, @Email, @Designation, @Department, '2013-01-01','2013-01-01','test','test')",
                new SqlParameter("@UserName", u.UserName),
                new SqlParameter("@Data", x),
                new SqlParameter("@FullName", user.FullName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Designation", user.Designation),
                new SqlParameter("@Department", user.Department)
                );

            Console.WriteLine("User {0} is successfully created", user.UserName);

        }
    }
}