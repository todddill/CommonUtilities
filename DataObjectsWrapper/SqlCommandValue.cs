using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace DataObjectsWrapper
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlCommandValue : IDisposable
    {
        private string _commandText = string.Empty;
        private Hashtable _outputParams = new Hashtable();

        /// <summary>
        /// Gets the output params.
        /// </summary>
        /// <value>The output params.</value>
        /// <remarks>
        /// Author:  tdill
        /// Date:  7/8/2009
        /// Notes:
        /// </remarks>
        public Hashtable OutputParams
        {
            get { return _outputParams; }
        }

        /// <summary>
        /// Gets or sets the command text.
        /// </summary>
        /// <value>The command text.</value>
        public string CommandText
        {
            get { return _commandText; }
            set { _commandText = value; }
        }

        private CommandType _commandType;

        /// <summary>
        /// Gets or sets the type of the command.
        /// </summary>
        /// <value>The type of the command.</value>
        public CommandType CommandType
        {
            get { return _commandType; }
            set { _commandType = value; }
        }
        private List<SqlParameter> _parameters = new List<SqlParameter>();

        /// <summary>
        /// Gets or sets the parameters.
        /// </summary>
        /// <value>The parameters.</value>
        public List<SqlParameter> Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Adds the parameter.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <param name="output">if set to <c>true</c> [output].</param>
        public void AddParameter(SqlParameter parameter, bool output)
        {
            if (output)
            {
                _outputParams.Add(parameter.ParameterName, null);
                parameter.Direction = ParameterDirection.Output;
            }
            _parameters.Add(parameter);
        }

        /// <summary>
        /// Gets the output parameters.
        /// </summary>
        /// <returns></returns>
        public Hashtable GetOutputParameters()
        {
            Hashtable outputParams = (Hashtable)_outputParams.Clone();
            foreach (string key in _outputParams.Keys)
            {
                outputParams[key] = _parameters.Find(delegate(SqlParameter param) { return param.ParameterName.Equals(key); }).Value;
            }
            return outputParams;
        }

        #region Implementation of the Disposal Pattern
        private bool _alreadyDisposed;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>
        /// tdill - 4/9/2008 12:50 PM
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="isDisposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// <remarks>
        /// tdill - 4/9/2008 12:50 PM
        /// </remarks>
        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDisposed) return;
            if (isDisposing)
            {
                //TODO: free managed resources here.
                _outputParams = null;
            }
            // TODO:  free unmanaged resources here.
            _alreadyDisposed = true;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="SqlCommandValue"/> is reclaimed by garbage collection.
        /// </summary>
        /// <remarks>
        /// tdill - 4/9/2008 12:51 PM
        /// </remarks>
        ~SqlCommandValue()
        {
            Dispose(false);
        }
        #endregion
    }

    /// <summary>
    /// Custom exception for SqlCommandValue object
    /// </summary>
    /// <remarks>
    /// Author:  tdill
    /// Date:  7/6/2009
    /// Notes:
    /// </remarks>
    [Serializable()]
    public class SqlCommandValueException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCommandValueException"/> class.
        /// </summary>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public SqlCommandValueException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCommandValueException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public SqlCommandValueException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCommandValueException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public SqlCommandValueException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlCommandValueException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        /// <remarks>
        /// tdill - 4/22/2008 9:59 AM
        /// </remarks>
        protected SqlCommandValueException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
