using DocumentFormat.OpenXml.Spreadsheet;
using Google.Api.Gax.Grpc;
using Microsoft.Extensions.Options;
using MimeKit;
using OSP.Common.Domain;
using OSP.Common.Domain.Tables;
using OSP.Common.Service.ServiceContract;
using System.Net.Http;
using System.Net.Mail;
using System.ServiceModel.Channels;

namespace OSP.Common.Service.OperationContract 
{
    public class SendEmailService : ISendEmailService<TblSendEmail>
    {

        private readonly MailSettings _mailSettings;
        private readonly HttpClient _httpClient;


        //public SendEmailService(IOptions<MailSettings> mailSettingsOptions, IHttpClientFactory httpClientFactory)
        //{
        //    _mailSettings = mailSettingsOptions.Value;
        //    _httpClient = httpClientFactory.CreateClient("MailTrapApiClient");
        //}




        public void SendEmail()
        {
            string From = "ronom@stpeter.com.ph";
            string To = "ronom@stpeter.com.ph";
            string Subject = "Sample";
            string Body = "Hello World!";
            string Attachment = "";
            bool addCC = true;
            IList<string> CCemails = null;

            try
            {
                // ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                SmtpClient mailClient = new SmtpClient("smtp-relay.gmail.com", 587);
                mailClient.EnableSsl = false;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.UseDefaultCredentials = false;

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
        public void SendEmail(TblSendEmail _tblsendemail)
        {

            try
            {
                // ServicePointManager.ServerCertificateValidationCallback = delegate(object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                SmtpClient mailClient = new SmtpClient(_tblsendemail.Host, Convert.ToInt16(_tblsendemail.Port));
                mailClient.EnableSsl = false;
                mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                mailClient.UseDefaultCredentials = false;

                MailMessage email = new MailMessage(_tblsendemail.From, _tblsendemail.To, _tblsendemail.Subject, _tblsendemail.Body);
                email.IsBodyHtml = true;



                if (_tblsendemail.Attachment != null)
                {
                    foreach (var item in _tblsendemail.Attachment)
                    {
                        email.Attachments.Add(new Attachment(item));

                    }

                }



                if (_tblsendemail.CCemails != null)
                {
                    if (_tblsendemail.CCemails.Count != 0)
                    {
                        foreach (var item in _tblsendemail.CCemails)
                        {
                            email.CC.Add(item);
                        }
                    }
                }

                if (_tblsendemail.BCemails != null)
                {
                    if (_tblsendemail.BCemails.Count != 0)
                    {
                        foreach (var item in _tblsendemail.BCemails)
                        {
                            email.Bcc.Add(item);
                        }
                    }
                }

                mailClient.Send(email);
                mailClient.Dispose();

                if (_tblsendemail.Attachment != null)
                {
                    email.Attachments.Dispose();

                }

                string msgbox = "Email successfully sent to " + _tblsendemail.To + "";
                //MessageBox.Show("Email successfully sent to " + To, "Email Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                throw;
            }
        }



        public async Task<TblResponse> SendEmailAsync(TblSendEmail _tblsendemail)
        {
            try
            {
                using (SmtpClient mailClient = new SmtpClient(_tblsendemail.Host, Convert.ToInt16(_tblsendemail.Port)))
                {
                    mailClient.EnableSsl = false;
                    mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                    mailClient.UseDefaultCredentials = false;
                    List<string> emailList = new List<string>(_tblsendemail.To.Split(';'));

                    using (MailMessage email = new MailMessage(_tblsendemail.From, emailList.First(), _tblsendemail.Subject, _tblsendemail.Body))
                    {
                        email.IsBodyHtml = true;

                        // Output the list
                        foreach (var PerEmail in emailList)
                        {
                            if (!emailList.Contains(PerEmail))
                            {
                                email.To.Add(PerEmail);
                                email.To.Add("warrenlb@stpeter.com.ph");
                            }
                        }

                        if (_tblsendemail.Attachment != null)
                        {
                            foreach (var item in _tblsendemail.Attachment)
                            {
                                email.Attachments.Add(new Attachment(item));
                            }
                        }

                        if (_tblsendemail.CCemails != null && _tblsendemail.CCemails.Count != 0)
                        {
                            foreach (var item in _tblsendemail.CCemails)
                            {
                                email.CC.Add(item);
                            }
                        }

                        if (_tblsendemail.BCemails != null && _tblsendemail.BCemails.Count != 0)
                        {
                            foreach (var item in _tblsendemail.BCemails)
                            {
                                email.Bcc.Add(item);
                            }
                        }

                        await mailClient.SendMailAsync(email);

                        if (_tblsendemail.Attachment != null)
                        {
                            foreach (var attachment in email.Attachments)
                            {
                                attachment.Dispose();
                            }
                        }

                        return new TblResponse()
                        {
                            Status = "SUCCESS",
                            ErrorMessage = "Email successfully sent"

                        };

                        string msgbox = "Email successfully sent to " + _tblsendemail.To;
                        // MessageBox.Show("Email successfully sent to " + To, "Email Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); // You can log the exception here or handle it as per your needs
            }
        }



        public TblSendEmail SendEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SendMailAsync(TblSendEmail mailData)
        {
            try
            {
                //using (MimeMessage emailMessage = new MimeMessage())
                //{
                //    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                //    emailMessage.From.Add(emailFrom);
                //    MailboxAddress emailTo = new MailboxAddress(mailData.To, mailData.To);
                //    emailMessage.To.Add(emailTo);

                //    // you can add the CCs and BCCs here.
                //    //emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
                //    //emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                //    emailMessage.Subject = mailData.Subject;

                //    BodyBuilder emailBodyBuilder = new BodyBuilder();
                //    emailBodyBuilder.TextBody = mailData.Body;

                //    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                //    //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                //    using (SmtpClient mailClient = new SmtpClient())
                //    {
                //        //await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                //        //await mailClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
                //        //await mailClient.SendAsync(emailMessage);
                //        //await mailClient.DisconnectAsync(true);
                //        await mailClient.SendMailAsync(emailMessage);
                //    }
                //}

                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                return false;
            }
        }

        public bool SendHTMLMailAsync(TblSendEmail mailData)
        {

            try
            {
                //using (MimeMessage emailMessage = new MimeMessage())
                //{
                //    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                //    emailMessage.From.Add(emailFrom);

                //    MailboxAddress emailTo = new MailboxAddress(htmlMailData.EmailToName, htmlMailData.EmailToId);
                //    emailMessage.To.Add(emailTo);

                //    emailMessage.Subject = "Hello";

                //    string filePath = Directory.GetCurrentDirectory() + "\\Templates\\Hello.html";
                //    string emailTemplateText = File.ReadAllText(filePath);

                //    emailTemplateText = string.Format(emailTemplateText, htmlMailData.EmailToName, DateTime.Today.Date.ToShortDateString());

                //    BodyBuilder emailBodyBuilder = new BodyBuilder();
                //    emailBodyBuilder.HtmlBody = emailTemplateText;
                //    emailBodyBuilder.TextBody = "Plain Text goes here to avoid marked as spam for some email servers.";

                //    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                //    using (SmtpClient mailClient = new SmtpClient())
                //    {
                //        mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                //        mailClient.Authenticate(_mailSettings.SenderEmail, _mailSettings.Password);
                //        mailClient.Send(emailMessage);
                //        mailClient.Disconnect(true);
                //    }
                //}

                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                return false;
            }
        }

        public bool SendMailWithAttachmentsAsync(TblSendEmail mailData)
        {
            try
            {
                //using (MimeMessage emailMessage = new MimeMessage())
                //{
                //    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                //    emailMessage.From.Add(emailFrom);
                //    MailboxAddress emailTo = new MailboxAddress(mailData.To, mailData.To);
                //    emailMessage.To.Add(emailTo);

                //    // you can add the CCs and BCCs here.
                //    //emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
                //    //emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                //    emailMessage.Subject = mailData.Subject;

                //    BodyBuilder emailBodyBuilder = new BodyBuilder();
                //    emailBodyBuilder.TextBody = mailData.Body;

                //    if (mailData.Attachment != null)
                //    {
                //        foreach (var attachmentFile in mailData.Attachment)
                //        {
                //            if (attachmentFile.Length == 0)
                //            {
                //                continue;
                //            }

                //            using (MemoryStream memoryStream = new MemoryStream())
                //            {
                //                attachmentFile.CopyTo(memoryStream);
                //                var attachmentFileByteArray = memoryStream.ToArray();

                //                emailBodyBuilder.Attachments.Add(attachmentFile.FileName, attachmentFileByteArray, ContentType.Parse(attachmentFile.ContentType));
                //            }
                //        }
                //    }

                //    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                //    //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                //    using (SmtpClient mailClient = new SmtpClient())
                //    {
                //        mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                //        mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                //        mailClient.Send(emailMessage);
                //        mailClient.Disconnect(true);
                //    }
                //}

                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                return false;
            }
        }
    }
}
