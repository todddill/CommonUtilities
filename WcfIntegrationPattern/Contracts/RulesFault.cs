using System.Runtime.Serialization;

namespace WcfIntegrationPattern.Contracts
{
    [DataContract]
    public class RulesFault
    {
        [DataMember]
        public string[] Operation { get; set; }

        [DataMember]
        public string[] ProblemType { get; set; }
    }
}