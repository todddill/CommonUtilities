﻿using System.Data;
using System.Data.OleDb;

namespace DataObjectsWrapper
{
    /// <summary>
    /// The OleDbDataAccessLayer contains the data access layer for OleDb data provider. 
    /// This class implements the abstract methods in the DataAccessLayerBaseClass class.
    /// </summary>
    public class OleDbDataAccessLayer : DataAccessLayerBaseClass
    {
        // Provide class constructors
        public OleDbDataAccessLayer() { }
        public OleDbDataAccessLayer(string connectionString) { this.ConnectionString = connectionString; }

        // DataAccessLayerBaseClass Members
        internal override IDbConnection GetDataProviderConnection()
        {
            return new OleDbConnection();
        }
        internal override IDbCommand GetDataProviderCommand()
        {
            return new OleDbCommand();
        }

        internal override IDbDataAdapter GetDataProviderDataAdapter()
        {
            return new OleDbDataAdapter();
        }
    }
}
