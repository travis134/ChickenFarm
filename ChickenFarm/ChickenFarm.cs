using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Threading;

namespace ChickenFarm
{
    [CallbackBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]
    class ChickenFarm : IChickenFarmService
    {
        private PricingModel mPricingModel = null;
        private OrderProcessing mOrderProcessing = null;
        private Dictionary<Guid, IChickenFarmCallback> mSubscribers;
        private CellBuffer<Order> mOrderBuffer = null;
        private int mCurrentPriceCuts = -1;
        private int mMaxPriceCuts = -1;
        private Decimal mLastPrice = -1;
        Thread mOrderConsumerThread = null;

        public ChickenFarm()
        {
            List<UInt32> dailySales = new List<UInt32>()
            {
                20,
                20,
                20
            };

            mPricingModel = new PricingModel(dailySales, new Decimal(), new Decimal());
            mOrderProcessing = new OrderProcessing(mPricingModel);

            mOrderBuffer = new CellBuffer<Order>(4);

            mCurrentPriceCuts = 0;
            mMaxPriceCuts = 10;

            mLastPrice = Decimal.MaxValue;

            mSubscribers = new Dictionary<Guid, IChickenFarmCallback>();

            mOrderConsumerThread = new Thread(new ThreadStart(ProcessOrder));
            mOrderConsumerThread.Name = "OrderConsumer";
            mOrderConsumerThread.Start();
        }

        public void Subscribe(Guid id)
        {
            Thread.Sleep(10000);
            mSubscribers.Add(id, OperationContext.Current.GetCallbackChannel<IChickenFarmCallback>());
        }

        public void Unsubscribe(Guid id)
        {
            mSubscribers.Remove(id);
        }

        public void PlaceOrder(Order order)
        {
            mOrderBuffer.SetOneCell(order);
        }

        public void ProcessOrder()
        {
            while (mCurrentPriceCuts < mMaxPriceCuts)
            {
                if (mPricingModel.TodaysPrice() <= mLastPrice)
                {
                    mLastPrice = mPricingModel.TodaysPrice();
                    foreach(IChickenFarmCallback callback in mSubscribers.Values)
                    {
                        callback.PriceChanged(mPricingModel.TodaysPrice());
                    }
                    mCurrentPriceCuts++;
                }
                Order order = mOrderBuffer.GetOneCell();
                ParameterizedThreadStart processOrder = new ParameterizedThreadStart(mOrderProcessing.ProcessOrder);
                Thread orderProcessingThread = new Thread(processOrder);
                orderProcessingThread.Start(new object[] { order, mSubscribers[order.SenderId] });
                Thread.Sleep(1000);
            }
        }
    }
}
