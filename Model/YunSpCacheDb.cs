using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace YunLib.Model
{
    public class YunSpCacheDb<T> where T : TDFDataModel, new()
    {     
        private static YunSpCacheDb<T> mInstance = new YunSpCacheDb<T>();

        private string mLastDate = string.Empty;
        private ReaderWriterLockSlim mReadWriteLockSlim = new ReaderWriterLockSlim();

        private List<T> mCacheDataList = new List<T>();
        private int mLastReadIndex = 0;

        public static YunSpCacheDb<T> getInstance(string dbName)
        {
            mInstance.OpenDataStoreCache(dbName);
            return mInstance;
        }

        private void OpenDataStoreCache(string dbName)
        {
            var currentDate = DateTime.Now.ToString("yyyyMMdd");
            if (currentDate == this.mLastDate)
            {
                return;
            }


            try
            {
                this.mReadWriteLockSlim.EnterWriteLock();

                this.InitDataStoreCache(dbName, currentDate);

                this.mLastDate = currentDate;

            }
            finally
            {
                this.mReadWriteLockSlim.ExitWriteLock();
            }

        }

        private void InitDataStoreCache(string dbName, string currentDate)
        {

            if (currentDate == this.mLastDate)
            {
                return;
            }

            this.mCacheDataList.Clear();
            this.mLastReadIndex = this.mCacheDataList.Count;

            Console.WriteLine("{0} InitDataStoreCache,currentDate={1}",this.GetType(),currentDate);

        }


        public void PushData(T iData)
        {

            try
            {
                this.mReadWriteLockSlim.EnterWriteLock();
                this.mCacheDataList.Add(iData);

            }
            catch (Exception ex)
            {
                
            }
            finally
            {
                this.mReadWriteLockSlim.ExitWriteLock();
            }

        }


        public T PopData()
        {
            T iData = new T();

            try
            {
                this.mReadWriteLockSlim.EnterReadLock();

                if (this.mCacheDataList.Count>this.mLastReadIndex)
                {
                    iData = this.mCacheDataList[this.mLastReadIndex];
                    this.mLastReadIndex++;
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.mReadWriteLockSlim.ExitReadLock();
            }

            return iData;
        }

        public void SetReadKey(int idex)
        {
            try
            {
                this.mReadWriteLockSlim.EnterReadLock();

                if (idex >= this.mCacheDataList.Count)
                {
                    idex = this.mCacheDataList.Count - 1;
                }

                this.mLastReadIndex = idex;

            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.mReadWriteLockSlim.ExitReadLock();
            }
        }



        public long GetCount()
        {
            int iCount = 0;

            try
            {
                this.mReadWriteLockSlim.EnterReadLock();

                iCount = this.mCacheDataList.Count;

            }
            catch (Exception ex)
            {

            }
            finally
            {
                this.mReadWriteLockSlim.ExitReadLock();
            }

            return iCount;
        }

        public void PrintStatus()
        {
            Console.WriteLine("{0},mLastReadIndex={1}", this.GetType(), this.mLastReadIndex);
            Console.WriteLine("{0},mCacheDataList Count={1}", this.GetType(), this.mCacheDataList.Count);
        }


    }
}
