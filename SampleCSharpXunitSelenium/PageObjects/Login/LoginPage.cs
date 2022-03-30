using System;
using SampleCSharpXunitSelenium.Support;
using OpenQA.Selenium;

namespace SampleCSharpXunitSelenium.PageObjects.Login
{
    public class LoginPage
    {
        IWebDriver _driver;
        public LoginPage(IWebDriver Driver) => _driver = Driver;
        public IWebElement loginPageHeading => _driver.FindElement(By.XPath("//h2[contains(text(),'Login Page')]"));
        public IWebElement usernameInput => _driver.FindElement(By.XPath("//*[@id='username']"));
        public IWebElement passwordInput => _driver.FindElement(By.XPath("//*[@id='password']"));
        public IWebElement submitButton => _driver.FindElement(By.XPath("//*[@id='login']/button/i"));

        public void LoginUser(string user, string pass)
        {
            _driver.WaitFor(() => usernameInput.Displayed && usernameInput.Enabled
            , TimeSpan.FromSeconds(60)
            , "waiting for the input on the login page timed out");

            usernameInput.SendKeys(user);
            passwordInput.SendKeys(pass);
            submitButton.Click();
        }

    }
}
