using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DomainUtilities;

namespace DomainUtilitiesTests
{
    /// <summary>
    /// Summary description for FullNameParserTests
    /// </summary>
    [TestClass]
    public class FullNameParserTests
    {
        public FullNameParserTests()
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
         	
        //Mary White, Mary Jo White, Dr. Mary Jo White, Dr. Mary Jo T. White, Ph.D.

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void FullNameParserOnlyFirst()
        {
            Name expected = CreateNameObject("first");
            Name actual = FullNameParser.Parse(expected.ToString());

            Assert.AreEqual(actual.First, expected.First);
            Assert.AreEqual(actual.Last, expected.Last);
            Assert.AreEqual(actual.Title, expected.Title);
            Assert.AreEqual(actual.Suffix, expected.Suffix);
            Assert.AreEqual(actual.Middle, expected.Middle);
        }

        [TestMethod]
        public void FullNameParserFirstAndLast()
        {
            Name expected = CreateNameObject("first", "last");
            Name actual = FullNameParser.Parse(expected.ToString());

            Assert.AreEqual(actual.First, expected.First);
            Assert.AreEqual(actual.Last, expected.Last);
            Assert.AreEqual(actual.Title, expected.Title);
            Assert.AreEqual(actual.Suffix, expected.Suffix);
            Assert.AreEqual(actual.Middle, expected.Middle);
        }

        [TestMethod]
        public void FullNameParserFirstMiddleLast()
        {
            Name expected = CreateNameObject("first", "last", "middle");
            Name actual = FullNameParser.Parse(expected.ToString());

            Assert.AreEqual(actual.First, expected.First);
            Assert.AreEqual(actual.Last, expected.Last);
            Assert.AreEqual(actual.Title, expected.Title);
            Assert.AreEqual(actual.Suffix, expected.Suffix);
            Assert.AreEqual(actual.Middle, expected.Middle);
        }

        [TestMethod]
        public void FullNameParserTitleFirstLast()
        {
            Name expected = CreateNameObject("first", "last", "title");
            Name actual = FullNameParser.Parse(expected.ToString());

            Assert.AreEqual(actual.First, expected.First);
            Assert.AreEqual(actual.Last, expected.Last);
            Assert.AreEqual(actual.Title, expected.Title);
            Assert.AreEqual(actual.Suffix, expected.Suffix);
            Assert.AreEqual(actual.Middle, expected.Middle);
        }

        public Name CreateNameObject(params string[] elements)
        {
            Name name = new Name();

            foreach (string element in elements)
            {
                if (element.Equals("title")) name.Title = "Dr.";
                if (element.Equals("first")) name.First = "Peter";
                if (element.Equals("middle")) name.Middle = "A.";
                if (element.Equals("last")) name.Last = "Gibbons";
                if (element.Equals("suffix")) name.Suffix = "Jr.";
            }

            return name;
        }
    }
}
