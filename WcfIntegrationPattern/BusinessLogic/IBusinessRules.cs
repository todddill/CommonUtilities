using System.Collections.Generic;

namespace WcfIntegrationPattern.BusinessLogic
{
    public interface IBusinessRules
    {
        IEnumerable<string> Execute(object obj); 
    }
}
