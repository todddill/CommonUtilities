using System;
using System.Runtime.Serialization;

namespace WcfIntegrationPattern.Contracts
{
    [DataContract]
    public class Response
    {
        [DataMember]
        public string Field { get; set; }
    }
}