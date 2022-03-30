using OpenQA.Selenium;

namespace SampleCSharpXunitSelenium.PageObjects.SecureArea
{
    public class SecureAreaPage
    {
        IWebDriver _driver;
        public SecureAreaPage(IWebDriver Driver) => _driver = Driver;
        public IWebElement secureAreaHeading => _driver.FindElement(By.XPath("//h2[contains(text(),'Secure Area')]"));
        public IWebElement loggedinFlash => _driver.FindElement(By.XPath("//*[@id='flash']"));
        public IWebElement loggedInMessage => _driver.FindElement(By.XPath("//*[contains(text(),'logged in')]"));
        public IWebElement logoutButton => _driver.FindElement(By.XPath("//*[@id='content']/div/a"));

    }
}
