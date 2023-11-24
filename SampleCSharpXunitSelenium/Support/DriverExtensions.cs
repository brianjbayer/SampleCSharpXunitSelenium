using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Xunit;

namespace SampleCSharpXunitSelenium.Support
{
    public static class DriverExtensions
    {
        public static void ClearInput(this IWebElement element)
        {
            while (!element.GetAttribute("value").Equals(""))
            {
                element.SendKeys(Keys.Backspace);
            }
        }

        public static string ConvertRGBToHex(this string rgb)
        {
            var numbers = rgb.Contains("a") ?
                rgb.Replace("rgba(", "").Replace(")", "").Split(',')
                : rgb.Replace("rgb(", "").Replace(")", "").Split(',');

            int r = Convert.ToInt16(numbers[0].Trim());
            int g = Convert.ToInt16(numbers[1].Trim());
            int b = Convert.ToInt16(numbers[2].Trim());
            var hex = r.ToString("X2") + g.ToString("X2") + b.ToString("X2");
            return hex;
        }

        public static void HoverElement(this IWebElement element)
        {
            var driver = ((IWrapsDriver)element).WrappedDriver;
            var act = new Actions(driver);
            act.MoveToElement(element).Build().Perform();
        }

        public static bool IsChecked(this IWebElement element)
        {
            var driver = ((IWrapsDriver)element).WrappedDriver;
            var jsDriver = (IJavaScriptExecutor)driver;
            return (bool)jsDriver.ExecuteScript("return arguments[0].checked", element);
        }

        ///<summary>
        ///Pass the select element and the value, and this will set the value in the select and attempt to fire an event to trigger
        ///any dynamic vue logic and or form validation.
        ///</summary>
        public static void JsSelectByValue(this IWebElement element, string value)
        {
            var driver = ((IWrapsDriver)element).WrappedDriver;
            var jsDriver = (IJavaScriptExecutor)driver;
            var elementId = element.GetAttribute("id");
            jsDriver.ExecuteScript("document.getElementById(arguments[0]).value = arguments[1] ;", elementId, value);
            jsDriver.ExecuteScript("const event = new Event('input'); arguments[0].dispatchEvent(event);", element);
            element.SendKeys(Keys.Tab);
        }

        public static void RemoveElement(this IWebElement element)
        {
            var driver = ((IWrapsDriver)element).WrappedDriver;
            var jsDriver = (IJavaScriptExecutor)driver;
            jsDriver.ExecuteScript("arguments[0].remove();", element);
        }

        public static void ScrollToElement(this IWebElement element)
        {
            var driver = ((IWrapsDriver)element).WrappedDriver;
            var jsDriver = (IJavaScriptExecutor)driver;
            jsDriver.ExecuteScript("arguments[0].scrollIntoView(true);", element);
            Thread.Sleep(500);
        }

        public static bool WaitFor(this IWebDriver Driver, Func<bool> waitFunction, string timeOutMessage)
        {
            return Driver.WaitFor<bool>(waitFunction, timeOutMessage);
        }

        public static T WaitFor<T>(this IWebDriver Driver, Func<T> waitFunction, string timeoutMessage)
        {
            return Driver.WaitFor<T>(waitFunction, TimeSpan.FromSeconds(5), timeoutMessage);
        }

        public static T WaitFor<T>(this IWebDriver Driver, Func<T> waitFunction, TimeSpan timeout, string timeoutMessage)
        {
            DateTime endTime = DateTime.Now.Add(timeout);
            T value = default(T);
            Exception lastException = null;
            while (DateTime.Now < endTime)
            {
                try
                {
                    value = waitFunction();
                    if (typeof(T) == typeof(bool))
                    {
                        if ((bool)(object)value)
                        {
                            return value;
                        }
                    }
                    else if (value != null)
                    {
                        return value;
                    }

                    Thread.Sleep(100);
                }
                catch (Exception e)
                {
                    //swallow to rethrow later
                    lastException = e;
                }
            }

            if (lastException != null)
            {
                TakeScreenshot(Driver);
                throw new WebDriverException($"Webdriver timed out: {timeoutMessage}", lastException);
            }

            TakeScreenshot(Driver);
            Assert.Fail("Condition timed out: " + timeoutMessage);
            return default(T);
        }

        private static void TakeScreenshot(IWebDriver Driver)
        {
            Screenshot ss = (Driver as ITakesScreenshot).GetScreenshot();
            string title = @$"TimeoutScreenshot_{DateTime.Now.ToString("yy-MM-dd-HH_mm_ss")}";
            string fileName = $"../../../TestResults/{title}.jpg";

            try
            {
                ss.SaveAsFile(fileName);
            }
            catch
            {
                Console.WriteLine($"Failed to save screenshot: {fileName}");
            }

        }
    }
}
