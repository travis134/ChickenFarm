using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading;

namespace ChickenFarm
{
    class CellBuffer<T>
    {
        private T[] mData = null;
        private int mIndex = -1;
        private Semaphore mSemaphoreGet = null;
        private Semaphore mSemaphoreSet = null;
        
        public int Length
        {
            get
            {
                return mData.Length;
            }
        }

        public CellBuffer(int capacity)
        {
            mData = new T[capacity];
            mIndex = 0;
            mSemaphoreGet = new Semaphore(0, 4);
            mSemaphoreSet = new Semaphore(4, 4);
        }

        public T GetOneCell()
        {
            lock(mData)
            {   
                //if mindex <= 0 wait
                mSemaphoreGet.WaitOne();
                mIndex--;
                T result = mData[mIndex];
                mSemaphoreSet.Release();
                return result;
            }
        }

        public void SetOneCell(T data)
        {
            lock (mData)
            {
                //if mindex >= 4 wait
                mSemaphoreSet.WaitOne();
                mData[mIndex] = data;
                mIndex++;
                mSemaphoreGet.Release();
            }
        }
    }
}
