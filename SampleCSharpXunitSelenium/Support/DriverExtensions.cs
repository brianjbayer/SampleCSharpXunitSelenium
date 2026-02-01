using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using Xunit;

namespace SampleCSharpXunitSelenium.Support
{
    public static class DriverExtensions
    {

        public static void HoverElement(this IWebElement element)
        {
            var driver = ((IWrapsDriver)element).WrappedDriver;
            var act = new Actions(driver);
            act.MoveToElement(element).Build().Perform();
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
