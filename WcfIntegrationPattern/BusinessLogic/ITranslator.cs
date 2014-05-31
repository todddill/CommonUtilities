
namespace WcfIntegrationPattern.BusinessLogic
{
    public interface ITranslator
    {
        TOut Translate<TIn, TOut>(TIn obj);
    }
}
