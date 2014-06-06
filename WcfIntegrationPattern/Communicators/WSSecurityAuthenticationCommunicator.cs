using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WcfWsSecurityAuthentication;

namespace WcfIntegrationPattern.Communicators
{
    public class WSSecurityAuthenticationCommunicator : ICommunicator 
    {
        #region ICommunicator Members

        public object InvokeOperation(object[] parameters)
        {
            WsSecurityProxy proxy = new WsSecurityProxy{ 
                MaxMessageSize = Int32.MaxValue
            };
            SomeProxyInterface myProxy = proxy.CreateSoap11Proxy<SomeProxyInterface>("http://www.exapmle.com", "user", "pass");
            return myProxy.SomeProxyOperation(parameters).ToString();
        }

        #endregion
    }

    interface SomeProxyInterface
    {
        string SomeProxyOperation(object[] parameters);
    }
}