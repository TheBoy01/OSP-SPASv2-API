using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Wordprocessing;
using OSP.Common.Domain.Params;
using OSP.Common.Service.Service;
using OSP.Common.Service.Utility;
using System.Collections.Concurrent;
using System.Net.Mail;
using System.Text;

namespace OSP.Common.Service.APIRepository.Service
{
    public class EmailSenderService : BackgroundService
    {
        private readonly IEmailService _emailService;
        private readonly ConcurrentQueue<EmailMessage> _queue = new ConcurrentQueue<EmailMessage>();
        private readonly SemaphoreSlim _signal = new SemaphoreSlim(0);
        private readonly CancellationToken cancellationToken;
        private IConfiguration _configuration;
        private ServiceUnit _ServiceUnit;
        private TblSendEmail _tblSendEmail;

        public EmailSenderService(IEmailService emailService, IConfiguration configuration)
        {
            _emailService = emailService;
            _configuration = configuration;
        }

        //public void QueueEmail(string to, string subject, string body)
        //{
        //    _queue.Enqueue(new EmailMessage { To = to, Subject = subject, Body = body });
        //    _signal.Release();
        //}

        public override Task StartAsync(CancellationToken cancellationToken)
        {


            // Start the background thread for processing emails
            Task.Run(BackgroundProcessing);
            return Task.CompletedTask;
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            // Clean up any resources if needed
            return base.StopAsync(cancellationToken);
        }

