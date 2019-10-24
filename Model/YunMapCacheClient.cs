using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using YunLib.StructData;

namespace YunLib.Model
{
    public class YunMapCacheClient<T> : IYunMapCache, IDisposable where T : struct, IFoxxData
    {

        private long mLastReadKey = -1;
        private ReaderWriterLockSlim mKeyLock = new ReaderWriterLockSlim();

        private MemoryMappedFile mMapFile = null;
        private MemoryMappedViewStream mMapStream = null;

        private long mMaxDataCount = 10000;
        private string mCacheFileName = string.Empty;

        public void InitCache(string cacheFileName, long maxDataCount)
        {
            this.mCacheFileName = cacheFileName;
            this.mMaxDataCount = maxDataCount;
            this.initDataCache();
        }


        public bool IsCacheValid()
        {
            bool bRet = false;

            if (mLastReadKey >= 0)
            {
                bRet = true;
            }

           

            return bRet;
        }

        private void initDataCache()
        {
            try
            {

                var mapName = string.Format("YunMapCache_{0}", this.mCacheFileName);

                if (this.openMap(mapName))
                {
                    this.mMapStream = this.mMapFile.CreateViewStream();
                    this.mLastReadKey = 0;

                    var nReadLen = this.ReadLen();

                    YunLib.LogWriter.Log("YunMapCacheClient InitCache mapName={0} Success,ReadLen={1}", mapName,nReadLen);
                    Console.WriteLine("YunMapCacheClient InitCache mapName={0} Success,ReadLen={1}", mapName,nReadLen);
                }
                else
                {
                    YunLib.LogWriter.Log("YunMapCacheClient InitCache mapName={0} Faild!",mapName);
                    Console.WriteLine("YunMapCacheClient InitCache mapName={0} Faild!", mapName);
                }
                

            }
            catch (Exception ex)
            {
                YunLib.LogWriter.Log(ex.ToString());
            }
        }


        private bool openMap(string mapName)
        {
            bool bRet = false;
            try
            {
                this.mMapFile = MemoryMappedFile.OpenExisting(mapName);
                bRet = true;
            }
            catch (Exception)
            {
            }

            return bRet;
        }

        public void PopData(ref T iData)
        {
            this.mKeyLock.EnterWriteLock();

            try
            {
                if (this.mLastReadKey >= 0)
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
                else
                {
                    YunLib.LogWriter.Log("YunMapCacheClient Not Inited,PopData Faild!");
                }

            }
            catch (Exception ex)
            {
                LogWriter.Log(ex.ToString());
            }


            this.mKeyLock.ExitWriteLock();

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


        public void ZeroReadIndex()
        {
            this.mKeyLock.EnterWriteLock();

            this.mLastReadKey = 0;

            this.mKeyLock.ExitWriteLock();
        }

        /// <summary>
        /// 关闭缓存
        /// </summary>
        public void Close()
        {
            try
            {
                if (this.mMapStream != null)
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
                if (File.Exists(dbFilePath))
                {

                    dbFilePath = Path.GetFullPath(dbFilePath);

                    YunLib.LogWriter.Log("YunMapCacheClient Destory dbFilePath={0}", dbFilePath);

                    File.Delete(dbFilePath);
                }

            }
            catch (Exception)
            {
                //YunLib.LogWriter.Log(ex.ToString());
            }
        }

        public void Dispose()
        {
            this.Close();
        }


    }
}
