using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YunLib.Model;
using ZooKeeperNet;

namespace YunLib
{
    public abstract class YunTaskService
    {

        private ZooKeeper mZooKeeper = null;


        public YunTaskService()
        {
            initTaskMonitor();
        }


        //获取当前运行的应用的名称
        private string getCurrentTaskAppName()
        {
            string sRet = string.Empty;

            sRet = AppDomain.CurrentDomain.SetupInformation.ApplicationName;

            sRet = Path.GetFileNameWithoutExtension(sRet);

            return sRet;
        }

      
        private void initTaskMonitor()
        {
            try
            {

                var appName = this.getCurrentTaskAppName();

                mZooKeeper = new ZooKeeper("zk.in.yuncaijing.com:2181",new TimeSpan(0,0,10),new MonitorWatcher());

                var appFullPath = "/dubbo";
                var iStat=mZooKeeper.Exists(appFullPath,true);
                if (iStat==null)
                {
                    mZooKeeper.Create(appFullPath, new byte[] { }, Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                }

                appFullPath = string.Format("/dubbo/com.yuncaijing.monitor.donet.{0}",appName);
                iStat = mZooKeeper.Exists(appFullPath, true);
                if (iStat == null)
                {
                    mZooKeeper.Create(appFullPath, new byte[] { }, Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                }

                appFullPath = string.Format("/dubbo/com.yuncaijing.monitor.donet.{0}/providers", appName);
                iStat = mZooKeeper.Exists(appFullPath, true);
                if (iStat == null)
                {
                    mZooKeeper.Create(appFullPath, new byte[] { }, Ids.OPEN_ACL_UNSAFE, CreateMode.Persistent);
                }

                var nodeInfo = new MonitorNodeModel();
                nodeInfo.Create();

                var localUrl = nodeInfo.ToUrl();

                appFullPath = string.Format("/dubbo/com.yuncaijing.monitor.donet.{0}/providers/{1}", appName,localUrl);
                iStat = mZooKeeper.Exists(appFullPath, true);
                if (iStat == null)
                {
                    mZooKeeper.Create(appFullPath, new byte[] { }, Ids.OPEN_ACL_UNSAFE, CreateMode.Ephemeral);
                }

            }
            catch (Exception ex)
            {
                YunLib.LogWriter.Log(ex.ToString());
            }
        }

        public abstract void startService();
        public abstract void showStatus();
        
    }
}
