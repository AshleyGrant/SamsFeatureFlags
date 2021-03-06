using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using OpenQA.Selenium.Chrome;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Linq;
using OpenQA.Selenium;
using System.Threading;
using System.Collections;
using System;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.KeyVault;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace FeatureFlags.FunctionalTests.Website
{
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    [TestClass]
    public class WebsiteSmokeTest
    {
        private ChromeDriver _driver;
        private TestContext _testContextInstance;
        private string _webUrl = null;
        private string _keyVaultURL = null;
        private string _keyVaultClientId = null;
        private string _keyVaultClientSecret = null;
        private string _environment = null;

        [TestMethod]
        [TestCategory("SkipWhenLiveUnitTesting")]
        [TestCategory("SmokeTest")]
        public void GotoFeatureFlagsWebHomeIndexPageTest()
        {
            //Arrange
            bool webLoaded;

            //Act
            string webURL = _webUrl + "home";
            Console.WriteLine("webURL:" + webURL);
            _driver.Navigate().GoToUrl(webURL);
            webLoaded = (_driver.Url == webURL);
            OpenQA.Selenium.IWebElement data = _driver.FindElementByXPath(@"/html/body/div/main/div/h1");
            Console.WriteLine("data:" + data.Text);
            Console.WriteLine("environment:" + _environment);

            //Assert
            Assert.IsTrue(webLoaded);
            Assert.IsTrue(data != null);
            //Assert.AreEqual(data.Text, "SamLearnsAzure Feature Flags management: " + _environment);
            Assert.AreEqual(data.Text, "SamLearnsAzure Feature Flags management");
        }

        [TestMethod]
        [TestCategory("SkipWhenLiveUnitTesting")]
        [TestCategory("SmokeTest")]
        public void GotoFeatureFlagsWebAboutPageTest()
        {
            //Arrange
            bool webLoaded;

            //Act
            string webURL = _webUrl + "home/about";
            Console.WriteLine("webURL:" + webURL);
            _driver.Navigate().GoToUrl(webURL);
            webLoaded = (_driver.Url == webURL);
            OpenQA.Selenium.IWebElement data = _driver.FindElementByXPath(@"/html/body/div/main/h1");
            Console.WriteLine("data:" + data.Text);

            //Assert
            Assert.IsTrue(webLoaded);
            Assert.IsTrue(data != null);
            Assert.IsTrue(data.Text == "About");
        }

        [TestInitialize]
        public void SetupTests()
        {
            ChromeOptions chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");
            _driver = new ChromeDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), chromeOptions);

            if (TestContext.Properties == null || TestContext.Properties.Count == 0)
            {
                throw new Exception("Select test settings file to continue");
            }
            else
            {
                _webUrl = TestContext.Properties["WebsiteUrl"].ToString();
                _keyVaultURL = TestContext.Properties["KeyVaultURL"].ToString();
                _keyVaultClientId = TestContext.Properties["KeyVaultClientId"].ToString();
                _keyVaultClientSecret = TestContext.Properties["KeyVaultClientSecret"].ToString();
                _environment = TestContext.Properties["TestEnvironment"].ToString();
            }
        }

        public TestContext TestContext
        {
            get
            {
                return _testContextInstance;
            }
            set
            {
                _testContextInstance = value;
            }
        }

        [TestCleanup()]
        public void CleanupTests()
        {
            _driver.Quit();
        }
    }
}
