using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChickenFarm
{
    /*
    * Determine the price for an order based on time and the number of
    * chickens the farm can produce.
    */
    class PricingModel
    {
        /*
         * Contains the number of chickens sold per order
         * @see calculateProjectedSales()
         */
        private List<UInt32> mSales;

        /*
         * Used to calculate the new markup or markoff of the product.
         * mBottomLinePrice is the lowest possible price of a chicken.
         * mTopLinePrice is the highest possible price of a chicken.
         */
        private Decimal mBottomLinePrice, mTopLinePrice;

        public PricingModel(List<UInt32> sales, Decimal bottomLinePrice, Decimal topLinePrice)
        {
            mSales = sales;
            mBottomLinePrice = bottomLinePrice;
            mTopLinePrice = topLinePrice;
        }

        public void AddSale(UInt16 sale)
        {
            mSales.Add(sale);
        }

        public Decimal TodaysPrice()
        {
            double averageSales = calculateAverageSales();
            double projectedSales = calculateProjectedSales();
            double changeInSales = 1 + ((projectedSales - averageSales) / averageSales);
            Decimal averagePrice = (mBottomLinePrice + mTopLinePrice) / 2;
            return Math.Max(Math.Min(averagePrice * (decimal)changeInSales, mTopLinePrice), mBottomLinePrice);
        }

        private double calculateAverageSales()
        {
            double sum = 0.0d;
            for (int i = 0; i < mSales.Count; i++)
            {
                sum += mSales[i];
            }
            return sum / mSales.Count;
        }

        /*
         * Linear regression model
         */
        private double calculateProjectedSales()
        {
            double xAvg = 0;
            double yAvg = 0;

            for (int x = 0; x < mSales.Count; x++)
            {
                xAvg += x;
                yAvg += mSales[x];
            }

            xAvg = xAvg / mSales.Count;
            yAvg = yAvg / mSales.Count;

            double v1 = 0;
            double v2 = 0;

            for (int x = 0; x < mSales.Count; x++)
            {
                v1 += (x - xAvg) * (mSales[x] - yAvg);
                v2 += Math.Pow(x - xAvg, 2);
            }

            double a = v1 / v2;
            double b = yAvg - a * xAvg;

            return b + mSales.Count * a;
        }
    }
}
