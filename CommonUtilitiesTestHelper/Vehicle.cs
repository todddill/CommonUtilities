using System;
using System.Xml.Serialization;

namespace CommonUtilitiesTestHelper
{
    [Serializable]
    [XmlRoot("Vehicle")]
    public class Vehicle
    {
        [XmlElement(IsNullable = true)]
        public string Wheels { get; set; }

        [XmlElement(IsNullable = true)]
        public string Windows { get; set; }

        [XmlElement]
        public int Doors { get; set; }

        [XmlElement]
        public Engine Engine { get; set; }
    }

    [Serializable]
    [XmlType]
    public enum Engine
    {
        [XmlEnum]
        I4,

        [XmlEnum]
        I6,

        [XmlEnum]
        V6,

        [XmlEnum]
        V8
    }
}
