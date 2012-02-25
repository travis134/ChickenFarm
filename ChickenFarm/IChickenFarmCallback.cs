using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ChickenFarm
{
    public interface IChickenFarmCallback
    {
        [OperationContract(IsOneWay = true)]
        void PriceChanged(Decimal newPrice);

        [OperationContract(IsOneWay = true)]
        void OrderConfirmation(ProcessedOrder processedOrder);
    }
}
