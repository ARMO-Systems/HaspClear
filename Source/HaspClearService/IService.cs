using System.ServiceModel;

namespace ArmoSystems.ArmoGet.HaspClearService
{
    [ServiceContract]
    public interface IService
    {
        [OperationContract]
        void RestartSLM();
    }
}