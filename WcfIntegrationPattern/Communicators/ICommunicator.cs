namespace WcfIntegrationPattern.Communicators
{
    interface ICommunicator
    {
        object InvokeOperation(object[] parameters);
    }
}
