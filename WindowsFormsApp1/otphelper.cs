using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp1
{
    public class otphelper
    {
        public static async Task<ViotpResultGetNumber> getnumber(string APIKEY)
        {
            var url_get_number_otp = "https://api.viotp.com/request/getv2?token=" + APIKEY + "&serviceId=2&network=ITELECOM|VINAPHONE|MOBIFONE";


            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                var data_api = webClient.DownloadString(url_get_number_otp);

                JObject json = JObject.Parse(data_api);
                if (json != null)
                {
                    var a = JsonConvert.DeserializeObject<ViotpResultGetNumber>(json["data"].ToString());

                    if (a != null)
                    {
                        return a;
                    }
                }






            }
            return null;

        }
        public static async Task<ViotpResultGetNumber> getagiannumber(string APIKEY, string old_number)
        {
            //var url_get_number_otp = "https://api.viotp.com/request/getv2?token=" + APIKEY + "&serviceId=29&network=ITELECOM";

            var url_get_number_otp = "https://api.viotp.com/request/getv2?token=" + APIKEY + "&serviceId=2&number=0" + old_number;
            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                var data_api = webClient.DownloadString(url_get_number_otp);

                JObject json = JObject.Parse(data_api);
                if (json != null && json["data"] != null)
                {
                    var a = JsonConvert.DeserializeObject<ViotpResultGetNumber>(json["data"].ToString());

                    if (a != null)
                    {
                        return a;
                    }
                }






            }
            return null;

        }

        public static async Task<CodeResult> Getcode(string request_id, string ApiKey)
        {
            var url_getcode = "https://api.viotp.com/session/getv2?requestId=" + request_id + "&token=" + ApiKey;
            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                var data_api = webClient.DownloadString(url_getcode);

                JObject json = JObject.Parse(data_api);
                if (json != null)
                {
                    var a = JsonConvert.DeserializeObject<CodeResult>(json["data"].ToString());

                    if (a != null)
                    {
                        return a;
                    }
                }






            }
            return null;
        }
        public static async Task<GmailResult> getgmail(string APIKEY)
        {
            var url_get_gmail = "http://sellgmail.com/api/mailselling/order-rent-mail?serviceId=16&apiKey=" + APIKEY;


            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                var data_api = webClient.DownloadString(url_get_gmail);

                JObject json = JObject.Parse(data_api);
                if (json != null)
                {
                    var a = JsonConvert.DeserializeObject<GmailResult>(json.ToString());

                    if (a != null)
                    {
                        return a;
                    }
                }






            }
            return null;

        }

        public static async Task<GmailCodeResult> GetGmailcode(string APIKEY, string email)
        {
            var url_getcode = "http://sellgmail.com/api/mailselling/get-mail-otp?mail=" + email + "&apiKey=" + APIKEY;
            using (WebClient webClient = new WebClient())
            {
                webClient.Encoding = Encoding.UTF8;
                var data_api = webClient.DownloadString(url_getcode);

                JObject json = JObject.Parse(data_api);
                if (json != null)
                {
                    var a = JsonConvert.DeserializeObject<GmailCodeResult>(json["data"].ToString());

                    if (a != null)
                    {
                        return a;
                    }
                }






            }
            return null;
        }
    }
}
