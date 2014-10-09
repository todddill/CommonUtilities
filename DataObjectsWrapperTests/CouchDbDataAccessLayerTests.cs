using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataObjectsWrapper;
using Newtonsoft.Json;
using System.IO;

namespace DataObjectsWrapperTests
{
    [TestClass]
    public class CouchDbDataAccessLayerTests
    {
        [TestMethod]
        public void testurl_equals_example_com()
        {
            CouchDbDataAccessLayer dal = new CouchDbDataAccessLayer("http://localhost:5984/sample_config");
            string expected = "http://www.example.com";

            string actual = dal.GetJsonValue("4ef4a8097e1f9e72022a2a7303000daa", "testurl");

            Assert.AreEqual(expected, actual);
        }
    }
}
