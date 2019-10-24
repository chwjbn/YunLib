using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using YunLib.StructData;

namespace YunLib.Model
{
    public class YunMapCacheServer<T>:IYunMapCache,IDisposable where T:struct,IFoxxData
    { 

        private long mMaxDataCount = 10000;
        private string mCacheFileName = string.Empty;


        private long mLastWriteKey = -1;  //写入位置
        private ReaderWriterLockSlim mKeyLock = new ReaderWriterLockSlim();

        private MemoryMappedFile mMapFile = null;
        private MemoryMappedViewStream mMapStream = null;

        public void InitCache(string cacheFileName, long maxDataCount)
        {
            this.mCacheFileName = cacheFileName;
            this.mMaxDataCount = maxDataCount;
            this.initDataCache();
        }


        public bool IsCacheValid()
        {
            bool bRet = false;

            if (mLastWriteKey >= 0)
            {
                bRet = true;
            }

            return bRet;
        }

        /// <summary>
        /// 初始化缓存
        /// </summary>
        private void initDataCache()
        {
            try
            {

                long nLenSize = DataHelper.StructSize<DataLenNode>();
                long nDataSize = DataHelper.StructSize<T>();

                long mapCacheSize = this.mMaxDataCount * nDataSize + nLenSize;

                var dbFilePath = string.Format("{0}/{1}.dat", YunMapCacheHelper.getMapCacheRoot(), this.mCacheFileName);
                var mapName = string.Format("YunMapCache_{0}", this.mCacheFileName);

                this.mMapFile= MemoryMappedFile.CreateFromFile(dbFilePath, FileMode.OpenOrCreate, mapName, mapCacheSize);
                this.mMapStream = this.mMapFile.CreateViewStream();

                //读取最后一个写入位置
                this.mLastWriteKey = this.ReadLen();

                YunLib.LogWriter.Log("YunMapCacheServer InitCache dbFilePath={0}",Path.GetFullPath(dbFilePath));
                YunLib.LogWriter.Log("YunMapCacheServer InitCache mapName={0} Success,WriteLen={1}", mapName, this.mLastWriteKey);

                Console.WriteLine("YunMapCacheServer InitCache dbFilePath={0}", Path.GetFullPath(dbFilePath));
                Console.WriteLine("YunMapCacheServer InitCache mapName={0} Success,WriteLen={1}", mapName, this.mLastWriteKey);

            }
            catch (Exception ex)
            {
                YunLib.LogWriter.Log(ex.ToString());
            }
        }


        /// <summary>
        /// 数据追加进缓存
        /// </summary>
        /// <param name="iData"></param>
        public void PushData(T iData)
        {
            this.mKeyLock.EnterWriteLock();

            //数据存储
            try
            {

                if (this.mLastWriteKey >= 0)
                {

                    byte[] iDataBuf = DataHelper.StructToBytes<T>(iData);

                    int nLenSize = DataHelper.StructSize<DataLenNode>();
                    long offSet = this.mLastWriteKey * iDataBuf.LongLength + nLenSize;

                    this.mMapStream.Seek(offSet, SeekOrigin.Begin);
                    this.mMapStream.Write(iDataBuf, 0, iDataBuf.Length);

                    this.mLastWriteKey = this.mLastWriteKey + 1;

                    this.WriteLen(this.mLastWriteKey);
                }
                else
                {
                    YunLib.LogWriter.Log("YunMapCacheServer Not Inited,PushData Faild!");
                }

            }
            catch (Exception ex)
            {
                YunLib.LogWriter.Log(ex.ToString());
            }

            this.mKeyLock.ExitWriteLock();
        }


        /// <summary>
        /// 写入长度
        /// </summary>
        /// <param name="len"></param>
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

        /// <summary>
        /// 读取长度
        /// </summary>
        /// <returns></returns>
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


        /// <summary>
        /// 关闭缓存
        /// </summary>
        public void Close()
        {
            try
            {
                if (this.mMapStream!=null)
                {
                    this.mMapStream.Dispose();
                }
            }
            catch (Exception ex)
            {
                YunLib.LogWriter.Log(ex.ToString());
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

        /// <summary>
        /// 彻底清理
        /// </summary>
        public void Destory()
        {

            this.Close();

            try
            {
                var dbFilePath = string.Format("{0}/{1}.dat", YunMapCacheHelper.getMapCacheRoot(), this.mCacheFileName);
                YunLib.LogWriter.Log("YunMapCacheServer Destory dbFilePath={0} Begin", dbFilePath);

                if (File.Exists(dbFilePath))
                {
                    dbFilePath = Path.GetFullPath(dbFilePath);

                    try
                    {
                        File.Delete(dbFilePath);
                    }
                    catch (Exception eex)
                    {
                        YunLib.LogWriter.Log(eex.Message);
                    }

                }

                YunLib.LogWriter.Log("YunMapCacheServer Destory dbFilePath={0} End", dbFilePath);
            }
            catch (Exception ex)
            {
                YunLib.LogWriter.Log(ex.ToString());
            }
        }

        public void Dispose()
        {
            this.Close();
        }
    }
}
