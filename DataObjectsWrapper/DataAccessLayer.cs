using System;
using System.Configuration;
using System.Data;
using System.Runtime.Serialization;

namespace DataObjectsWrapper
{
    /// <summary>
    /// Defines the DataAccessLayer implemented data provider types.
    /// </summary>
    public enum DataProviderType
    {
        /// <summary>
        /// Flag to tell the factory to create an access data provider
        /// </summary>
        Access,
        /// <summary>
        /// Flag to tell the factory to create an odbc data provider
        /// </summary>
        Odbc,
        /// <summary>
        /// Flag to tell the factory to create an oledb data provider
        /// </summary>
        OleDb,
        /// <summary>
        /// Flag to tell the factory to create an Oracle data provider
        /// </summary>
        Oracle,
        /// <summary>
        /// Flag to tell the factory to create an sql data provider
        /// </summary>
        Sql,
        /// <summary>
        /// Flag to tell the factory to create an mysql data provider
        /// </summary>
        MySql,
        /// <summary>
        /// Flag to tell the factory to create a mock data provider
        /// </summary>
        Mock
    }

    /// <summary>
    /// The DataAccessLayerBaseClass lists all the abstract methods that each data access layer provider (SQL Server, OleDb, etc.) must implement.
    /// </summary>
    public abstract class DataAccessLayerBaseClass : IDisposable
    {
        #region private data members, methods & constructors

        // Private Members

        private string _connectionString;
        private IDbConnection _connection;
        private IDbCommand _command;
        private IDbTransaction _transaction;
        private int _commandTimeout = 60;

        /// <summary>
        /// Gets or sets the command timeout.
        /// </summary>
        /// <value>The command timeout.</value>
        public int CommandTimeout
        {
            get { return _commandTimeout; }
            set { _commandTimeout = value; }
        }

