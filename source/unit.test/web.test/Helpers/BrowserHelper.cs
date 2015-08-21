using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using Castle.Core.Internal;
using Humanizer;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace Bespoke.Sph.WebTests.Helpers
{
    public static class BrowserHelper
    {

        public static IWebDriver NavigateToUrl(this IWebDriver driver, string url, TimeSpan wait = new TimeSpan())
        {
            driver.Navigate().GoToUrl(BrowserTest.BaseUrl + url);
            return driver.Sleep(wait);
        }


        public static IWebDriver Login(this IWebDriver driver, string userName = "admin", string password = "123456", int wait = 2000)
        {
            driver.NavigateToUrl("/Sph/SphAccount/Login", 1.Seconds());
            driver.Value("[name=UserName]", userName)
                .Value("[name=Password]", password)
                .Click("[name=submit]");

            return driver;
        }
        public static IWebDriver LogOff(this IWebDriver driver)
        {
            driver.NavigateToUrl("/Sph/SphAccount/Logoff");
            return driver;
        }

        public static IWebDriver ClikOkDialog(this IWebDriver driver)
        {
            return driver.ClickLast("input[value=OK]", x => null != x.GetAttribute("data-bind"));
        }

        public static IWebDriver Value(this IWebDriver driver, string selector, string text, int index = 0, int wait = 0)
        {
            var list = driver.FindElements(By.CssSelector(selector));
            if (list.Count <= index)
                throw new ArgumentException($"There's only {list.Count} elements for {selector} selector");

            var element = list[index];
            driver.Focus(selector);

            var tag = $"{element.TagName}".ToLower();
            try
            {
                switch (tag)
                {
                    case "select":
                        driver.SelectOption(element, text);
                        break;
                    case "input":
                        driver.SetText(element, text);
                        break;
                    case "textarea":
                        driver.SetText(element, text);
                        break;
                }

            }
            finally
            {
                if (wait > 0)
                    driver.Sleep(wait);

                driver.Blur(selector);

            }
            return driver;
        }

        public static IWebDriver Value(this IWebDriver driver, string selector, string text, Expression<Func<IWebElement, bool>> filter, int wait = 0)
        {
            var elements = driver.FindElements(By.CssSelector(selector)).AsQueryable();
            var element = elements.SingleOrDefault(filter);
            if (null == element) return driver;
            try
            {
                driver.Focus(selector);
                var ag = $"{element.TagName}".ToLower();
                switch (ag)
                {
                    case "select":
                        driver.SelectOption(element, text);
                        break;
                    case "input":
                        driver.SetText(element, text);
                        break;
                    case "textarea":
                        driver.SetText(element, text);
                        break;
                }

                if (wait > 0)
                    driver.Sleep(wait);
            }
            finally
            {
                driver.Blur(selector);
            }

            return driver;
        }


        public static IWebDriver Blur(this IWebDriver driver, string selector)
        {
            return driver.ExecuteScript($"$(\"{selector}\").blur();");

        }

        public static IWebDriver Focus(this IWebDriver driver, string selector)
        {
            return driver.ExecuteScript($"$(\"{selector}\").focus();");

        }

        public static IWebDriver ExecuteScript(this IWebDriver driver, string script)
        {
            var jse = driver as IJavaScriptExecutor;
            Console.WriteLine(" execute scripts : " + script);
            jse?.ExecuteScript(script);

            return driver;

        }
        public static IWebDriver StringAssert(this IWebDriver driver, string expected, string actual)
        {
            if (actual.Contains(expected)) return driver;
            throw new Exception($"Cannot find {expected} in \r\n{actual}");
        }

        public static IWebDriver SelectOption(this IWebDriver driver, IWebElement element, string text)
        {
            new SelectElement(element).SelectByText(text);
            return driver;
        }

        public static IWebDriver SelectOption(this IWebDriver driver, string selector, string text, int index = 0, bool selectByText = true)
        {
            var elements = driver.FindElements(By.CssSelector(selector));
            if (elements.Count <= index)
                throw new ArgumentException(
$"There's only {elements.Count} elements for {selector} selector");

            var select = new SelectElement(elements[index]);

            if (selectByText)
                select.SelectByText(text);
            else
                select.SelectByValue(text);
            return driver;
        }



        public static IWebDriver Click(this IWebDriver driver, string selector, int index = 0, int wait = 0)
        {
            try
            {
                var elements = driver.FindElements(By.CssSelector(selector));
                var element = elements[index];

                element.Click();
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

        public static IWebDriver ContextClick(this IWebDriver driver, string selector, int index = 0, int wait = 0)
        {
            try
            {


                // step 1 - select the element you want to right-click
                var elementToRightClick = driver.FindElement(By.CssSelector(selector));
                // step 2 - create and step up an Actions object with your driver
                var action = new Actions(driver);
                action.ContextClick(elementToRightClick);
                // step 3 - execute the action
                action.Perform();

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
            Assert.IsTrue(null != ele, message);
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

        public static IWebDriver ActivateTabItem(this IWebDriver driver, string href)
        {
            return driver.ClickFirst("a[data-toggle=tab]", e => e.GetAttribute("href").EndsWith(href));
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


        public static IWebDriver SendKeys(this IWebDriver driver, string selector, string text)
        {
            var element = driver.FindElement(By.CssSelector(selector));
            element.SendKeys(text);

            return driver;
        }

        public static IWebDriver SetText(this IWebDriver driver, string selector, string text)
        {
            var element = driver.FindElement(By.CssSelector(selector));
            return driver.SetText(element, text);
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
        public static IWebDriver WaitUntil(this IWebDriver driver, By by, TimeSpan span, string message = "")
        {
            Console.WriteLine("Wait {0} : {1}", span, message);
            driver.Manage().Timeouts().ImplicitlyWait(span);
            var wait = new WebDriverWait(driver, span);
            wait.Until(d => d.FindElement(by));

            return driver;
        }
        public static IWebDriver WaitUntil(this IWebDriver driver,string selector, double seconds = 2, string message = "")
        {
            Console.WriteLine("Wait {0} : {1}", seconds, message);
            driver.Manage().Timeouts().ImplicitlyWait(seconds.Seconds());
            var wait = new WebDriverWait(driver, seconds.Seconds());
            wait.Until(d => d.FindElement(By.CssSelector(selector)));

            return driver;
        }
        public static IWebDriver Sleep(this IWebDriver driver, int miliseconds, string message = "")
        {
            Console.WriteLine("Sleep {0} : {1}", miliseconds, message);
            Thread.Sleep(miliseconds);
            return driver;
        }
        public static IWebDriver Sleep(this IWebDriver driver, TimeSpan span, string message = "")
        {
            Console.WriteLine("Sleep {0} : {1}", span, message);
            Thread.Sleep(span);
            return driver;
        }
    }
}
