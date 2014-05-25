using System.Data.Common;

namespace DataObjectsWrapper
{
    /// <summary>
    /// 
    /// </summary>
    public struct DataAccessParameters
    {
        DataProviderType _dataProviderType;

        /// <summary>
        /// Gets or sets the type of the data provider.
        /// </summary>
        /// <value>The type of the data provider.</value>
        public DataProviderType DataProviderType
        {
            get { return _dataProviderType; }
            set { _dataProviderType = value; }
        }

        DbConnectionStringBuilder _connectionString;

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public DbConnectionStringBuilder ConnectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns>
        /// A 32-bit signed integer that is the hash code for this instance.
        /// </returns>
        public override int GetHashCode()
        {
            return _connectionString.GetHashCode() ^ _dataProviderType.GetHashCode();
        }

        /// <summary>
        /// Indicates whether this instance and a specified object are equal.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        /// true if <paramref name="obj"/> and this instance are the same type and represent the same value; otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (!(obj is DataAccessParameters))
                return false;
            return Equals((DataAccessParameters)obj);
        }

        private bool Equals(DataAccessParameters dataAccessParameters)
        {
            if (_connectionString != dataAccessParameters._connectionString)
                return false;

            return _dataProviderType == dataAccessParameters._dataProviderType;
        }

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="dataAccessParameters1">The data access parameters1.</param>
        /// <param name="dataAccessParameters2">The data access parameters2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator ==(DataAccessParameters dataAccessParameters1, DataAccessParameters dataAccessParameters2)
        {
            return dataAccessParameters1.Equals(dataAccessParameters2);
        }

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="dataAccessParameters1">The data access parameters1.</param>
        /// <param name="dataAccessParameters2">The data access parameters2.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator !=(DataAccessParameters dataAccessParameters1, DataAccessParameters dataAccessParameters2)
        {
            return !dataAccessParameters1.Equals(dataAccessParameters2);
        }
    }
}
