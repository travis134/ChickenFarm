using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ChickenFarm
{
    [ServiceContract(SessionMode=SessionMode.Required, CallbackContract=typeof(IChickenFarmCallback))]
    public interface IChickenFarmService
    {
        [OperationContract]
        void Subscribe(Guid id);

        [OperationContract]
        void Unsubscribe(Guid id);

        [OperationContract]
        void PlaceOrder(Order order);
    }
}
