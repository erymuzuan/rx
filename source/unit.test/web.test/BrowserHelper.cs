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
        public static IWebDriver Value(this IWebDriver driver, string selector, string text)
        {
            var element = driver.FindElement(By.CssSelector(selector));
            var ag = string.Format("{0}", element.TagName).ToLower();
            if (ag == "select") return driver.SelectOption(selector, text);
            if (ag == "input") return driver.SetText(selector, text);
            if (ag == "textarea") return driver.SetText(selector, text);
            return driver;
        }


        public static IWebDriver StringAssert(this IWebDriver driver, string expected, string actual)
        {
            if (actual.Contains(expected)) return driver;
            throw new Exception(string.Format("Cannot find {0} in \r\n{1}", expected, actual));
        }

        public static IWebDriver SelectOption(this IWebDriver driver, string selector, string text)
        {
            new SelectElement(driver.FindElement(By.CssSelector(selector))).SelectByText(text);
            return driver;
        }


        public static IWebDriver Click(this IWebDriver driver, string selector)
        {
            try
            {
                driver.FindElement(By.CssSelector(selector)).Click();
                return driver;
            }
            catch
            {
                Console.WriteLine("Error executing click on {0}", selector);
                throw;
            }
        }

        public static IWebDriver Click(this IWebDriver driver, string selector, Expression<Func<IWebElement, bool>> filter)
        {
            var elements = driver.FindElements(By.CssSelector(selector)).AsQueryable();
            var ele = elements.SingleOrDefault(filter);
            if (null != ele)
                ele.Click();
            else
                Console.WriteLine("Cannot find element {0} : {1}", selector, filter);
            return driver;
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


        public static IWebDriver SetText(this IWebDriver driver, string selector, string text)
        {
            driver.FindElement(By.CssSelector(selector)).Clear();
            driver.FindElement(By.CssSelector(selector)).SendKeys(text);

            return driver;
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
