using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace WindowsFormsApp1
{
    public class FPTOCR
    {

        public static async Task<OcrIdCardResult> getresult(string imageBase64)
        {

            string apiKey = null;
            string apifile = "fptapi.txt";
            List<String> list_api = new List<string>();
            if (File.Exists(apifile))
            {
                var list_apistr = File.ReadAllLines(apifile);

                list_api.AddRange(list_apistr);
                if (list_api.Count > 0)
                {
                    apiKey = list_api[0];
                }
            }
            else return null;
            agian:
            if (apiKey == null) { return null; };
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://api.fpt.ai/vision/idr/vnm");
            request.Headers.Add("api-key", apiKey);
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(imageBase64), "image_base64");
            request.Content = content;
            var response = await client.SendAsync(request);
            var content2 = await response.Content.ReadAsStringAsync();

            JObject json = JObject.Parse(content2);
            try
            {
                if (json["message"] != null)
                {


                    string mess = json["message"].ToString();
                    if (mess.IndexOf("limit") > -1)
                    {
                        if (list_api.Contains(apiKey))
                        {
                            list_api.Remove(apiKey);
                        }

                        //using(StreamWriter wr = new StreamWriter(apifile))
                        //{
                        //    foreach(var item in list_api)
                        //    {
                        //        if(item!=apiKey)
                        //        {
                        //            wr.WriteLine(item);
                        //        }
                        //    }
                        //    wr.Close();
                        //}
                        apiKey = list_api[0];
                        goto agian;
                    }
                }
            }
            catch (Exception ex)
            {

            }

            if (json["data"] != null)
            {
                JArray jsonArray = JArray.Parse(json["data"].ToString());
                if (jsonArray.Count > 0)
                {
                    var a = jsonArray[0]["type"];
                    if (a.ToString() == "chip_front" || a.ToString() == "new" || a.ToString() == "old")
                    {
                        OcrIdCardResult reusltocr = new OcrIdCardResult();
                        reusltocr.id_card = jsonArray[0]["id"].ToString();
                        reusltocr.name_card = jsonArray[0]["name"].ToString();
                        reusltocr.address = jsonArray[0]["address"].ToString();
                        DateTime dateTmp;
                        bool isValidDateTime = DateTime.TryParseExact(jsonArray[0]["dob"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTmp);
                        reusltocr.birthday_date = dateTmp;
                        reusltocr.gender = jsonArray[0]["sex"].ToString().ToLower();
                        if (reusltocr.birthday_date.Year.ToString() == new DateTime().Year.ToString()) return null;
                        return reusltocr;
                    }
                    else return null;


                }
                else
                {
                    return null;
                }

            }

            return null;
        }

        public static string ConvertImageToBase64(string imagePath)
        {
            byte[] imageBytes = File.ReadAllBytes(imagePath);
            string base64Image = Convert.ToBase64String(imageBytes);
            return base64Image;
        }
    }
}
