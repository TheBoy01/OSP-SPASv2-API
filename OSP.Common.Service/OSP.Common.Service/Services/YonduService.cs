using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace OSP.Common.Service.Services
{
    public class YonduService
    {
        public void SendSMSVersion3(String Receiver, String Message)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;

            try
            {
                YonduSMSAPI _SMSApi = new YonduSMSAPI()
                {
                    username = "stpeter",
                    password = "ZY758QNT",
                    msisdn = Receiver,
                    content = Message,
                    shortcode_mask = "STPETER",
                    rcvd_transid = Receiver,
                    is_intl = false
                };

                var stringPayload = JsonConvert.SerializeObject(_SMSApi);

                WebClient client = new WebClient();

                client.Headers["Content-type"] = "application/json";
                client.Headers["Media-type"] = "application/json";
                client.Headers["Accept"] = "application/json";
                client.Encoding = Encoding.UTF8;
                byte[] bytes = Encoding.UTF8.GetBytes(stringPayload);
                //client.Headers["HttpRequestHeader.Content-Length"] = bytes.Length.ToString();
                //client.Headers.Add("HttpRequestHeader.Content-Length", bytes.Length.ToString());
                //client.Headers.Add("HttpRequestHeader.Content-Length", );
                client.Headers.Set("HttpRequestHeader.Content-Length", bytes.Length.ToString());

                //the official URL
                var json = client.UploadString("https://api.m360.com.ph/v3/api/broadcast", stringPayload);

                YonduResponse _YonduResponse = new YonduResponse();

                _YonduResponse = JsonConvert.DeserializeObject<YonduResponse>(json);
                Console.WriteLine(_YonduResponse);


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        public void SendSMS(String Receiver, String Message)
        {
            string textmessage = "Dear Client, " + Environment.NewLine +
             "Let us know about your experience with our services by answering this survey. " + Environment.NewLine +
             "" + Message + Environment.NewLine +
             "Thank you.";

            YonduSMSAPI _SMSApi = new YonduSMSAPI()
            {
                username = "stpeter",
                password = "ZY758QNT",
                msisdn = Receiver,
                content = textmessage,
                shortcode_mask = "STPETER",
                rcvd_transid = Receiver
            };

            var stringPayload = JsonConvert.SerializeObject(_SMSApi);

            //String json = "{\"username\":\"stpeter\",\"password\":\"ZY758QNT\",\"msisdn\":\"" + Receiver + "\",\"content\":\"" + Message + "\",\"shortcode_mask\":\"SMSTrial\",\"rcvd_transid\":\"09193175694\"}";
            // HttpWebRequest requestyondu = (HttpWebRequest)WebRequest.Create("https://smsapi.mobile360.ph/v2/api/broadcast");
            HttpWebRequest requestyondu = (HttpWebRequest)WebRequest.Create("https://api.mobile360.ph/v3/api/broadcast");

            requestyondu.Method = "POST";
            requestyondu.ContentType = "application/json";
            requestyondu.ContentLength = stringPayload.Length;
            requestyondu.KeepAlive = false;

            //System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            try
            {
                using (var streamWriter = new StreamWriter(requestyondu.GetRequestStream()))
                {
                    streamWriter.Write(stringPayload);
                    streamWriter.Close();

                    try
                    {
                        using (HttpWebResponse response = (HttpWebResponse)requestyondu.GetResponse())
                        {
                            if (response.StatusCode == HttpStatusCode.OK)
                            {

                            }
                        }
                    }
                    catch (WebException ex)
                    {
                        throw new Exception(ex.ToString());
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.ToString());
            }

        }
    }
}
