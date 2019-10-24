using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace YunLib.Model
{
    public static class YunMapCacheManager<T>  where T:IYunMapCache, new()
    {

        //实例列表
        private static Dictionary<string, T> mInstanceList = new Dictionary<string, T>();
        private static ReaderWriterLockSlim mInstanceListLock = new ReaderWriterLockSlim();

        /// <summary>
        /// 清除过期的缓存数据
        /// </summary>
        /// <param name="mLastDateStr"></param>
        private static void clearOutDateCache(string mLastDateStr)
        {
            try
            {
                var needClearList = new List<string>();

                foreach (var kp in mInstanceList)
                {
                    if (kp.Key.Contains(mLastDateStr))
                    {
                        continue;
                    }

                    needClearList.Add(kp.Key);
                }


                YunLib.LogWriter.Log("YunMapCacheManager.clearOutDataCache needClearList count={0}",needClearList.Count);

                foreach (string k in needClearList)
                {
                    var tempCache = mInstanceList[k];
                    mInstanceList.Remove(k);
                    tempCache.Destory();
                }

                clearOutDateFile(mLastDateStr);

            }
            catch (Exception ex)
            {
                YunLib.LogWriter.Log(ex.ToString());
            }
        }

        private static void clearOutDateFile(string mLastDateStr)
        {

            YunLib.LogWriter.Log("YunMapCacheManager.clearOutDateFile Begin");

            string fileDir = YunMapCacheHelper.getMapCacheRoot();

            

            try
            {

                var fileList = Directory.GetFiles(fileDir);

                foreach (string fileName in fileList)
                {
                    if (fileName.Contains(mLastDateStr))
                    {
                        continue;
                    }

                    try
                    {
                        File.Delete(fileName);
                    }
                    catch (Exception eex)
                    {
                        YunLib.LogWriter.Log(eex.Message);
                    }
                }


            }
            catch (Exception ex)
            {
                YunLib.LogWriter.Log(ex.ToString());
            }

            YunLib.LogWriter.Log("YunMapCacheManager.clearOutDateFile End");
        }


        /// <summary>
        /// 获取存在的实例
        /// </summary>
        /// <param name="mLastDateStr"></param>
        /// <param name="dbName"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        private static T getExistsInstance(string mLastDateStr, string dbName, long maxCount)
        {
            T mInstance = new T();

            mInstanceListLock.EnterReadLock();

            try
            {
                string dataKey = YunMapCacheHelper.getInstanceName(mLastDateStr, dbName, maxCount);
                if (mInstanceList.ContainsKey(dataKey))
                {
                    mInstance = mInstanceList[dataKey];
                }
            }
            catch (Exception ex)
            {
                YunLib.LogWriter.Log(ex.ToString());
            }

            mInstanceListLock.ExitReadLock();

            return mInstance;
        }


        /// <summary>
        /// 获取一个新的实例
        /// </summary>
        /// <returns></returns>
        private static T getNewInstance(string mLastDateStr, string dbName, long maxCount)
        {
            T mInstance = new T();

            mInstanceListLock.EnterWriteLock();

            try
            {
                string dataKey = YunMapCacheHelper.getInstanceName(mLastDateStr, dbName, maxCount);

                //可能多个写锁触发,先判断
                if (mInstanceList.ContainsKey(dataKey))
                {
                    mInstance = mInstanceList[dataKey];
                }
                else
                {
                    mInstance.InitCache(dataKey, maxCount);
                    if (mInstance.IsCacheValid())
                    {
                        mInstanceList.Add(dataKey, mInstance);
                    }
                }
                  
                //清理过期数据缓存
                clearOutDateCache(mLastDateStr);

            }
            catch (Exception ex)
            {
                YunLib.LogWriter.Log(ex.ToString());
            }

            mInstanceListLock.ExitWriteLock();

            return mInstance;
        }


        /// <summary>
        /// 获取实例对象
        /// </summary>
        /// <param name="dbName">数据库名称</param>
        /// <param name="maxCount">最大记录数</param>
        /// <returns></returns>
        public static T getInstance(string dbName, long maxCount)
        {
            string mLastDateStr = DateTime.Now.ToString("yyyyMMdd");

            T mInstance = new T();

            try
            {

                //获取已经存在的
                mInstance = getExistsInstance(mLastDateStr, dbName, maxCount);
                if (mInstance.IsCacheValid())
                {
                    return mInstance;
                }

                while (true)
                {
                    //获取新创建实例
                    mInstance = getNewInstance(mLastDateStr, dbName, maxCount);
                    if (mInstance.IsCacheValid())
                    {
                        break;
                    }   

                    YunLib.LogWriter.Log("YunMapCacheManager.getInstance Invalid,Wait for 20s...");
                    Thread.Sleep(20 * 1000);
                }

            }
            catch (Exception ex)
            {

                YunLib.LogWriter.Log(ex.ToString());
            }

            return mInstance;
        }

    }
}
