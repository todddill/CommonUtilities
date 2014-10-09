using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DataObjectsWrapperTestHelper
{
    [JsonObject(MemberSerialization.OptIn)]
    public class sample_config
    {
        [JsonProperty("_id", Required = Required.Always)]
        public virtual string _id { get; set; }

        [JsonProperty("_rev", Required = Required.Always)]
        public virtual string _rev { get; set; }

        [JsonProperty("testurl", Required = Required.Always)]
        public virtual string testurl { get; set; }

    }
}
