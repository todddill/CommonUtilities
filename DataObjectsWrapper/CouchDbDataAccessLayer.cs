using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MyCouch;
using Newtonsoft.Json.Serialization;

namespace DataObjectsWrapper
{
    public class CouchDbDataAccessLayer : DataAccessLayerBaseClass
    {
        public CouchDbDataAccessLayer() { }
        public CouchDbDataAccessLayer(string connectionString) { this.ConnectionString = connectionString; }

        internal override IDbConnection GetDataProviderConnection()
        {
            throw new NotImplementedException();
        }

        internal override IDbCommand GetDataProviderCommand()
        {
            throw new NotImplementedException();
        }

        internal override IDbDataAdapter GetDataProviderDataAdapter()
        {
            throw new NotImplementedException();
        }

        public string GetJsonValue(string id, string field)
        {
            MyCouchStore store = GetMyCouchStore();

            MyCouchClient client = new MyCouchClient(this.ConnectionString);
            var getResponse = client.Documents.GetAsync(id);


            var keyvalues = client.Serializer.Deserialize<IDictionary<string, dynamic>>(getResponse.Result.Content);
            return keyvalues[field];
        }

        private MyCouchStore GetMyCouchStore()
        {
            if (string.IsNullOrEmpty(this.ConnectionString)) throw new ArgumentOutOfRangeException("connectionString");

            return new MyCouchStore(this.ConnectionString);
        }
    }
}
