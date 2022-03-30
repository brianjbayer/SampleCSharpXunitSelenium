using System;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chromium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Safari;
using OpenQA.Selenium.Remote;
using WebDriverManager;
using WebDriverManager.DriverConfigs;
using WebDriverManager.DriverConfigs.Impl;

namespace SampleCSharpXunitSelenium.Support
{
    public class BrowserFactory
    {
        public const int StandardWaitSecs = 10;
        public const int MaxHDWidth = 1875;
        public const int MaxHDHeight = 1080;

        private static bool IsLocalBrowser(IConfiguration config)
        {
            return String.IsNullOrEmpty(config["Remote_Url"]);
        }
        private static bool IsHeadless(IConfiguration config)
        {
            return Boolean.Parse(config["Headless"] ?? "false");
        }
        private static T ConfigureChromiumOptions<T>(bool isHeadless) where T : ChromiumOptions
        {
            // Create Instance of Passed Type
            T opts = (T)Activator.CreateInstance(typeof(T), new object[] { });

            // Disable chrome's password facility so it won't bug us about saving passwords
            opts.AddUserProfilePreference("credentials_enable_service", false);
            opts.AddUserProfilePreference("profile.password_manager_enabled", false);
            // Remove controlled by automation banner
            opts.AddExcludedArgument("enable-automation");
            opts.AddAdditionalOption("useAutomationExtension", false);
            if (isHeadless)
            {
                opts.AddArgument("--headless");
            }
            return opts;
        }

        private static FirefoxOptions ConfigureFirefoxOptions(bool isHeadless)
        {
            FirefoxOptions browserOptions = new FirefoxOptions();
            if (isHeadless)
            {
                browserOptions.AddArgument("--headless");
            }
            return browserOptions;
        }

        private static IWebDriver CreateBrowser<T, U, V>(T browserOptions,
          bool isLocal, String remoteUrl)
          where T : DriverOptions
          where U : WebDriver
          where V : IDriverConfig
        {
            IWebDriver driver;

            if (isLocal)
            {
                V driverConfig = (V)Activator.CreateInstance(typeof(V), new object[] { });
                new DriverManager().SetUpDriver(driverConfig);
                driver = (U)Activator.CreateInstance(typeof(U), new object[] { browserOptions });
            }
            else
            {
                driver = new RemoteWebDriver(new Uri(remoteUrl), browserOptions);
            }
            return driver;
        }

        public static IWebDriver GetBrowser(IConfiguration config)
        {
            IWebDriver driver;

            bool isHeadless = IsHeadless(config);
            bool isLocal = IsLocalBrowser(config);
            String remoteUrl = config["Remote_Url"];

            String browser = config["Browser"].ToLower();
            switch (browser)
            {
                case "chrome":
                    {
                        ChromeOptions browserOptions = ConfigureChromiumOptions<ChromeOptions>(isHeadless);
                        driver = CreateBrowser<ChromeOptions, ChromeDriver, ChromeConfig>(browserOptions, isLocal, remoteUrl);
                        break;
                    }

                case "edge":
                    {
                        EdgeOptions browserOptions = ConfigureChromiumOptions<EdgeOptions>(isHeadless);
                        driver = CreateBrowser<EdgeOptions, EdgeDriver, EdgeConfig>(browserOptions, isLocal, remoteUrl);
                        break;
                    }

                case "firefox":
                    {
                        FirefoxOptions browserOptions = ConfigureFirefoxOptions(isHeadless);
                        driver = CreateBrowser<FirefoxOptions, FirefoxDriver, FirefoxConfig>(browserOptions, isLocal, remoteUrl);
                        break;
                    }

                case "safari":
                    {
                        // Safari only supports local, and has builtin browserdriver
                        SafariOptions browserOptions = new SafariOptions();
                        driver = new SafariDriver(browserOptions);
                        break;
                    }

                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(browser));
                    }
            }

            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(StandardWaitSecs);
            // Set Window Size to avoid missing elements in DOM due to viewport size
            // (issues with macOS Chrome set-maximized and Manage().Window.Maximize())
            driver.Manage().Window.Size = new System.Drawing.Size(MaxHDWidth, MaxHDHeight);
            driver.Manage().Cookies.DeleteAllCookies();
            return driver;
        }
    }
}
