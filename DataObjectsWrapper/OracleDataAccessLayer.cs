using System.Data;
using System.Data.OracleClient;

namespace DataObjectsWrapper
{
    /// <summary>
    /// The SQLDataAccessLayer contains the data access layer for Oracle data provider. 
    /// This class implements the abstract methods in the DataAccessLayerBaseClass class.
    /// </summary>
    public class OracleDataAccessLayer : DataAccessLayerBaseClass
    {
        // Provide class constructors
        public OracleDataAccessLayer() { }
        public OracleDataAccessLayer(string connectionString) { this.ConnectionString = connectionString; }

        // DataAccessLayerBaseClass Members
        internal override IDbConnection GetDataProviderConnection()
        {
            return new OracleConnection();
        }
        internal override IDbCommand GetDataProviderCommand()
        {
            return new OracleCommand();
        }

        internal override IDbDataAdapter GetDataProviderDataAdapter()
        {
            return new OracleDataAdapter();
        }
    }
}
