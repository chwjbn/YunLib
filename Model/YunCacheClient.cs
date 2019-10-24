using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.IO.MemoryMappedFiles;
using YunLib.StructData;
using System.IO;

namespace YunLib.Model
{
    public class YunCacheClient<T> where T :struct,IFoxxData
    {
        private string mLastDate = string.Empty;
        private long mLastReadKey = 0;

        private MemoryMappedFile mMapFile = null;
        private MemoryMappedViewStream mMapStream = null;

        private object mReadLock = new object();  //读锁
        private object mOpenLock = new object();    //打开锁

        private static YunCacheClient<T> mInstance = new YunCacheClient<T>();

        public static YunCacheClient<T> getInstance(string dbName)
        {
            mInstance.OpenDataStoreCache(dbName);
            return mInstance;
        }


        private void CloseMap()
        {
            try
            {
                if (this.mMapStream != null)
                {
                    this.mMapStream.Dispose();
                }
            }
            catch (Exception)
            {
            }

            try
            {
                if (this.mMapFile != null)
                {
                    this.mMapFile.Dispose();
                }
            }
            catch (Exception ex)
            {
                YunLib.LogWriter.Log(ex.ToString());
            }
        }


        private bool OpenMapName(string mapName)
        {
            bool bRet = false;

            try
            {
                this.mMapFile = MemoryMappedFile.OpenExisting(mapName);
                bRet = true;
            }
            catch (Exception ex)
            {
                //YunLib.LogWriter.Log(ex.ToString());
            }

            return bRet;
        }


        private void InitDataStoreCache(string dbName)
        {
            var currentDate = DateTime.Now.ToString("yyyyMMdd");
            if (currentDate == this.mLastDate)
            {
                return;
            }

            this.CloseMap();

            var mapName = string.Empty;

            while (true)
            {
                currentDate = DateTime.Now.ToString("yyyyMMdd");
                mapName = string.Format("DBName_{0}_{1}", dbName, currentDate);

                if (this.OpenMapName(mapName))
                {
                    this.mLastDate = currentDate;
                    break;
                }

                Console.WriteLine("MapName={0} Not Exists,Waiting...", mapName);
                Thread.Sleep(5000);
            }

            this.mMapStream = this.mMapFile.CreateViewStream();
            this.mLastReadKey = this.ReadLen();

            if (this.mLastReadKey > 0)
            {
                this.mLastReadKey = this.mLastReadKey - 1;
            }


            Console.WriteLine("InitDataStoreCache MapName={0},mLastReadKey={1},Len={2}", mapName, this.mLastReadKey, this.ReadLen());
        }

        private void OpenDataStoreCache(string dbName)
        {
            var currentDate = DateTime.Now.ToString("yyyyMMdd");
            if (currentDate == this.mLastDate)
            {
                return;
            }

            Monitor.Enter(mOpenLock);
            this.InitDataStoreCache(dbName);
            Monitor.Exit(mOpenLock);

        }


        public void ZeroReadIndex()
        {
            Monitor.Enter(this.mReadLock);

            this.mLastReadKey = 0;

            Monitor.Exit(this.mReadLock);
        }


        public void PopData(ref T iData)
        {
            Monitor.Enter(this.mReadLock);

            try
            {
                int nLenSize = DataHelper.StructSize<DataLenNode>();
                int nSize = DataHelper.StructSize<T>();
                long offSet = this.mLastReadKey * nSize + nLenSize;

                byte[] iDataBuf = new byte[nSize];

                this.mMapStream.Seek(offSet, SeekOrigin.Begin);
                this.mMapStream.Read(iDataBuf, 0, iDataBuf.Length);

                iData = DataHelper.BytesToStruct<T>(iDataBuf);

                if (iData.HasFoxxCode())
                {
                    this.mLastReadKey = this.mLastReadKey + 1;
                }

            }
            catch (Exception ex)
            {
                LogWriter.Log(ex.ToString());
            }


            Monitor.Exit(this.mReadLock);

        }

        public long ReadLen()
        {
            long lRet = 0;

            try
            {
                DataLenNode iData = new DataLenNode();
                iData.length = 0;

                long offSet = 0;

                byte[] iDataBuf = DataHelper.StructToBytes<DataLenNode>(iData);

                this.mMapStream.Seek(offSet, SeekOrigin.Begin);
                this.mMapStream.Read(iDataBuf, 0, iDataBuf.Length);

                iData = DataHelper.BytesToStruct<DataLenNode>(iDataBuf);

                lRet = iData.length;
            }
            catch (Exception ex)
            {
                YunLib.LogWriter.Log(ex.ToString());
            }

            return lRet;
        }


    }
}
