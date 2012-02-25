using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ChickenFarm
{
    class OrderProcessing
    {
        private PricingModel mPricingModel = null;

        public OrderProcessing(PricingModel pricingModel)
        {
            mPricingModel = pricingModel;
        }

        /*
         * Check credit card number
         * Calculate total amount of charge (unit price * quantity + tax + shipping)
         * Send confirmation to retailer
         */
        public void ProcessOrder(object parameters)
        {
            object[] boxedParams = (object[])parameters;

            Order order = (Order)boxedParams[0];
            IChickenFarmCallback callBack = (IChickenFarmCallback)boxedParams[1];

            ProcessedOrder pOrder;
            if(validateCreditCard(order.CreditCardNumber))
            {
                pOrder = new ProcessedOrder(true, order, mPricingModel.TodaysPrice());
                mPricingModel.AddSale(order.AmountOfChickens);
            }else{
                pOrder = new ProcessedOrder(false, order);
            }

            callBack.OrderConfirmation(pOrder);
        }

        /*
         * Valid credit cards are numeric and of 16 length
         */
        private Boolean validateCreditCard(String creditCardNumber)
        {
            Boolean result = true;

            if (creditCardNumber.Length != 16)
            {
                result = false;
            }
            else
            {
                for (int i = 0; i < 16; i++)
                {
                    if (!char.IsDigit(creditCardNumber, i))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }
    }
}
