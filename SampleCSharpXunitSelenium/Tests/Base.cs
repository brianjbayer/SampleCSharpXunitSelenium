using System;
using SampleCSharpXunitSelenium.Support;
using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;

namespace SampleCSharpXunitSelenium.Tests
{
    public class Base
    {
        public IConfiguration Configuration;
        protected IWebDriver Driver;

        public Base()
        {
            InitializeConfiguration();
        }

        public IWebDriver InitializeWebDriver()
        {
            Driver = BrowserFactory.GetBrowser(Configuration);
            return Driver;
        }

        public void InitializeConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory);

            // Configuration Precedence:
            //   1. Environment Variables override...
            //   2. file "appsettings.<ASPNETCORE_ENVIRONMENT>.json" override...
            //   3. file "appsettings.json"
            builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            if (!String.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")))
                builder.AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true);
            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public void TerminateBase()
        {
            Driver.Quit();
            Driver.Dispose();
        }

    }
}
