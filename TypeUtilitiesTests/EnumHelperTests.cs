using CommonUtilitiesTestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeUtilities;

namespace TypeUtilitiesTests
{
    /// <summary>
    /// Summary description for EnumHelperTests
    /// </summary>
    [TestClass]
    public class EnumHelperTests
    {
        public EnumHelperTests()
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
        public void ConvertStringToVehicleEngine()
        {
            string engine = "V6";

            Engine convertedEngine = EnumHelper.ConvertStringToEnum<Engine>(engine);

            Assert.IsTrue(convertedEngine == Engine.V6);
        }

        [TestMethod]
        public void ConvertStringToVehicleEngineCaseSensitive()
        {
            string engine = "v6";

            Engine convertedEngine = EnumHelper.ConvertStringToEnum<Engine>(engine, false);

            Assert.IsTrue(convertedEngine == default(Engine));
        }

        [TestMethod]
        public void ConvertStringToVehicleEngineNotInList()
        {
            string engine = "V12";

            Engine convertedEngine = EnumHelper.ConvertStringToEnum<Engine>(engine);

            Assert.IsTrue(convertedEngine == default(Engine));
        }
    }
}
