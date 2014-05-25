using System;
using System.Data;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.XPath;

namespace DataObjectsWrapper.Utility
{
    /// <summary>
    /// Helper class for the System.Xml namespace.
    /// </summary>
    /// <remarks>
    /// Author:  tdill
    /// Date:  7/6/2009
    /// Notes:
    /// </remarks>
    public static class XmlData
    {
        /// <summary>
        /// Gets the XML document.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static IXPathNavigable GetXmlDocument(DataTable table)
        {
            XmlDocument doc = new XmlDocument();
            using (MemoryStream ms = new MemoryStream())
            {
                table.WriteXml(ms, XmlWriteMode.IgnoreSchema);
                ms.Position = 0;

                XmlReaderSettings settings = new XmlReaderSettings();
                settings.IgnoreComments = true;
                settings.IgnoreProcessingInstructions = true;
                settings.IgnoreWhitespace = true;

                XmlReader reader = XmlReader.Create(ms, settings);

                doc.Load(reader);
            }
            return doc;
        }
    }

    /// <summary>
    /// Custom exception for XmlHelper object.
    /// </summary>
    /// <remarks>
    /// Author:  tdill
    /// Date:  7/6/2009
    /// Notes:
    /// </remarks>
    [Serializable()]
    public class XmlHelperException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlHelperException"/> class.
        /// </summary>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public XmlHelperException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlHelperException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public XmlHelperException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlHelperException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public XmlHelperException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="XmlHelperException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        /// <remarks>
        /// tdill - 4/22/2008 9:59 AM
        /// </remarks>
        protected XmlHelperException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
