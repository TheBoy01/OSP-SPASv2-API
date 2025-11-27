using System;
using System.Collections.Generic;
using System.Text;
//using System.Windows.Forms;
//using System.Data.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Diagnostics;
using System.Reflection;
using System.Data;

namespace OSP.SPASv2.Service
{
    public static class Utilities 
    {

        public static string MdbPw = "197ospLpi@2024$pg5E";
        //public static string ServerPassword = "SPlpI1970@2024SPG5E";
        public static string GetmethodName()
        {
            StackTrace stackTrace = new StackTrace();
            StackFrame frame = stackTrace.GetFrame(1);
            string methodname = frame.GetMethod().Name;
            return methodname;
            // return MethodBase.GetCurrentMethod().Name ;// Console.WriteLine(methodname);
        }

        public static string GetMethodName1()
        {
            MethodBase method = new StackTrace().GetFrame(1)?.GetMethod();
            return method?.Name ?? "Method Not Found";
        }

        public static string Getprojectname()
        {
            string projectname = Assembly.GetExecutingAssembly().GetName().Name;
            return projectname;
        }

        public static bool IsValidEntity(object entitymodel)
        {
            StringBuilder sb = new StringBuilder();
            bool isvalidentity = true;

            ValidationContext validationcontext = new ValidationContext(entitymodel, null, null);
            IList<ValidationResult> errors = new List<ValidationResult>();

            if (!Validator.TryValidateObject(entitymodel, validationcontext, errors, true))
            {
                isvalidentity = false;
                foreach (ValidationResult result in errors)
                {
                    sb.AppendLine(result.ErrorMessage);
                }
            }

            if (isvalidentity == false)
            {
                throw new Exception(sb.ToString());
            }
            else
            {
                return isvalidentity;
            }
        }

        public static string ToCrptDate(DateTime inputdate)
        {
            if (inputdate.Hour == 0 && inputdate.Minute == 0 && inputdate.Second == 0)
            {
                return "DateTime (" + inputdate.Year + ", " + inputdate.Month + ", " + inputdate.Day + ")";
            }
            else
            {
                return "DateTime (" + inputdate.Year + ", " + inputdate.Month + ", " + inputdate.Day + ", " + inputdate.Hour + ", " + inputdate.Minute + ", " + inputdate.Second + ")";
            }
        }




        public static int GetYearInterval(DateTime from, DateTime to)
        {
            DateTime zeroTime = new DateTime(1, 1, 1);

            DateTime a = new DateTime(from.Year, from.Month, from.Day);
            DateTime b = new DateTime(to.Year, to.Month, to.Day);

            TimeSpan span = b - a;
            // because we start at year 1 for the Gregorian 
            // calendar, we must subtract a year here.
            int years = (zeroTime + span).Year - 1;
            return years;
        }

        public static void Compress(FileInfo fileToCompress)
        {
            string path = string.Empty;
            using (FileStream originalFileStream = fileToCompress.OpenRead())
            {
                if ((File.GetAttributes(fileToCompress.FullName) & FileAttributes.Hidden) != FileAttributes.Hidden & fileToCompress.Extension != ".gz")
                {
                    path = fileToCompress.FullName.Substring(0, fileToCompress.FullName.Length - 4) + ".BKP";
                    using (FileStream compressedFileStream = File.Create(path))
                    {
                        using (GZipStream compressionStream = new GZipStream(compressedFileStream, CompressionMode.Compress))
                        {
                            originalFileStream.CopyTo(compressionStream);
                            Console.WriteLine("Compressed {0} from {1} to {2} bytes.",
                                fileToCompress.Name, fileToCompress.Length.ToString(), compressedFileStream.Length.ToString());
                        }
                    }
                }
            }
        }

        public static void Decompress(FileInfo fileToDecompress)
        {

            using (FileStream originalFileStream = fileToDecompress.OpenRead())
            {
                string currentFileName = fileToDecompress.FullName;
                string newFileName = currentFileName.Remove(currentFileName.Length - fileToDecompress.Extension.Length) + ".mdb";

                using (FileStream decompressedFileStream = File.Create(newFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(originalFileStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(decompressedFileStream);
                        Console.WriteLine("Decompressed: {0}", fileToDecompress.Name);
                    }
                }
            }
        }

        public static void SendEmail(string From, string To, string Subject, string Body, string Attachment, bool addCC, IList<string> CCemails)
        {
            try
            {
                // ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                SmtpClient mailClient = new SmtpClient("smtp-relay.gmail.com", 587);
                mailClient.EnableSsl = false;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.UseDefaultCredentials = false;



                //RepositoryUnit _repositoryUnit = new RepositoryUnit();

                //string CredentialPassword = _repositoryUnit.UploadingRepository.getCredentialPasswordByEmail(From);
                // string CredentialPassword = "";
                //mailClient.Credentials = new NetworkCredential(From, CredentialPassword);

                MailMessage email = new MailMessage(From, To, Subject, Body);
                email.IsBodyHtml = true;
                if (Attachment != string.Empty)
                {
                    email.Attachments.Add(new Attachment(Attachment));
                }

                email.CC.Add(From);

                if (CCemails != null)
                {
                    if (CCemails.Count != 0)
                    {
                        foreach (var item in CCemails)
                        {
                            email.CC.Add(item);
                        }
                    }
                }



                mailClient.Send(email);

                string msgbox = "Email successfully sent to " + To + "";
                //MessageBox.Show("Email successfully sent to " + To, "Email Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {

                throw;
            }
           

        }

        public static string GetUrlWithQueryString(string requestUrl,
          Dictionary<string, string> queryStringParams)
        {
            bool startingQuestionMarkAdded = false;
            var sb = new StringBuilder();
            sb.Append(requestUrl);
            foreach (var parameter in queryStringParams)
            {
                if (parameter.Value == null)
                {
                    continue;
                }

                sb.Append(startingQuestionMarkAdded ? '&' : '?');
                sb.Append(parameter.Key);
                sb.Append('=');
                sb.Append(parameter.Value);
                startingQuestionMarkAdded = true;
            }
            return sb.ToString();
        }



    }

    
}
