using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hermes_UITest
{
    class LoginUserTest
    {        
        private string _test_url = " https://hermes2020onlinechatapp.azurewebsites.net/";

        IWebDriver driver;


        [SetUp]
        public void StartBrowser()
        {
            // Local Selenium WebDriver - Chrome driver
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        // Given when then

        [Test]
        public void LoginRegisteredUser_UsernamePasswordCorrect_LoginSuccededHelloUser()
        {
            driver.Url = _test_url;


            System.Threading.Thread.Sleep(3000);


            IWebElement loginButton = driver.FindElement(By.LinkText("Login"));

            loginButton.Click();

            System.Threading.Thread.Sleep(3000);

            IWebElement usernameField = driver.FindElement(By.CssSelector("[id='Input_UserName']"));

            usernameField.SendKeys("Test12");
            
            IWebElement passwordField = driver.FindElement(By.CssSelector("[id='Input_Password']"));

            passwordField.SendKeys("zxcvbnm");
            
            IWebElement loginConfirmButton = driver.FindElement(By.CssSelector("[class = 'btn btn-primary']"));

            loginConfirmButton.Click();

            System.Threading.Thread.Sleep(6000);

            IWebElement helloUser = driver.FindElement(By.PartialLinkText("Hello"));

            Console.WriteLine("Test Passed");
        }

        [Test]
        public void LoginRegisteredUser_UsernameCorrectPasswordIncorrect_ReturnMessageInvalidLogin()
        {
            driver.Url = _test_url;


            System.Threading.Thread.Sleep(3000);


            IWebElement loginButton = driver.FindElement(By.LinkText("Login"));

            loginButton.Click();

            System.Threading.Thread.Sleep(3000);

            IWebElement usernameField = driver.FindElement(By.CssSelector("[id='Input_UserName']"));

            usernameField.SendKeys("Test12");

            IWebElement passwordField = driver.FindElement(By.CssSelector("[id='Input_Password']"));

            passwordField.SendKeys("qwertyuiop");

            IWebElement loginConfirmButton = driver.FindElement(By.CssSelector("[class = 'btn btn-primary']"));

            loginConfirmButton.Click();

            System.Threading.Thread.Sleep(6000);

            driver.FindElements(By.LinkText("//li[text(),'Invalid login attempt.']"));

            Console.WriteLine("Test Passed");
        }

        [Test]
        public void LoginRegisteredUser_UsernameInorrectPasswordCorrect_ReturnMessageInvalidLogin()
        {
            driver.Url = _test_url;


            System.Threading.Thread.Sleep(3000);


            IWebElement loginButton = driver.FindElement(By.LinkText("Login"));

            loginButton.Click();

            System.Threading.Thread.Sleep(3000);

            IWebElement usernameField = driver.FindElement(By.CssSelector("[id='Input_UserName']"));

            usernameField.SendKeys("Test122");

            IWebElement passwordField = driver.FindElement(By.CssSelector("[id='Input_Password']"));

            passwordField.SendKeys("zxcvbnm");

            IWebElement loginConfirmButton = driver.FindElement(By.CssSelector("[class = 'btn btn-primary']"));

            loginConfirmButton.Click();

            System.Threading.Thread.Sleep(6000);

            driver.FindElements(By.LinkText("//li[text(),'Invalid login attempt.']"));

            Console.WriteLine("Test Passed");
        }

        [Test]
        public void LoginUser_UsernameIncorrectPasswordIncorrect_ReturnMessageInvalidLogin()
        {
            driver.Url = _test_url;


            System.Threading.Thread.Sleep(3000);


            IWebElement loginButton = driver.FindElement(By.LinkText("Login"));

            loginButton.Click();

            System.Threading.Thread.Sleep(3000);

            IWebElement usernameField = driver.FindElement(By.CssSelector("[id='Input_UserName']"));

            usernameField.SendKeys("Test21");

            IWebElement passwordField = driver.FindElement(By.CssSelector("[id='Input_Password']"));

            passwordField.SendKeys("qwertyuiop");

            IWebElement loginConfirmButton = driver.FindElement(By.CssSelector("[class = 'btn btn-primary']"));

            loginConfirmButton.Click();

            System.Threading.Thread.Sleep(6000);

            driver.FindElements(By.LinkText("//li[text(),'Invalid login attempt.']"));

            Console.WriteLine("Test Passed");
        }

        [Test]
        public void LoginUser_UsernameEmptyPasswordCorrect_ReturnMessageInvalidLogin()
        {
            driver.Url = _test_url;


            System.Threading.Thread.Sleep(3000);


            IWebElement loginButton = driver.FindElement(By.LinkText("Login"));

            loginButton.Click();

            System.Threading.Thread.Sleep(3000);
            
            IWebElement passwordField = driver.FindElement(By.CssSelector("[id='Input_Password']"));

            passwordField.SendKeys("zxcvbnm");

            IWebElement loginConfirmButton = driver.FindElement(By.CssSelector("[class = 'btn btn-primary']"));

            loginConfirmButton.Click();

            System.Threading.Thread.Sleep(6000);

            driver.FindElements(By.LinkText("//li[text(),'The Username field is required.']"));

            Console.WriteLine("Test Passed");
        }

        [Test]
        public void LoginUser_UsernameCorrectPasswordEmpty_ReturnMessageInvalidLogin()
        {
            driver.Url = _test_url;


            System.Threading.Thread.Sleep(3000);


            IWebElement loginButton = driver.FindElement(By.LinkText("Login"));

            loginButton.Click();

            System.Threading.Thread.Sleep(3000);

            IWebElement usernameField = driver.FindElement(By.CssSelector("[id='Input_UserName']"));

            usernameField.SendKeys("Test12");
            
            IWebElement loginConfirmButton = driver.FindElement(By.CssSelector("[class = 'btn btn-primary']"));

            loginConfirmButton.Click();

            System.Threading.Thread.Sleep(6000);

            driver.FindElements(By.LinkText("//li[text(),'The Password field is required.']"));

            Console.WriteLine("Test Passed");
        }

        [Test]
        public void LoginUser_UsernameEmptyPasswordEmpty_ReturnMessageInvalidLogin()
        {
            driver.Url = _test_url;


            System.Threading.Thread.Sleep(3000);


            IWebElement loginButton = driver.FindElement(By.LinkText("Login"));

            loginButton.Click();

            System.Threading.Thread.Sleep(3000);
            
            IWebElement loginConfirmButton = driver.FindElement(By.CssSelector("[class = 'btn btn-primary']"));

            loginConfirmButton.Click();

            System.Threading.Thread.Sleep(6000);

            driver.FindElements(By.LinkText("//li[text(),'The Username field is required.']"));
            driver.FindElements(By.LinkText("//li[text(),'The Password field is required.']"));

            Console.WriteLine("Test Passed");
        }

        [Test]
        public void LoginRegisteredUser_UsernamePasswordCorrect_Logout()
        {
            driver.Url = _test_url;


            System.Threading.Thread.Sleep(3000);


            IWebElement loginButton = driver.FindElement(By.LinkText("Login"));

            loginButton.Click();

            System.Threading.Thread.Sleep(3000);

            IWebElement usernameField = driver.FindElement(By.CssSelector("[id='Input_UserName']"));

            usernameField.SendKeys("Test12");

            IWebElement passwordField = driver.FindElement(By.CssSelector("[id='Input_Password']"));

            passwordField.SendKeys("zxcvbnm");

            IWebElement loginConfirmButton = driver.FindElement(By.CssSelector("[class = 'btn btn-primary']"));

            loginConfirmButton.Click();

            System.Threading.Thread.Sleep(6000);

            IWebElement helloUser = driver.FindElement(By.PartialLinkText("Hello"));

            IWebElement logoutButton = driver.FindElement(By.CssSelector("[class = 'nav-link btn btn-link text-dark']"));

            logoutButton.Click();

            System.Threading.Thread.Sleep(2000);

            IWebElement loginButtonagain = driver.FindElement(By.LinkText("Login"));

            Console.WriteLine("Test Passed");
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Quit();
        }


    }
}
