using System;

namespace WcfIntegrationPattern.Communicators
{
    public class StrategyBMockCommunicator : ICommunicator 
    {
        #region ICommunicator Members

        public object InvokeOperation(object[] parameters)
        {
            return new StrategyBResponse {
                Field = ((StrategyBRequest)parameters[0]).Field + 2
            };
        }

        #endregion
    }

    public class StrategyBRequest
    {
        public int Field { get; set; }
    }

    public class StrategyBResponse
    {
        public int Field { get; set; }
    }
}