using Microsoft.Extensions.Options;
using MailKit;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;
using MailKit.Security;


namespace OSP.Common.Service.APIRepository.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body);
       // Task<TblResponse> SendEmailAsync(TblSendEmail _tblsendemail);
    }

    public class EmailService : IEmailService
    {
        private readonly SmtpClientSettings _smtpSettings;

        public EmailService(IOptions<SmtpClientSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

       


        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Your Name", "your-email@example.com"));
            emailMessage.To.Add(new MailboxAddress("", to)); // Correct way to add recipient

            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart("plain")
            {
                Text = body
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, SecureSocketOptions.StartTls);

                // Note: Since you're using credentials, it's important to use the secure authentication method
                await client.AuthenticateAsync(new SaslMechanismOAuth2("your-email@example.com", "your-password"));

                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }


        //public async Task<TblResponse> SendEmailAsync(TblSendEmail _tblsendemail)
        //{
        //    try
        //    {
        //        using (SmtpClient mailClient = new SmtpClient(_tblsendemail.Host, Convert.ToInt16(_tblsendemail.Port)))
        //        {
        //            mailClient.EnableSsl = false;
        //            mailClient.DeliveryMethod = SmtpDeliveryMethod.Network;
        //            mailClient.UseDefaultCredentials = false;

        //            using (MailMessage email = new MailMessage(_tblsendemail.From, _tblsendemail.To, _tblsendemail.Subject, _tblsendemail.Body))
        //            {
        //                email.IsBodyHtml = true;

        //                if (_tblsendemail.Attachment != null)
        //                {
        //                    foreach (var item in _tblsendemail.Attachment)
        //                    {
        //                        email.Attachments.Add(new Attachment(item));
        //                    }
        //                }

        //                if (_tblsendemail.CCemails != null && _tblsendemail.CCemails.Count != 0)
        //                {
        //                    foreach (var item in _tblsendemail.CCemails)
        //                    {
        //                        email.CC.Add(item);
        //                    }
        //                }

        //                if (_tblsendemail.BCemails != null && _tblsendemail.BCemails.Count != 0)
        //                {
        //                    foreach (var item in _tblsendemail.BCemails)
        //                    {
        //                        email.Bcc.Add(item);
        //                    }
        //                }

        //                await mailClient.SendMailAsync(email);

        //                if (_tblsendemail.Attachment != null)
        //                {
        //                    foreach (var attachment in email.Attachments)
        //                    {
        //                        attachment.Dispose();
        //                    }
        //                }

        //                return new TblResponse()
        //                {
        //                    Status = "SUCCESS",
        //                    ErrorMessage = "Email successfully sent"

        //                };

        //                string msgbox = "Email successfully sent to " + _tblsendemail.To;
        //                // MessageBox.Show("Email successfully sent to " + To, "Email Sent", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message); // You can log the exception here or handle it as per your needs
        //    }
        //}
    }

    public class SmtpClientSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
    }
}
