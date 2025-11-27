using Newtonsoft.Json;
using OSP.Common.Domain.Tables;
using OSP.Common.Service.ServiceContract;
using System.Net;
using System.Text;

namespace OSP.Common.Service.Services
{
    public class SendSMSService
    {
        public void SendSMSYondu(TblSendSMSYondu _tblSendSMS)
        {
            //ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;



            HttpClientHandler handler = new HttpClientHandler();
            // Set the desired security protocols
            handler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true;
            handler.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls11 | System.Security.Authentication.SslProtocols.Tls;


            try
            {
                SMSModels.SMSApi _SMSApi = new SMSModels.SMSApi()
                {
                    username = "stpeter",
                    password = "ZY758QNT",
                    msisdn = _tblSendSMS.Receiver,
                    content = _tblSendSMS.Message,
                    shortcode_mask = "STPETER",
                    rcvd_transid = _tblSendSMS.Receiver,
                    is_intl = false
                    //app_key = appkey,
                    //app_secret = appsecret
                };

                var stringPayload = JsonConvert.SerializeObject(_SMSApi);

                WebClient client = new WebClient();

                client.Headers["Content-type"] = "application/json";
                client.Headers["Media-type"] = "application/json";
                client.Headers["Accept"] = "application/json";
                client.Encoding = Encoding.UTF8;
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

        public void SendSMSSmart(TblSendSMSSmart _tblSendSMSSmart)
        {

            String _username = _tblSendSMSSmart.Username;
            String _password = _tblSendSMSSmart.Password;

            HttpWebRequest _smartSMS = (HttpWebRequest)WebRequest.Create("https://messagingsuite.smart.com.ph/cgphttp/servlet/sendmsg?destination=63" + _tblSendSMSSmart.Receiver.Substring(_tblSendSMSSmart.Receiver.Length - 10) + "&text=" + _tblSendSMSSmart.Message + "&source=STPETER");
            _smartSMS.Method = "POST";
            _smartSMS.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("davidga@stpeter.com.ph:$m@3TSw3"));
            byte[] bytes = Encoding.UTF8.GetBytes(_tblSendSMSSmart.Message);
            _smartSMS.ContentType = "application/json";
            _smartSMS.MediaType = "application/json";
            _smartSMS.Accept = "application/json";
            _smartSMS.Headers.Set("HttpRequestHeader.Content-Length", bytes.Length.ToString());

            try
            {
                using (HttpWebResponse response = (HttpWebResponse)_smartSMS.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        
                    }
                }
            }
            catch (WebException ex)
            {
                
            }


        }
    }
}