        private async Task BackgroundProcessing()
        {
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    // await _signal.WaitAsync(cancellationToken);

                    //_queue.Enqueue(new EmailMessage
                    //{
                    //    To = "recipient1@example.com",
                    //    Subject = "Hello from ASP.NET Core Email Service",
                    //    Body = "This is a sample email body."
                    //});

                    //_queue.Enqueue(new EmailMessage
                    //{
                    //    To = "recipient2@example.com",
                    //    Subject = "Greetings from ASP.NET Core",
                    //    Body = "Another sample email body."
                    //});


                    OSPParams oSP = new OSPParams();
                    oSP.TblNotificationList = new List<TblNotification>();
                    oSP.tblSendemaildtlsList = new List<TblSendemaildtl>();


                    string URLCommonRepo = _configuration.GetSection("APIBaseURLCommon")["Common.Repository"];
                    string requestAddress = URLCommonRepo + "/CommonRepository/GetNotifications";

                    oSP = await UtilitiesHttpClient<OSPParams>.PostAsyncT<OSPParams>(oSP, requestAddress);

                    TblResponse _response = new TblResponse();
                    _ServiceUnit = new ServiceUnit();
                    List<TblSendEmail> SendEmailList = new List<TblSendEmail>();

                    List<Task> NotifTasks = new List<Task>();
                    var taskCount = 1;

                    StringBuilder sb = new StringBuilder();
                    sb.Append("<div>To: 1,  </div><div class = 'clearfix'></div><br><div>Dear Sir/Ma'am,  </div><div class = 'clearfix'></div><br><div class = 'clearfix'></div><br><div class = 'clearfix'></div><br><div>Please see the attached purchase order/s 123 to your company, and observe the following guidelines:  </div><div class = 'clearfix'></div><br><div class = 'clearfix'></div><br><div>1. For any questions or clarifications on the Purchase Order/s, please inform us via email/viber.  </div><div>2. Caskets must be delivered within 15 days from the receipt of the Purchase Order/s.  </div><div>3. Any casket delivery without any prior approval or Purchase Order will not be accepted or paid.  </div><div>4. Chapels are not allowed to make direct orders to vendors/suppliers. If the chapels ask for additional orders of caskets, please inform us via email or viber.  </div><div>5. All caskets must be in good condition with complete accessories and the deliveries must be in the correct quantity, casket types and sizes.  </div><div>6. Please inform us of each delivery completion via email or viber.  </div><div>7. Submit your invoice and delivery receipts promptly after each delivery to the Head Office or the casket factory to expedite payment processing.  </div><div>8. Sales invoices and delivery receipts must not contain any erasures or alterations.   </div><div class = 'clearfix'></div><br><div class = 'clearfix'></div><br><div class = 'clearfix'></div><br><style type='text/css'>table { border-collapse:collapse; }table,th, td {border: 1px solid black;padding:5px;}.clearfix:after { visibility: hidden; display: block; font-size: 0; content: ' '; clear: both; height: 0; }.clearfix { display: inline-block; }.clearfix { display: block; zoom: 1; </style><div> Thank you.<br><div class = 'clearfix'></div><div> St. Peter Casket – Head Office.<br><div class = 'clearfix'></div><div> ** This is a system generated email. Please do not reply**");

                    string subject = "STRESS TEST ePPS Advisory - Purchase Order - " + DateTime.Now.ToString();
                    
                    
                    var attach = @"\\192.168.1.6\SPASv2$\Files\Reports\POReport\PO12345000137-RM MENDOZA CASKET MANUFACTURING.pdf";
                    List<string> attachmentlist = new List<string>();
                    attachmentlist.Add(attach);

                    var dir = @"\\192.168.1.6\spasv2$\Files\Requisition\SPLPI2407-000074";
                    string[] filePaths = Directory.GetFiles(dir);

                    attachmentlist.AddRange(filePaths);


                    foreach (var item in oSP.TblNotificationList.DistinctBy(a => a.NotificationCode))
                    {

                        switch (item.NotificationCode)
                        {
                            case "EMAIL":

                                int i = 99;
                                foreach (var item1 in oSP.tblSendemaildtlsList)
                                //for (i = 90; i < 99; i++)

                                {
                                    _tblSendEmail = new TblSendEmail()
                                    {
                                        From = "noreplynotifications@stpeter.com.ph",
                                        To = "warrenlb@stpeter.com.ph",
                                        Subject = subject,
                                        //Body = "sample",
                                        Body = sb.ToString(),
                                        Host = "smtp-relay.gmail.com",
                                        Port = "587",
                                        Username = null,
                                        Password = null,
                                           Attachment = attachmentlist.ToArray(),



                                    };
                                    NotifTasks.Add(Task.Run(async () =>
                                                                    {
                                                                        try
                                                                        {
                                                                          //  TblResponse _response = await _ServiceUnit.SendEmailService.SendEmailAsync(_tblSendEmail);
                                                                            // Handle response if needed
                                                                        }
                                                                        catch (Exception ex)
                                                                        {
                                                                            // Handle any errors appropriately (logging, etc.)
                                                                            Console.WriteLine($"Failed to send email: {ex.Message}");
                                                                        }
                                                                    }));


                                }

                                await Task.Delay(i,cancellationToken);
                                break;
                            default:
                                break;
                        }

                        await Task.WhenAll(NotifTasks);  // Wait for all email tasks to complete

                        var delayTime = TimeSpan.FromSeconds(taskCount * 2);  // Adjust the multiplier as necessary
                        await Task.Delay(delayTime, cancellationToken);
                      
                    }

                    //while (_queue.TryDequeue(out var EmailMessage))
                    //{
                    //    try
                    //    {

                    //       //  await _emailService.SendEmailAsync(EmailMessage);
                    //        _response = await _ServiceUnit.SendEmailService.SendEmailAsync(_tblsendemail);

                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        // Handle any errors appropriately (logging, etc.)
                    //        Console.WriteLine($"Failed to send email to {tblSendEmail.To}: {ex.Message}");
                    //    }
                    //}



                    await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken); // Example delay of 10 minutes
                }
            }
            catch (Exception exs)
            {
                Console.WriteLine(exs.Message);
            }


        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Start the background thread for processing emails
            Task.Run(BackgroundProcessing, stoppingToken);
            return Task.CompletedTask;
        }
        private record EmailMessage
        {
            //public string To { get; set; }
            //public string Subject { get; set; }
            //public string Body { get; set; }

            public string ReferenceNo { get; set; }
            public string SystemCode { get; set; }
            public string From { get; set; }
            public string To { get; set; }
            public string Subject { get; set; }
            public string Body { get; set; }
            public IList<string> Attachment { get; set; } = new List<string>();
            public IList<string> CCemails { get; set; }
            public IList<string> BCemails { get; set; }
            public string Host { get; set; }
            public string Port { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
