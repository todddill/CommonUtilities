using System.ServiceModel;
using WcfIntegrationPattern.Contracts;

namespace WcfIntegrationPattern
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IProxyService
    {
        [OperationContract]
        [FaultContract(typeof(RulesFault))]
        Response ExecuteProxyOperationA(Request value);

        [OperationContract]
        [FaultContract(typeof(RulesFault))]
        Response ExecuteProxyOperationB(Request value);
    }
}
