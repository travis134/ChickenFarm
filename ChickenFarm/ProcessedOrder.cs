using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ChickenFarm
{
    [DataContract]
    public class ProcessedOrder
    {
        [DataMember]
        public Boolean Success { get; set; }
        [DataMember]
        public Order Order { get; set; }
        [DataMember]
        public Decimal Total { get; set; }

        public ProcessedOrder(Boolean success, Order order)
        {
            Success = success;
            Order = order;
            Total = 0;
        }

        public ProcessedOrder(Boolean success, Order order, Decimal total)
        {
            Success = success;
            Order = order;
            Total = total;
        }
    }
}
