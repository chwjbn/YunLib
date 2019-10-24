using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace YunLib.Model
{
    public class MonitorNodeModel
    {
        public string hostname;
        public string owner;
        public string side;
        public string method;
        public string version;
        public string pid;
        public string interface_name;
        public string generic;
        public string revision;
        public string serialization;
        public string app_name;
        public string optimizer;
        public string anyhost;
        public string tamp;
        public string description;

        public void Create()
        {

            var localHostName = getLocalHost();

            if (string.IsNullOrEmpty(localHostName))
            {
                localHostName = Dns.GetHostName();
            }

            this.hostname = localHostName;

            this.owner = Environment.UserName;
            this.side = "provider";
            this.method = "doVoid";
            this.version = "9.9.9";
            this.pid = string.Format("{0}", Process.GetCurrentProcess().Id);
            this.interface_name = string.Format("com.yuncaijing.monitor.donet.{0}",getCurrentTaskAppName());
            this.generic = "false";

            FileVersionInfo mFileVersionInfo = FileVersionInfo.GetVersionInfo(AppDomain.CurrentDomain.SetupInformation.ApplicationBase+AppDomain.CurrentDomain.SetupInformation.ApplicationName);
            this.revision = mFileVersionInfo.FileVersion;

            this.description = mFileVersionInfo.Comments;

            this.serialization = "zmq";
            this.app_name = AppDomain.CurrentDomain.SetupInformation.ApplicationName;

            this.optimizer = "com.yuncaijing.configuration.dubbo.serialization.SerializationOptimizerImpl";

            this.anyhost = "false";

            this.tamp = DateTime.Now.ToString("yyyyMMddHHmmss");


        }

        public void SetHostName(string hostname)
        {
            this.hostname = hostname;
        }

        public string ToUrl()
        {
            string sRet = string.Format("dubbo://{0}/{1}?anyhost={2}&application={3}&dubbo={4}&generic={5}&interface={6}&methods={7}&optimizer={8}&owner={9}&pid={10}&revision={11}&serialization={12}&side={13}&tamp={14}&description={15}",hostname,interface_name,anyhost,app_name,version,generic,interface_name,method,optimizer,owner,pid,revision,serialization,side,tamp,description);

            sRet=System.Web.HttpUtility.UrlEncode(sRet);

            return sRet;
        }


        private string getCurrentTaskAppName()
        {
            string sRet = string.Empty;

            sRet = AppDomain.CurrentDomain.SetupInformation.ApplicationName;

            sRet = Path.GetFileNameWithoutExtension(sRet);

            return sRet;
        }

        private string getLocalHost()
        {
            string sRet = string.Empty;

            try
            {
                TcpClient tcpClient = new TcpClient();
                tcpClient.Connect("zk.in.yuncaijing.com", 2181);
                if (tcpClient.Connected)
                {
                    sRet = tcpClient.Client.LocalEndPoint.ToString();
                }
            }
            catch (Exception)
            {
            }

            return sRet;
        }
    }
}
