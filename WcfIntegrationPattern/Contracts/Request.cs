using System.Runtime.Serialization;

namespace WcfIntegrationPattern.Contracts
{
    [DataContract]
    public class Request
    {
        [DataMember]
        public string Field { get; set; }
    }
}