using System.Data;
using System.Data.Odbc;

namespace DataObjectsWrapper
{
    /// <summary>
    /// The SQLDataAccessLayer contains the data access layer for Odbc data provider. 
    /// This class implements the abstract methods in the DataAccessLayerBaseClass class.
    /// </summary>
    public class OdbcDataAccessLayer : DataAccessLayerBaseClass
    {
        // Provide class constructors
        public OdbcDataAccessLayer() { }
        public OdbcDataAccessLayer(string connectionString) { this.ConnectionString = connectionString; }

        // DataAccessLayerBaseClass Members
        internal override IDbConnection GetDataProviderConnection()
        {
            return new OdbcConnection();
        }
        internal override IDbCommand GetDataProviderCommand()
        {
            return new OdbcCommand();
        }

        internal override IDbDataAdapter GetDataProviderDataAdapter()
        {
            return new OdbcDataAdapter();
        }
    }
}
