using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ChickenFarm
{
    /**
    * Represents a customer's order to the farm.
    */
    [DataContract]
    public class Order
    {
        [DataMember]
        public Guid SenderId { get; set; }

        [DataMember]
        public String CreditCardNumber { get; set; }

        [DataMember]
        public UInt16 AmountOfChickens { get; set; }


        /*
         * Encoder and decoder, not used in project, but work
         */
        public static String Encode(Order order)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(order.SenderId.ToString());
            sb.Append("/");
            sb.Append(order.CreditCardNumber);
            sb.Append("/");
            sb.Append(order.AmountOfChickens);
            return sb.ToString();
        }

        public static Order Decode(String orderString)
        {
            String[] properties = orderString.Split('/');
            return new Order()
            {
                SenderId = Guid.Parse(properties[0]),
                CreditCardNumber = properties[1],
                AmountOfChickens = UInt16.Parse(properties[2])
            };
        }
    }
}
