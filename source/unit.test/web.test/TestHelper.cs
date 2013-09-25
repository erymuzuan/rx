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
