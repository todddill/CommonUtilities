using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace DataObjectsWrapper.Concurrency
{
    public class ExclusiveReadLockManagerDatabaseCommandValues : IDisposable
    {
        private SqlCommandValue _acquireCommand;

        /// <summary>
        /// Gets or sets the acquire command.
        /// </summary>
        /// <value>
        /// The acquire command.
        /// </value>
        public SqlCommandValue AcquireCommand
        {
            get { return _acquireCommand; }
            set { _acquireCommand = value; }
        }
        private SqlCommandValue _releaseCommand;

        /// <summary>
        /// Gets or sets the release command.
        /// </summary>
        /// <value>The release command.</value>
        public SqlCommandValue ReleaseCommand
        {
            get { return _releaseCommand; }
            set { _releaseCommand = value; }
        }
        private SqlCommandValue _releaseAllCommand;

        /// <summary>
        /// Gets or sets the release all command.
        /// </summary>
        /// <value>The release all command.</value>
        public SqlCommandValue ReleaseAllCommand
        {
            get { return _releaseAllCommand; }
            set { _releaseAllCommand = value; }
        }
        private SqlCommandValue _findCommand;

        /// <summary>
        /// Gets or sets the find command.
        /// </summary>
        /// <value>The find command.</value>
        public SqlCommandValue FindCommand
        {
            get { return _findCommand; }
            set { _findCommand = value; }
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
                if (_acquireCommand != null) _acquireCommand.Dispose();
                if (_findCommand != null) _findCommand.Dispose();
                if (_releaseAllCommand != null) _releaseAllCommand.Dispose();
                if (_releaseCommand != null) _releaseCommand.Dispose();
            }
            // TODO:  free unmanaged resources here.
            _alreadyDisposed = true;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="ExclusiveReadLockManagerDatabaseCommandValues"/> is reclaimed by garbage collection.
        /// </summary>
        /// <remarks>
        /// tdill - 4/9/2008 12:51 PM
        /// </remarks>
        ~ExclusiveReadLockManagerDatabaseCommandValues()
        {
            Dispose(false);
        }
        #endregion
    }

    /// <summary>
    /// Object to manage locks stored in a lock table in the database
    /// </summary>
    public class ExclusiveReadLockManagerDatabase : ExclusiveLockManager, IDisposable
    {
        private SqlDataAccessLayer _dataAccessLayer;
        private SqlCommandValue _acquireCommand;
        private SqlCommandValue _releaseCommand;
        private SqlCommandValue _releaseAllCommand;
        private SqlCommandValue _findCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExclusiveReadLockManagerDatabase"/> class.
        /// </summary>
        /// <param name="dataAccessLayer">The data access layer.</param>
        /// <param name="commandValue">The command value.</param>
        public ExclusiveReadLockManagerDatabase(SqlDataAccessLayer dataAccessLayer, ExclusiveReadLockManagerDatabaseCommandValues commandValue)
        {
            _dataAccessLayer = dataAccessLayer;
            _acquireCommand = commandValue.AcquireCommand;
            _releaseCommand = commandValue.ReleaseCommand;
            _releaseAllCommand = commandValue.ReleaseAllCommand;
            _findCommand = commandValue.FindCommand;
        }

        /// <summary>
        /// Acquires the lock.
        /// </summary>
        internal override void AcquireLock()
        {
            _dataAccessLayer.ExecuteScalar(_acquireCommand.CommandText, _acquireCommand.CommandType, new List<SqlParameter>(_acquireCommand.Parameters).ToArray());
        }

        /// <summary>
        /// Releases the lock.
        /// </summary>
        internal override void ReleaseLock()
        {
            _dataAccessLayer.ExecuteScalar(_releaseCommand.CommandText, _releaseCommand.CommandType, new List<SqlParameter>(_releaseCommand.Parameters).ToArray());
        }

        /// <summary>
        /// Releases all locks.
        /// </summary>
        internal override void ReleaseAllLocks()
        {
            _dataAccessLayer.ExecuteScalar(_releaseAllCommand.CommandText, _releaseAllCommand.CommandType, new List<SqlParameter>(_releaseAllCommand.Parameters).ToArray());
        }

        /// <summary>
        /// Finds the lock.
        /// </summary>
        /// <param name="findOutputName">Name of the find output.</param>
        /// <returns></returns>
        internal override bool FindLock(string findOutputName)
        {
            _dataAccessLayer.ExecuteScalar(_findCommand.CommandText, _findCommand.CommandType, new List<SqlParameter>(_findCommand.Parameters).ToArray());
            return bool.Parse(_findCommand.GetOutputParameters()[findOutputName].ToString());
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
                _dataAccessLayer.Dispose();
            }
            // TODO:  free unmanaged resources here.
            _alreadyDisposed = true;
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="ExclusiveReadLockManagerDatabase"/> is reclaimed by garbage collection.
        /// </summary>
        /// <remarks>
        /// tdill - 4/9/2008 12:51 PM
        /// </remarks>
        ~ExclusiveReadLockManagerDatabase()
        {
            Dispose(false);
        }
        #endregion
    }

    /// <summary>
    /// Abstract object to manage locks in a lock table in the database
    /// </summary>
    public abstract class ExclusiveLockManager
    {
        /// <summary>
        /// Acquires the lock.
        /// </summary>
        internal abstract void AcquireLock();

        /// <summary>
        /// Releases the lock.
        /// </summary>
        internal abstract void ReleaseLock();

        /// <summary>
        /// Releases all locks.
        /// </summary>
        internal abstract void ReleaseAllLocks();

        /// <summary>
        /// Finds the lock.
        /// </summary>
        /// <param name="findOutputName">Name of the find output.</param>
        /// <returns></returns>
        internal abstract bool FindLock(string findOutputName);
    }

    /// <summary>
    /// Custom exception for the Pesimistic Offline Lock class
    /// </summary>
    /// <remarks>
    /// Author:  tdill
    /// Date:  7/6/2009
    /// Notes:
    /// </remarks>
    [Serializable()]
    public class PessimisticOfflineLockException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PessimisticOfflineLockException"/> class.
        /// </summary>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public PessimisticOfflineLockException() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PessimisticOfflineLockException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public PessimisticOfflineLockException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PessimisticOfflineLockException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="inner">The inner.</param>
        /// <remarks>
        /// tdill - 4/22/2008 10:00 AM
        /// </remarks>
        public PessimisticOfflineLockException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="PessimisticOfflineLockException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">The <paramref name="info"/> parameter is null. </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0). </exception>
        /// <remarks>
        /// tdill - 4/22/2008 9:59 AM
        /// </remarks>
        protected PessimisticOfflineLockException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