        /// <summary>
        /// Gets or sets the string used to open a database.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                // make sure conection string is not empty
                if (string.IsNullOrEmpty(_connectionString))
                    throw new DataAccessLayerException("Invalid database connection string.");
                return _connectionString;
            }
            set
            {
                _connectionString = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessLayerBaseClass"/> class.
        /// </summary>
        /// <remarks>
        /// Author:  tdill
        /// Date:  7/6/2009
        /// Notes:
        /// </remarks>
        protected DataAccessLayerBaseClass() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessLayerBaseClass"/> class.
        /// </summary>
        /// <param name="commandTimeout">The command timeout.</param>
        protected DataAccessLayerBaseClass(int commandTimeout)
        {
            _commandTimeout = commandTimeout;
        }


        /// <summary>
        /// This method opens (if necessary) and assigns a connection, transaction, command type and parameters 
        /// to the provided command.
        /// </summary>
        private void PrepareCommand(CommandType commandType, string commandText, IDataParameter[] commandParameters)
        {
            // provide the specific data provider connection object, if the connection object is null
            if (_connection == null)
            {
                _connection = GetDataProviderConnection();
                _connection.ConnectionString = this.ConnectionString;
            }

            // if the provided connection is not open, then open it
            if (_connection.State != ConnectionState.Open)
                _connection.Open();

            // Provide the specific data provider command object, if the command object is null
            if (_command == null)
                _command = GetDataProviderCommand();

            // associate the connection with the command
            _command.Connection = _connection;
            // set the command text (stored procedure name or SQL statement)
            _command.CommandText = commandText;
            // set the command type
            _command.CommandType = commandType;
            _command.CommandType = commandType;
            // set the command Timeout
            _command.CommandTimeout = _commandTimeout;

            // if a transaction is provided, then assign it.
            if (_transaction != null)
                _command.Transaction = _transaction;

            // attach the command parameters if they are provided
            if (commandParameters != null)
            {
                foreach (IDataParameter param in commandParameters)
                    _command.Parameters.Add(param);
            }
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Data provider specific implementation for accessing relational databases.
        /// </summary>
        internal abstract IDbConnection GetDataProviderConnection();
        /// <summary>
        /// Data provider specific implementation for executing SQL statement while connected to a data source.
        /// </summary>
        internal abstract IDbCommand GetDataProviderCommand();
        /// <summary>
        /// Data provider specific implementation for filling the DataSet.
        /// </summary>
        internal abstract IDbDataAdapter GetDataProviderDataAdapter();

        #endregion

        // Generic methods implementation

        #region Database Transaction

        /// <summary>
        /// Begins a database transaction.
        /// </summary>
        public void BeginTransaction()
        {
            if (_transaction != null)
                return;

            try
            {
                // instantiate a connection object
                _connection = GetDataProviderConnection();
                _connection.ConnectionString = this.ConnectionString;
                // open connection
                _connection.Open();
                // begin a database transaction with a read committed isolation level
                _transaction = _connection.BeginTransaction(IsolationLevel.ReadCommitted);
            }
            catch
            {
                if (_connection != null)
                    _connection.Close();
                throw;
            }
        }

        /// <summary>
        /// Commits the database transaction.
        /// </summary>
        public void CommitTransaction()
        {
            if (_transaction == null)
                return;

            try
            {
                // Commit transaction
                _transaction.Commit();
            }
            catch
            {
                // rollback transaction
                RollbackTransaction();
                throw;
            }
            finally
            {
                _transaction = null;
            }
        }

        /// <summary>
        /// Rolls back a transaction from a pending state.
        /// </summary>
        public void RollbackTransaction()
        {
            if (_transaction == null)
                return;

            try
            {
                _transaction.Rollback();
            }
            catch
            {
                throw;
            }
            finally
            {
                _transaction = null;
            }
        }

        #endregion

        #region ExecuteDataReader

        /// <summary>
        /// Executes the CommandText against the Connection and builds an IDataReader.
        /// </summary>
        public IDataReader ExecuteDataReader(string commandText)
        {
            return this.ExecuteDataReader(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Executes the CommandText against the Connection and builds an IDataReader.
        /// </summary>
        public IDataReader ExecuteDataReader(string commandText, CommandType commandType)
        {
            return this.ExecuteDataReader(commandText, commandType, null);
        }

        /// <summary>
        /// Executes a parameterized CommandText against the Connection and builds an IDataReader.
        /// </summary>
        public IDataReader ExecuteDataReader(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteDataReader(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Executes a stored procedure against the Connection and builds an IDataReader.
        /// </summary>
        public IDataReader ExecuteDataReader(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);

                IDataReader dr = _command.ExecuteReader();
                return dr;
            }
            catch (Exception ex)
            {
                if (_transaction != null)
                    RollbackTransaction();
                else
                    if (_command != null)
                        _command.Dispose();

                throw new DataAccessLayerException(ex.Message);
            }
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        public DataSet ExecuteDataSet(string commandText)
        {
            return this.ExecuteDataSet(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType)
        {
            return this.ExecuteDataSet(commandText, commandType, null);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        public DataSet ExecuteDataSet(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteDataSet(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the data source using the DataSet name, and creates a DataTable named "Table".
        /// </summary>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            DataSet ds = new DataSet();
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);
                //create the DataAdapter & DataSet
                IDbDataAdapter da = GetDataProviderDataAdapter();
                da.SelectCommand = _command;


                //fill the DataSet using default values for DataTable names, etc.
                da.Fill(ds);

                //return the dataset
                return ds;
            }
            catch (Exception ex)
            {
                if (_transaction != null)
                    RollbackTransaction();
                ds.Dispose();

                throw new DataAccessLayerException(ex.Message);
            }
            finally
            {
                if (_transaction == null)
                {
                    _connection.Close();
                    if (_command != null)
                        _command.Dispose();
                }
            }
        }

        #endregion

        #region ExecuteQuery

        /// <summary>
        /// Executes an SQL statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        public int ExecuteQuery(string commandText)
        {
            return this.ExecuteQuery(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Executes an SQL statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        public int ExecuteQuery(string commandText, CommandType commandType)
        {
            return this.ExecuteQuery(commandText, commandType, null);
        }

        /// <summary>
        /// Executes an SQL parameterized statement against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        public int ExecuteQuery(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteQuery(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Executes a stored procedure against the Connection object of a .NET Framework data provider, and returns the number of rows affected.
        /// </summary>
        public int ExecuteQuery(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);

                // execute command
                int intAffectedRows = _command.ExecuteNonQuery();
                // return no of affected records
                return intAffectedRows;
            }
            catch (Exception ex)
            {
                if (_transaction != null)
                    RollbackTransaction();

                throw new DataAccessLayerException(ex.Message);
            }
            finally
            {
                if (_transaction == null)
                {
                    _connection.Close();
                    if (_command != null)
                        _command.Dispose();
                }
            }
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        public object ExecuteScalar(string commandText)
        {
            return this.ExecuteScalar(commandText, CommandType.Text, null);
        }

        /// <summary>
        /// Executes the query, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        public object ExecuteScalar(string commandText, CommandType commandType)
        {
            return this.ExecuteScalar(commandText, commandType, null);
        }

        /// <summary>
        /// Executes a parameterized query, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        public object ExecuteScalar(string commandText, IDataParameter[] commandParameters)
        {
            return this.ExecuteScalar(commandText, CommandType.Text, commandParameters);
        }

        /// <summary>
        /// Executes a stored procedure, and returns the first column of the first row in the resultset returned by the query. Extra columns or rows are ignored.
        /// </summary>
        public object ExecuteScalar(string commandText, CommandType commandType, IDataParameter[] commandParameters)
        {
            try
            {
                PrepareCommand(commandType, commandText, commandParameters);

                // execute command
                object objValue = _command.ExecuteScalar();
                // check on value
                if (objValue != DBNull.Value)
                    // return value
                    return objValue;
                else
                    // return null instead of dbnull value
                    return null;
            }
            catch (Exception ex)
            {
                if (_transaction != null)
                    RollbackTransaction();

                throw new DataAccessLayerException(ex.Message);
            }
            finally
            {
                if (_transaction == null)
                {
                    _connection.Close();
                    if (_command != null)
                        _command.Dispose();
                }
            }
        }

        #endregion

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
                if (_connection != null)
                    _connection.Dispose();
            }
            // TODO:  free unmanaged resources here.
            _alreadyDisposed = true;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="DataAccessLayerBaseClass"/> is reclaimed by garbage collection.
        /// </summary>
        /// <remarks>
        /// tdill - 4/9/2008 12:51 PM
        /// </remarks>
        ~DataAccessLayerBaseClass()
        {
            Dispose(false);
        }
        #endregion
    }


    /// <summary>
    /// Loads different data access layer provider depending on the configuration settings file or the caller defined data provider type.
    /// </summary>
    public sealed class DataAccessLayerFactory
    {

        // Since this class provides only static methods, make the default constructor private to prevent 
        // instances from being created with "new DataAccessLayerFactory()"
        private DataAccessLayerFactory() { }

        /// <summary>
        /// Constructs a data access layer data provider based on application configuration settings.
        /// Application configuration file must contain two keys: 
        ///		1. "DataProviderType" key, with one of the DataProviderType enumerator.
        ///		2. "ConnectionString" key, holds the database connection string.
        /// </summary>
        public static DataAccessLayerBaseClass GetDataAccessLayer()
        {
            // Make sure application configuration file contains required configuration keys
            if (ConfigurationManager.AppSettings["DataProviderType"] == null
                || ConfigurationManager.AppSettings["ConnectionString"] == null)
                throw new DataAccessLayerException("Please specify a 'DataProviderType' and 'ConnectionString' configuration keys in the application configuration file.");

            DataProviderType dataProvider;

            try
            {
                // try to parse the data provider type from configuration file
                dataProvider =
                    (DataProviderType)System.Enum.Parse(typeof(DataProviderType),
                    ConfigurationManager.AppSettings["DataProviderType"].ToString(),
                    true);
            }
            catch
            {
                throw new DataAccessLayerException("Invalid data access layer provider type.");
            }

            // return data access layer provider
            return GetDataAccessLayer(
                dataProvider,
                ConfigurationManager.AppSettings["ConnectionString"].ToString());
        }

        /// <summary>
        /// Constructs a data access layer based on caller specific data provider.
        /// Caller of this method must provide the database connection string through ConnectionString property.
        /// </summary>
        public static DataAccessLayerBaseClass GetDataAccessLayer(DataProviderType dataProviderType)
        {
            return GetDataAccessLayer(dataProviderType, null);
        }

        /// <summary>
        /// Constructs a data access layer data provider.
        /// </summary>
        public static DataAccessLayerBaseClass GetDataAccessLayer(DataProviderType dataProviderType, string connectionString)
        {
            // construct specific data access provider class
            switch (dataProviderType)
            {
                case DataProviderType.Sql:
                    return new SqlDataAccessLayer(connectionString);
                case DataProviderType.Odbc:
                    return new OdbcDataAccessLayer(connectionString);
                case DataProviderType.OleDb:
                    return new OleDbDataAccessLayer(connectionString);
                case DataProviderType.Oracle:
                    return new OracleDataAccessLayer(connectionString);
                default:
                    throw new DataAccessLayerException("Invalid data access layer provider type.");
            }
        }

        /// <summary>
        /// Gets the data access layer.
        /// </summary>
        /// <param name="dataProviderType">Type of the data provider.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public static DataAccessLayerBaseClass GetDataAccessLayer(DataAccessParameters parameters)
        {
            return GetDataAccessLayer(parameters.DataProviderType, parameters.ConnectionString.ToString());
        }
    }

    /// <summary>
    /// Custom exception for the data access layer object
    /// </summary>
    /// <remarks>
    /// Author:  tdill
    /// Date:  7/6/2009
    /// Notes:
    /// </remarks>
    [Serializable()]
    public class DataAccessLayerException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessLayerException"/> class.
        /// </summary>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public DataAccessLayerException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessLayerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public DataAccessLayerException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessLayerException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public DataAccessLayerException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataAccessLayerException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        /// <remarks>
        /// tdill - 4/22/2008 9:59 AM
        /// </remarks>
        protected DataAccessLayerException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
