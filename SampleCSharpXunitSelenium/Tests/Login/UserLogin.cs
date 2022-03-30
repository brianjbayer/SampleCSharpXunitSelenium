using System;
using Xunit;
using SampleCSharpXunitSelenium.PageObjects.Login;
using SampleCSharpXunitSelenium.PageObjects.SecureArea;
using OpenQA.Selenium;
using SampleCSharpXunitSelenium.Support;
using FluentAssertions;
using FluentAssertions.Execution;

namespace SampleCSharpXunitSelenium.Tests.Login
{
    [Collection("UserLogin")]
    public class UserLogin : IDisposable
    {
        Base _base;
        IWebDriver _driver;
        LoginPage _loginPage;

        public UserLogin()
        {
            _base = new Base();
            _driver = _base.InitializeWebDriver();

            // Navigate to Login Page
            _driver.Url = _base.Configuration["LoginPageUrl"];
            _loginPage = new LoginPage(_driver);
        }

        public void Dispose()
        {
            _base.TerminateBase();
        }

        [Fact]
        public void TestAssumeLoginPageItems()
        {
            _driver.WaitFor(() => _loginPage.loginPageHeading.Displayed
            , TimeSpan.FromSeconds(3)
            , "waiting for the login page to load timed out.");

            using (new AssertionScope())
            {
                _loginPage.usernameInput.Displayed.Should().BeTrue();
                _loginPage.passwordInput.Displayed.Should().BeTrue();
                _loginPage.submitButton.Displayed.Should().BeTrue();
            }
        }

        [Fact]
        public void TestLoginWithValidCredentials()
        {
            string validUser = _base.Configuration["ValidLoginUser"];
            string validPass = _base.Configuration["ValidLoginPass"];

            SecureAreaPage secureAreaPage;
            string secureAreaPageURL = _base.Configuration["SecureAreaPageUrl"];

            // Given the user is on the Login Page
            _driver.WaitFor(() => _loginPage.loginPageHeading.Displayed
             , TimeSpan.FromSeconds(3)
             , "waiting for the login page to load timed out.");

            // When the user logs in with valid credentials
            _loginPage.LoginUser(validUser, validPass);

            // Then the user is taken to the secure area page
            secureAreaPage = new SecureAreaPage(_driver);
            _driver.WaitFor(() => secureAreaPage.secureAreaHeading.Displayed
            , TimeSpan.FromSeconds(3)
            , "waiting for the secure area page to load timed out.");

            // And should be logged in
            using (new AssertionScope())
            {
                secureAreaPage.loggedInMessage.Displayed.Should().BeTrue();
            }
        }
    }
}
