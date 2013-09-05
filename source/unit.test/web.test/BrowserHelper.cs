using System;
using System.Linq.Expressions;
using Castle.Core.Internal;
using OpenQA.Selenium;
using System.Linq;
using OpenQA.Selenium.Support.UI;

namespace web.test
{
    public static class BrowserHelper
    {

        public static IWebDriver NavigateToUrl(this IWebDriver driver, string url, TimeSpan wait = new TimeSpan())
        {
            driver.Navigate().GoToUrl(BrowserTest.WEB_RUANG_KOMERCIAL_URL + url);
            return driver.Sleep(wait);
        }

        public static IWebDriver Login(this IWebDriver driver, TestUser user,
            int wait = 2000)
        {
            return driver.Login(user.UserName, user.Password, wait);
        }

        public static IWebDriver Login(this IWebDriver driver, string username = "admin", string password = "123456", int wait = 2000)
        {
            driver.Navigate().GoToUrl(BrowserTest.WEB_RUANG_KOMERCIAL_URL + "/Account/Login");
            driver.Sleep(150)
                .Sleep(TimeSpan.FromSeconds(2))
                .Value("[name='UserName']", username)
                .Value("[name='Password']", password)
                .Click("[name='submit']")
                .Sleep(200);

            return driver;
        }

        public static IWebDriver Value(this IWebDriver driver, string selector, string text, int index = 0, int wait = 0)
        {
            var list = driver.FindElements(By.CssSelector(selector));
            var element = list[index];

            var ag = string.Format("{0}", element.TagName).ToLower();
            if (ag == "select") return driver.SelectOption(element, text);
            if (ag == "input") return driver.SetText(element, text);
            if (ag == "textarea") return driver.SetText(element, text);

            if (wait > 0)
                driver.Sleep(wait);

            return driver;
        }

        public static IWebDriver StringAssert(this IWebDriver driver, string expected, string actual)
        {
            if (actual.Contains(expected)) return driver;
            throw new Exception(string.Format("Cannot find {0} in \r\n{1}", expected, actual));
        }

        public static IWebDriver SelectOption(this IWebDriver driver, IWebElement element, string text)
        {
            new SelectElement(element).SelectByText(text);
            return driver;
        }

        public static IWebDriver SelectOption(this IWebDriver driver, string selector, string text)
        {
            new SelectElement(driver.FindElement(By.CssSelector(selector))).SelectByText(text);
            return driver;
        }


        public static IWebDriver Click(this IWebDriver driver, string selector, int wait = 0)
        {
            try
            {
                driver.FindElement(By.CssSelector(selector)).Click();
                if (wait > 0)
                    driver.Sleep(wait);
                return driver;
            }
            catch
            {
                Console.WriteLine("Error executing click on {0}", selector);
                throw;
            }
        }

        public static IWebDriver AssertElementExist(this IWebDriver driver, string selector, Expression<Func<IWebElement, bool>> assert, string message = "")
        {
            var elements = driver.FindElements(By.CssSelector(selector)).AsQueryable();
            var ele = elements.SingleOrDefault(assert);
            NUnit.Framework.Assert.IsTrue(null != ele, message);
            return driver;
        }

        public static IWebDriver Click(this IWebDriver driver, string selector, Expression<Func<IWebElement, bool>> filter, TimeSpan wait = new TimeSpan())
        {
            var elements = driver.FindElements(By.CssSelector(selector)).AsQueryable();
            var ele = elements.SingleOrDefault(filter);
            if (null != ele)
                ele.Click();
            else
                Console.WriteLine("Cannot find element {0} : {1}", selector, filter);
            return driver.Sleep(wait);
        }


        public static IWebDriver ClickLast(this IWebDriver driver, string selector, Expression<Func<IWebElement, bool>> filter)
        {
            var elements = driver.FindElements(By.CssSelector(selector)).AsQueryable();
            var ele = elements.LastOrDefault(filter);
            if (null != ele)
            {
                Console.WriteLine("Clicking last on :{0}", ele.Text);
                ele.Click();
            }
            else
                Console.WriteLine("Cannot find element {0} : {1}", selector, filter);
            return driver;
        }
        public static IWebDriver ClickAll(this IWebDriver driver, string selector, Expression<Func<IWebElement, bool>> filter)
        {
            var elements = driver.FindElements(By.CssSelector(selector)).AsQueryable();
            elements.Where(filter).ForEach(e =>
                {
                    Console.WriteLine("Click on #{0}[name='{1}'][class='{2}']<{3}>"
                        , e.GetAttribute("id")
                        , e.GetAttribute("name")
                        , e.GetAttribute("class")
                        , e.Text);
                    e.Click();
                });
            return driver;
        }

        public static IWebDriver ClickFirst(this IWebDriver driver, string selector, Expression<Func<IWebElement, bool>> filter)
        {
            var elements = driver.FindElements(By.CssSelector(selector)).AsQueryable();
            var ele = elements.FirstOrDefault(filter);
            if (null != ele)
                ele.Click();
            else
                Console.WriteLine("Cannot find element {0} : {1}", selector, filter);
            return driver;
        }

        public static IWebDriver Click(this IWebDriver driver, string selector, Expression<Func<IWebElement, bool>> filter, int index)
        {
            var elements = driver.FindElements(By.CssSelector(selector)).AsQueryable();
            var ele = elements.Where(filter).ToList()[index];
            if (null != ele)
                ele.Click();
            else
                Console.WriteLine("Cannot find element {0} : {1}", selector, filter);
            return driver;
        }


        public static IWebDriver SetText(this IWebDriver driver, IWebElement element, string text)
        {
            element.Clear();
            element.SendKeys(text);

            return driver;
        }

        public static IWebDriver SetText(this IWebDriver driver, string selector, string text)
        {
            var element = driver.FindElement(By.CssSelector(selector));
            return driver.SetText(element,text);
        }


        public static IWebDriver Wait(this IWebDriver driver, int miliseconds, string message = "")
        {
            Console.WriteLine("Wait {0} : {1}", miliseconds, message);
            driver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromMilliseconds(miliseconds));
            return driver;
        }
        public static IWebDriver Wait(this IWebDriver driver, TimeSpan span, string message = "")
        {
            Console.WriteLine("Wait {0} : {1}", span, message);
            driver.Manage().Timeouts().ImplicitlyWait(span);
            return driver;
        }
        public static IWebDriver Sleep(this IWebDriver driver, int miliseconds, string message = "")
        {
            Console.WriteLine("Sleep {0} : {1}", miliseconds, message);
            System.Threading.Thread.Sleep(miliseconds);
            return driver;
        }
        public static IWebDriver Sleep(this IWebDriver driver, TimeSpan span, string message = "")
        {
            Console.WriteLine("Sleep {0} : {1}", span, message);
            System.Threading.Thread.Sleep(span);
            return driver;
        }
    }
}
