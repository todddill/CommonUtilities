using System;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using CommonUtilitiesTestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlUtilities;

namespace XmlUtilitiesTests
{
    [TestClass]
    public class XmlSerializationHelperTests
    {
        private Vehicle _vehicle = new Vehicle
        {
            Doors = 4,
            Wheels = "22 inch",
            Windows = "Tinted",
            Engine = Engine.V6,
            ManufactureTimestamp = Convert.ToDateTime("2014-11-24T06:04:02.000000")
        };

        [TestMethod]
        public void SerializeObjectToXmlString()
        {
            string xml = XmlSerializationHelper.Serialize<Vehicle>(_vehicle);
            Assert.IsTrue(xml.Contains("<Wheels>22 inch</Wheels>"));
            Assert.IsTrue(xml.Contains("<ManufactureTimestamp>2014-11-24T06:04:02.000000</ManufactureTimestamp>"));
        }

        [TestMethod]
        public void SerializeObjectToXmlStringOmitDeclaration()
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
            };

            string xml = XmlSerializationHelper.Serialize<Vehicle>(_vehicle, settings);
            Assert.IsFalse(xml.Contains("﻿<?xml version=\"1.0\" encoding=\"utf-16\"?>"));
        }

        [TestMethod]
        public void DeserializeXmlStringToObject()
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = false,
                Encoding = Encoding.UTF8
            };

            string xml = XmlSerializationHelper.Serialize<Vehicle>(_vehicle, settings, new MemoryStreamHelper());

            Vehicle actual = XmlSerializationHelper.Deserialize<Vehicle>(xml);
            Assert.IsTrue(actual.Doors == 4);
        }

        [TestMethod]
        public void DeserializeXmlStringToObjectWithDifferentRoot()
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = false,
                Encoding = Encoding.UTF8
            };

            string xml = XmlSerializationHelper.Serialize<Vehicle>(_vehicle, settings, new MemoryStreamHelper());

            XmlRootAttribute root = new XmlRootAttribute { 
                DataType = "Truck",
                ElementName = "Vehicle",
                IsNullable = false
            };

            Truck actual = XmlSerializationHelper.Deserialize<Truck>(xml, Encoding.Unicode, root);
            Assert.IsTrue(actual.DriveTrain == default(DriveTrain));
        }
    }
}
