using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace DataObjectsWrapper
{
    /// <summary>
    /// The SQLDataAccessLayer contains the data access layer for Microsoft SQL Server. 
    /// This class implements the abstract methods in the DataAccessLayerBaseClass class.
    /// </summary>
    public class SqlDataAccessLayer : DataAccessLayerBaseClass
    {
        // Provide class constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessLayer"/> class.
        /// </summary>
        /// <remarks>
        /// Author:  tdill
        /// Date:  7/6/2009
        /// Notes:
        /// </remarks>
        public SqlDataAccessLayer() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessLayer"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <remarks>
        /// Author:  tdill
        /// Date:  7/6/2009
        /// Notes:
        /// </remarks>
        public SqlDataAccessLayer(string connectionString) { this.ConnectionString = connectionString; }

        // DataAccessLayerBaseClass Members
        /// <summary>
        /// Data provider specific implementation for accessing relational databases.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Author:  tdill
        /// Date:  7/6/2009
        /// Notes:
        /// </remarks>
        internal override IDbConnection GetDataProviderConnection()
        {
            return new SqlConnection();
        }
        /// <summary>
        /// Data provider specific implementation for executing SQL statement while connected to a data source.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Author:  tdill
        /// Date:  7/6/2009
        /// Notes:
        /// </remarks>
        internal override IDbCommand GetDataProviderCommand()
        {
            return new SqlCommand();
        }

        /// <summary>
        /// Data provider specific implementation for filling the DataSet.
        /// </summary>
        /// <returns></returns>
        /// <remarks>
        /// Author:  tdill
        /// Date:  7/6/2009
        /// Notes:
        /// </remarks>
        internal override IDbDataAdapter GetDataProviderDataAdapter()
        {
            return new SqlDataAdapter();
        }

        #region BulkCopy

        /// <summary>
        /// Copies the data.
        /// </summary>
        /// <param name="sourceTable">The source table.</param>
        /// <param name="destinationTableName">Name of the destination table.</param>
        /// <param name="columnmappings">The columnmappings.</param>
        /// <remarks>
        /// Author:  tdill
        /// Date:  7/6/2009
        /// Notes:
        /// </remarks>
        public void CopyData(DataTable sourceTable, string destinationTableName, List<string[]> columnmappings)
        {
            // new method: SQLBulkCopy:
            using (SqlBulkCopy s = new SqlBulkCopy(this.ConnectionString))
            {
                s.DestinationTableName = destinationTableName;

                foreach (string[] pair in columnmappings)
                {
                    s.ColumnMappings.Add(pair[0], pair[1]);
                }
                s.WriteToServer(sourceTable);
                s.Close();
            }
        }

        #endregion
    }

    /// <summary>
    /// Custom exception during sql database access.
    /// </summary>
    /// <remarks>
    /// Author:  tdill
    /// Date:  7/6/2009
    /// Notes:
    /// </remarks>
    [Serializable()]
    public class SqlDataAccessException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessException"/> class.
        /// </summary>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public SqlDataAccessException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public SqlDataAccessException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public SqlDataAccessException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlDataAccessException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        /// <remarks>
        /// tdill - 4/22/2008 9:59 AM
        /// </remarks>
        protected SqlDataAccessException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}

