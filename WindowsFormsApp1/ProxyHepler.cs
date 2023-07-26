using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public static class ProxyHepler
    {
        public async static Task<ProxyResult> getProxy(string Api)
        {
            string url = "http://proxy.shoplike.vn/Api/getNewProxy?access_token=" + Api;
            using (WebClient wb = new WebClient())
            {
                wb.Encoding = Encoding.UTF8;
                var data_api = wb.DownloadString(url);

                JObject json = JObject.Parse(data_api);
                if (json != null)
                {
                    try
                    {
                        if (json["data"] == null) return null;
                        var proxy = JsonConvert.DeserializeObject<ProxyResult>(json["data"].ToString());

                        if (proxy != null)
                        {
                            if (checkExitsProxy(proxy.proxy))
                            {
                                return null;
                            }

                            return proxy;
                        }
                    }
                    catch
                    {
                        return null;
                    }

                }
            }
            return null;


        }


        public static bool checkExitsProxy(string proxy)
        {

            string proxyFile = "proxysave.txt";
            if (File.Exists(proxyFile))
            {

                var allproxySave = File.ReadAllLines(proxyFile);

                foreach (var prx in allproxySave)
                {

                    if (prx.Trim() == proxy.Trim())
                    {
                        return true;
                    }
                }
                using (var streanwirter = new StreamWriter(proxyFile, true))
                {
                    streanwirter.WriteLine(proxy);
                    streanwirter.Close();
                }
                return false;
            }
            else
            {

                using (var streanwirter = new StreamWriter(proxyFile, true))
                {
                    streanwirter.WriteLine(proxy);
                    streanwirter.Close();
                }
            }

            return false;

        }
    }
}
