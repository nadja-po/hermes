using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;

namespace Hermes_UITest
{
    class RegistredUserTests
    {
        String test_url = "  https://hermes2020onlinechat.azurewebsites.net/";

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
        public void RegistrateNewUser_EmailUsernameTaken_ReturnMessageTaken()
        {
            driver.Url = test_url;


            System.Threading.Thread.Sleep(3000);


            IWebElement registerButton = driver.FindElement(By.CssSelector("[class = 'nav-link text-dark']"));

            registerButton.Click();

            System.Threading.Thread.Sleep(3000);

            IWebElement usernameField = driver.FindElement(By.CssSelector("[id='Input_UserName']"));

            usernameField.SendKeys("UITest1");

            IWebElement emailField = driver.FindElement(By.CssSelector("[id='Input_Email']"));

            emailField.SendKeys("lienedarta+Test1@gmail.com");

            IWebElement passwordField = driver.FindElement(By.CssSelector("[id='Input_Password']"));

            passwordField.SendKeys("zxcvbnm");

            IWebElement passwordConfirm = driver.FindElement(By.CssSelector("[id='Input_ConfirmPassword']"));

            passwordConfirm.SendKeys("zxcvbnm");

            IWebElement registerConfirmButton = driver.FindElement(By.CssSelector("[class = 'btn btn-primary']"));

            registerConfirmButton.Click();
            
            System.Threading.Thread.Sleep(6000);

            var findTextTakenUser = driver.FindElements(By.LinkText("//li[text(),'UITest1']"));

            var findTextTakenEmail = driver.FindElements(By.LinkText("//li[text(),'lienedarta+Test1@gmail.com']"));

            Console.WriteLine("Test Passed");
        }

        [Test]
        public void RegistrateNewUser_PasswordInputsDontMatch_ReturnMessageDontMatch()
        {
            driver.Url = test_url;


            System.Threading.Thread.Sleep(3000);


            IWebElement registerButton = driver.FindElement(By.CssSelector("[class = 'nav-link text-dark']"));

            registerButton.Click();

            System.Threading.Thread.Sleep(3000);

            IWebElement usernameField = driver.FindElement(By.CssSelector("[id='Input_UserName']"));

            usernameField.SendKeys("UITest1");

            IWebElement emailField = driver.FindElement(By.CssSelector("[id='Input_Email']"));

            emailField.SendKeys("lienedarta+Test1@gmail.com");

            IWebElement passwordField = driver.FindElement(By.CssSelector("[id='Input_Password']"));

            passwordField.SendKeys("zxcvbnm");

            IWebElement passwordConfirm = driver.FindElement(By.CssSelector("[id='Input_ConfirmPassword']"));

            passwordConfirm.SendKeys("zxcvbn");

            IWebElement registerConfirmButton = driver.FindElement(By.CssSelector("[class = 'btn btn-primary']"));

            registerConfirmButton.Click();

            System.Threading.Thread.Sleep(6000);

            var findTextTakenUser = driver.FindElements(By.LinkText("//li[text(),'The password and confirmation password do not match.']"));

            Console.WriteLine("Test Passed");
        }

        [TearDown]
        public void CloseBrowser()
        {
            driver.Quit();
        }
    }
}