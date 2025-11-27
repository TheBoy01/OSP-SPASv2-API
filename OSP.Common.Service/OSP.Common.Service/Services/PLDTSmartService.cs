using System.Net;
using System.Text;

namespace OSP.Common.Service.Services
{
    
    public class PLDTSmartService
    {
        public void PostDataToSmartMessagingSuite3(string mobilenumber, string message)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            try
            {
                var smsRequest = (HttpWebRequest)WebRequest.Create("https://messagingsuite.smart.com.ph/cgphttp/servlet/sendmsg");

                smsRequest.Method = "POST";

                smsRequest.ContentType = "application/x-www-form-urlencoded";

                //smsRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("davidga@stpeter.com.ph:$M@R1_@p1"));

                //smsRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("davidga@stpeter.com.ph:tN@857Yz"));
                smsRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("davidga@stpeter.com.ph:$m@3TSw3"));//@n@F758.

                string postData = "destination=" + mobilenumber + "&text=" + message + "&source=STPETER";

                ASCIIEncoding encoding = new ASCIIEncoding();

                byte[] byte1 = encoding.GetBytes(postData);

                smsRequest.ContentLength = byte1.Length;

                using (var newStream = smsRequest.GetRequestStream())
                {
                    newStream.Write(byte1, 0, byte1.Length);
                    newStream.Close();

                    using (HttpWebResponse response = (HttpWebResponse)smsRequest.GetResponse())
                    {
                        //MessageBox.Show("Post transaction completed!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
        public void PostDataToSmartMessagingSuite2(string mobilenumber, string message)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;
            try
            {
                var smsRequest = (HttpWebRequest)WebRequest.Create("https://messagingsuite.smart.com.ph/cgphttp/servlet/sendmsg");

                smsRequest.Method = "POST";

                smsRequest.ContentType = "application/x-www-form-urlencoded";

                //smsRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("davidga@stpeter.com.ph:$M@R1_@p1"));

                //smsRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("davidga@stpeter.com.ph:tN@857Yz"));
                smsRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("davidga@stpeter.com.ph:@n@F758."));

                string postData = "destination=" + mobilenumber + "&text=" + message + "&source=STPETER";

                ASCIIEncoding encoding = new ASCIIEncoding();

                byte[] byte1 = encoding.GetBytes(postData);

                smsRequest.ContentLength = byte1.Length;

                using (var newStream = smsRequest.GetRequestStream())
                {
                    newStream.Write(byte1, 0, byte1.Length);
                    newStream.Close();

                    using (HttpWebResponse response = (HttpWebResponse)smsRequest.GetResponse())
                    {
                        //MessageBox.Show("Post transaction completed!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }

        }
        public void PostDataToSmartMessagingSuite1(string mobilenumber, string message)
        {
            ServicePointManager.SecurityProtocol = (SecurityProtocolType)48 | (SecurityProtocolType)192 | (SecurityProtocolType)768 | (SecurityProtocolType)3072;

            try
            {
                HttpWebRequest requestyondu = (HttpWebRequest)WebRequest.Create("https://messagingsuite.smart.com.ph/cgphttp/servlet/sendmsg?destination=" + mobilenumber + "&text=" + message + "&source=STPETER");
                requestyondu.Method = "POST";

                //requestyondu.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("davidga@stpeter.com.ph:tN@857Yz"));
                requestyondu.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("davidga@stpeter.com.ph:@n@F758"));

                requestyondu.ContentLength = 0;
                requestyondu.ContentType = "application/json";
                requestyondu.MediaType = "application/json";
                requestyondu.Accept = "application/json";

                //requestyondu.getenc = Encoding.UTF8;
                //byte[] bytes = Encoding.UTF8.GetBytes(stringPayload);

                using (HttpWebResponse response = (HttpWebResponse)requestyondu.GetResponse())
                {
                    //MessageBox.Show("Post transaction completed!");
                    Console.Write("Post transaction completed!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException + ":" + ex.Message);
            }

        }
        public void PostDataToSmartMessagingSuite(string mobilenumber, string message)
        {
            try
            {
                HttpWebRequest requestyondu = (HttpWebRequest)WebRequest.Create("https://messagingsuite.smart.com.ph/cgphttp/servlet/sendmsg?destination=" + mobilenumber + "&text=" + message + "&source=DEMOSTPETER");
                requestyondu.Method = "POST";
                requestyondu.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes("davidga@stpeter.com.ph:$M@R1_@p1"));
                requestyondu.ContentLength = 0;

                using (HttpWebResponse response = (HttpWebResponse)requestyondu.GetResponse())
                {
                    //MessageBox.Show("Post transaction completed!");
                    Console.Write("Post transaction completed!");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException + ":" + ex.Message);
            }

        }

        //public void PostDataToSmartMessagingSuite2(string mobilenumber, string message)
        //{

        //    try
        //    {
        //        var smsRequest = (HttpWebRequest)WebRequest.Create("https://messagingsuite.smart.com.ph/cgphttp/servlet/sendmsg");

        //        smsRequest.Method = "POST";

        //        smsRequest.ContentType = "application/x-www-form-urlencoded";

        //        smsRequest.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("davidga@stpeter.com.ph:$M@R1_@p1"));

        //        string postData = "destination=" + mobilenumber + "&text=" + message + "&source=DEMOSTPETER";

        //        ASCIIEncoding encoding = new ASCIIEncoding();

        //        byte[] byte1 = encoding.GetBytes(postData);

        //        smsRequest.ContentLength = byte1.Length;

        //        using (var newStream = smsRequest.GetRequestStream())
        //        {
        //            newStream.Write(byte1, 0, byte1.Length);
        //            newStream.Close();

        //            using (HttpWebResponse response = (HttpWebResponse)smsRequest.GetResponse())
        //            {
        //                //MessageBox.Show("Post transaction completed!");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.ToString());
        //    }

        //}
    }
}
