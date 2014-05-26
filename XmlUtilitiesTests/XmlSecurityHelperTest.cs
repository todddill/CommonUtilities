using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using XmlUtilities;
using System.Xml;

namespace XmlUtilitiesTests
{
    /// <summary>
    /// Summary description for XmlSecurityHelperTest
    /// </summary>
    [TestClass]
    public class XmlSecurityHelperTest
    {
        public XmlSecurityHelperTest()
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
        public void NoExpansionAttackParses()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<PARTS>");
            sb.AppendLine("<TITLE>Computer Parts</TITLE>");
            sb.AppendLine("<PART>");
            sb.AppendLine("<ITEM>Motherboard</ITEM>");
            sb.AppendLine("<MANUFACTURER>ASUS</MANUFACTURER>");
            sb.AppendLine("<MODEL>P3B-F</MODEL>");
            sb.AppendLine("<COST> 123.00</COST>");
            sb.AppendLine("</PART>");
            sb.AppendLine("</PARTS>");

            StringReader stringReader = new StringReader(sb.ToString());

            XmlReaderSettings settings = new XmlReaderSettings();
            XmlSecurityHelper.ControlExpansionAttack(settings);

            XmlReader reader = XmlReader.Create(stringReader, settings);

            string root = string.Empty;

            while (reader.Read())
            {
                root = reader.Name;
                break;
            }

            Assert.IsTrue(root.Equals("PARTS"));
        }

        [TestMethod]
        [ExpectedException(typeof(XmlException))]
        public void ControlExpansionAttack()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE foo [");
            sb.AppendLine("<!ENTITY a \"1234567890\" >");
            sb.AppendLine("<!ENTITY b \"&a;&a;&a;&a;&a;&a;&a;&a;\" >");
            sb.AppendLine("<!ENTITY c \"&b;&b;&b;&b;&b;&b;&b;&b;\" >");
            sb.AppendLine("<!ENTITY d \"&c;&c;&c;&c;&c;&c;&c;&c;\" >");
            sb.AppendLine("<!ENTITY e \"&d;&d;&d;&d;&d;&d;&d;&d;\" >");
            sb.AppendLine("<!ENTITY f \"&e;&e;&e;&e;&e;&e;&e;&e;\" >");
            sb.AppendLine("<!ENTITY g \"&f;&f;&f;&f;&f;&f;&f;&f;\" >");
            sb.AppendLine("<!ENTITY h \"&g;&g;&g;&g;&g;&g;&g;&g;\" >");
            sb.AppendLine("<!ENTITY i \"&h;&h;&h;&h;&h;&h;&h;&h;\" >");
            sb.AppendLine("<!ENTITY j \"&i;&i;&i;&i;&i;&i;&i;&i;\" >");
            sb.AppendLine("<!ENTITY k \"&j;&j;&j;&j;&j;&j;&j;&j;\" >");
            sb.AppendLine("<!ENTITY l \"&k;&k;&k;&k;&k;&k;&k;&k;\" >");
            sb.AppendLine("<!ENTITY m \"&l;&l;&l;&l;&l;&l;&l;&l;\" >");
            sb.AppendLine("]>");
            sb.AppendLine("<foo>&m;</foo>");

            StringReader stringReader = new StringReader(sb.ToString());

            XmlReaderSettings settings = new XmlReaderSettings();
            XmlSecurityHelper.ControlExpansionAttack(settings);

            XmlReader reader = XmlReader.Create(stringReader, settings);
            while (reader.Read()) ;

            Assert.Fail("Expected exception not thrown.");
        }
    }
}
