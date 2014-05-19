using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlUtilities;

namespace XmlUtilitiesTests
{
    /// <summary>
    /// Summary description for ConfigurationManagerHelperTests
    /// </summary>
    [TestClass]
    public class ConfigurationManagerHelperTests
    {
        public ConfigurationManagerHelperTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void GetAppSettingReturnsDefaultString()
        {
            ConfigurationManagerHelper config = new ConfigurationManagerHelper();

            string defaultValue = "test";
            string expectedValue = config.GetAppSetting("key", defaultValue);

            Assert.AreEqual(defaultValue, expectedValue);
        }

        [TestMethod]
        public void GetAppSettingReturnsDefaultInt()
        {
            ConfigurationManagerHelper config = new ConfigurationManagerHelper();

            int defaultValue = 15;
            int expectedValue = config.GetAppSetting("key", defaultValue);

            Assert.AreEqual(defaultValue, expectedValue);
        }

        [TestMethod]
        public void GetAppSettingReturnsDefaultBool()
        {
            ConfigurationManagerHelper config = new ConfigurationManagerHelper();

            bool defaultValue = true;
            bool expectedValue = config.GetAppSetting("key", defaultValue);

            Assert.AreEqual(defaultValue, expectedValue);
        }

        [TestMethod]
        public void GetAppSettingReturnsDefaultUri()
        {
            ConfigurationManagerHelper config = new ConfigurationManagerHelper();

            Uri defaultValue = new Uri("http://www.google.com");
            Uri expectedValue = config.GetAppSetting("key", defaultValue);

            Assert.AreEqual(defaultValue, expectedValue);
        }
    }
}
