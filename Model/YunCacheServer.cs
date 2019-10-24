using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using YunLib.StructData;

namespace YunLib.Model
{
    public class YunCacheServer<T> where T :struct,IFoxxData
    {
        private string mLastDate = string.Empty;

        private MemoryMappedFile mMapFile = null;
        private MemoryMappedViewStream mMapStream = null;

        private long mLastWriteKey = 0;

        private T mLastData = new T();

        private object mWriteLock = new object();  //写锁
        private object mOpenLock = new object();    //打开锁

        private long mMapSize = 1L*1024L*1024L*1024L;  //默认内存大小

        private static YunCacheServer<T> mInstance = new YunCacheServer<T>();



        public static YunCacheServer<T> getInstance(string dbName,long maxCount)
        {
            mInstance.OpenDataStoreCache(dbName,maxCount);
            return mInstance;
        }


        private string GetRootDir()
        {
            string sRet = string.Empty;

            sRet = AppDomain.CurrentDomain.BaseDirectory + "yun_data";

            if (!Directory.Exists(sRet))
            {
                Directory.CreateDirectory(sRet);
            }

            return sRet;
        }

        private void InitDataStoreCache(string dbName, string currentDate)
        {
            if (currentDate == this.mLastDate)
            {
                return;
            }

            var dbFilePath = string.Format("{0}\\{1}_{2}.dat",GetRootDir(),currentDate,dbName);

            var mapName = string.Format("DBName_{0}_{1}", dbName, currentDate);

            long mapSize = this.mMapSize;

            try
            {
                if (this.mMapStream!=null)
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
            catch (Exception)
            {
            }



            this.mMapFile = MemoryMappedFile.CreateFromFile(dbFilePath,FileMode.OpenOrCreate,mapName,mapSize);
            this.mMapStream = this.mMapFile.CreateViewStream();

            this.mLastWriteKey = this.ReadLen();


            Console.WriteLine("InitDataStoreCache MapName={0},mLastWriteKey={1}", mapName,this.mLastWriteKey);
        }


        private void OpenDataStoreCache(string dbName, long maxCount)
        {
            var currentDate = DateTime.Now.ToString("yyyyMMdd");
            if (currentDate == this.mLastDate)
            {
                return;
            }


            Monitor.Enter(mOpenLock);

            long nLenSize = DataHelper.StructSize<DataLenNode>();
            long nDataSize= DataHelper.StructSize<T>();

            this.mMapSize = maxCount*nDataSize+nLenSize;
            this.InitDataStoreCache(dbName, currentDate);
            this.mLastDate = currentDate;

            Monitor.Exit(mOpenLock);

        }


        public void PushData(T iData)
        {
            Monitor.Enter(this.mWriteLock);

            //数据存储
            try
            {
                byte[] iDataBuf = DataHelper.StructToBytes<T>(iData);

                int nLenSize = DataHelper.StructSize<DataLenNode>();
                long offSet = this.mLastWriteKey * iDataBuf.LongLength+nLenSize;

                this.mMapStream.Seek(offSet, SeekOrigin.Begin);
                this.mMapStream.Write(iDataBuf, 0, iDataBuf.Length);

               
                this.mLastWriteKey = this.mLastWriteKey + 1;

                this.WriteLen(this.mLastWriteKey);

                this.mLastData = iData;

            }
            catch (Exception ex)
            {
                YunLib.LogWriter.Log(ex.ToString());
            }
            
            Monitor.Exit(this.mWriteLock);
        }


        public void WriteLen(long len)
        {
            try
            {
                DataLenNode iData = new DataLenNode();
                iData.length = len;

                long offSet = 0;

                byte[] iDataBuf = DataHelper.StructToBytes<DataLenNode>(iData);

                this.mMapStream.Seek(offSet, SeekOrigin.Begin);
                this.mMapStream.Write(iDataBuf, 0, iDataBuf.Length);

            }
            catch (Exception ex)
            {
                YunLib.LogWriter.Log(ex.ToString());
            }
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



        public void PrintStatus()
        {
            Console.WriteLine("{0},mLastWriteKey={1},mLastData={2}", this.GetType(), this.mLastWriteKey,this.mLastData.PrintData());
        }

    }
}
