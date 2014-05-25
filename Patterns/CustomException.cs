using System;
using System.Runtime.Serialization;

namespace Patterns
{
    /// <summary>
    /// Sample exception
    /// </summary>
    /// <remarks>
    /// tdill - 4/22/2008 9:59 AM
    /// </remarks>
    [Serializable()]
    public class MyException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MyException"/> class.
        /// </summary>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public MyException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public MyException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public MyException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        /// <remarks>
        /// tdill - 4/22/2008 9:59 AM
        /// </remarks>
        protected MyException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

