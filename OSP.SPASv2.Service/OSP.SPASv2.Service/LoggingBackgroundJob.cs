//using OSP.SPASv2.Domain.View;
//using OSP.Common.Domain.View;
using OSP.Common.Domain.Tables;
using OSP.SPASv2.Domain.View;
using OSP.SPASv2.Service.Utility;
using Quartz;
using System.Text;

namespace OSP.SPASv2.Service
{
    public class LoggingBackgroundJob : IJob
    {
        private readonly ILogger<LoggingBackgroundJob> _logger;
        private IConfiguration _configuration;
        private string BaseUrlRepo;
        private string BaseUrlService;
        private string OSPUrlRepo;
        private string OSPUrlService;
        private string BaseUrl;
        string BaseURL;
        string BaseURLCommon;
        public LoggingBackgroundJob(ILogger<LoggingBackgroundJob> logger,IConfiguration configuration)
        {
            _logger = logger;
             _configuration = configuration;
            BaseUrlRepo = _configuration.GetSection("APIBaseURL")["SPASv2.Repository"];
            BaseUrlService = _configuration.GetSection("APIBaseURL")["SPASv2.Service"];
            OSPUrlRepo = _configuration.GetSection("APIBaseURLCommon")["Common.Repository"];
            OSPUrlService = _configuration.GetSection("APIBaseURLCommon")["Common.Service"];
            BaseUrl = _configuration.GetSection("BaseURL").Value;

        }

        public Task Execute(IJobExecutionContext context)
        {

            TimeSpan start = new TimeSpan(15, 0, 0); //10 o'clock
            TimeSpan end = new TimeSpan(15, 0, 1); //12 o'clock
            TimeSpan now = DateTime.Now.TimeOfDay;

            if ((now > start) && (now < end))
            {
                ///this.SendEmailAuthorization_PRNO_Scheduled();

            }

            start = new TimeSpan(9, 0, 0); //10 o'clock
            end = new TimeSpan(9, 0, 1); //12 o'clock
            now = DateTime.Now.TimeOfDay;

            if ((now > start) && (now < end))
            {
              //this.SendEmailAuthorization_PRNO_Scheduled();

            }
            return Task.CompletedTask;
        }

        public async Task<string> GetRequestPaymentRequestbyStatus(string status, string personid)
        {
            string requestAddress = BaseURL +"/api/Ron/GetRequestPaymentRequestbyStatus";

            var query = new Dictionary<string, string>()
            {
                ["status"] = "PD-VERIFIER",
                ["personid"] = "1",

            };

            requestAddress = Utility.UtilitiesSched.GetUrlWithQueryString(requestAddress, query);
            IList<qryRequestPaymentRequestbyStatus> vlist2 = await UtilitesHttpClientSched<IList<qryRequestPaymentRequestbyStatus>>.GetJsonlist1(requestAddress);
            string vlist = "";
            return vlist;
        }
        private string strBodyEmail;
        public async Task SendEmailAuthorization_PRNO_Scheduled()
        {

            IList<TblPaymentRequestAuth> _TblPaymentRequestAuth = await this.GetLatestAuthoriztionByAuthorizeLevel();
            IList<string> _personidlist = new List<string>();
            IList<string> _prnolist = new List<string>();
            IList<string> _prnolisttoscctg = new List<string>();

            string authpayclass = string.Empty;
            if (_TblPaymentRequestAuth == null)
            {
                return;
            }

            foreach (string id in _TblPaymentRequestAuth.Select(i => i.PersonID).Distinct())
            {
                _personidlist.Add(id);
            }

            //PRAuthorizationModel _PRAuthorizationModel = new PRAuthorizationModel();

            foreach (var item in _personidlist)
            {

                var _positioncode = await this.GetPositioncode(item);


                if (_positioncode == "SYSTEM")
                {
                    IList<qryPRAuthorizationList> _qryPRAuthorizationList = await this.GetPRAuthorizationLists(item);
                    foreach (string prno in _qryPRAuthorizationList.Select(i => i.Reqno).Distinct())
                    {
                        _prnolisttoscctg.Add(prno);
                    }
                }
                else
                {

                    IList<qryPRAuthorizationList> _qryPRAuthorizationList = await this.GetPRAuthorizationLists(item);

                    foreach (string prno in _qryPRAuthorizationList.Select(i => i.Reqno).Distinct())
                    {
                        _prnolist.Add(prno);
                        authpayclass = await this.GetAuthorizeClass(prno, item);
                    }

                    strBodyEmail = await this.BodyEMAIL_Authorization(_prnolist, item, authpayclass, _prnolist.Count());

                    //await _RepositoryUnit.PRAuthorizationRepository.SendEmailPRAuthorization(_tblsendemail);
                    //return Json(new { success = _resp.Status, errormsg = _resp.ErrorMessage });
                    //return RedirectToAction("PRAuthorizationLists");
                    //return RedirectToAction("PRAuthorizationLists", "Authorization");

                    TblSendEmail _tblsendemail = new TblSendEmail();

                    _tblsendemail.ReferenceNo = DateTime.Now.ToString("MM/dd/yyyy hhmmss");
                    _tblsendemail.SystemCode = "SPASv2";
                    _tblsendemail.From = "ronom@stpeter.com.ph";
                    _tblsendemail.To = "ronom@stpeter.com.ph";
                    _tblsendemail.Subject = "SAMPLE FOR AUTHORIZATION";



                    _tblsendemail.Body = strBodyEmail;
                    _tblsendemail.Attachment = null;

                    IList<string> CCs = new List<string>();
                    //CCs.Add("ronom@stpeter.com.ph");
                    //CCs.Add("davidga@stpeter.com.ph");
                    CCs.Add("warrenlb@stpeter.com.ph");
                    CCs.Add("rudyab@stpeter.com.ph");
                    CCs.Add("jonab@stpeter.com.ph");


                    _tblsendemail.CCemails = CCs;
                    _tblsendemail.BCemails = null;
                    _tblsendemail.Host = "smtp-relay.gmail.com";
                    _tblsendemail.Port = "587";
                    _tblsendemail.Username = null;
                    _tblsendemail.Password = null;

                    //this.SendEmailPRAuthorization(_tblsendemail);
                }

            }



        }

        public async Task<TblResponse> SendEmailPRAuthorization(TblSendEmail _tblSendEmail)
        {
            try
            {
                TblResponse response;
                // var config1 = ip;

                string requestAddress2 = BaseURLCommon + "/SendEmail/SendEmail";


                //await UtilitesHttpClient<string>.PostAsync(_prno, requestAddress2);
                response = await UtilitesHttpClientSched<TblSendEmail>.PostAsync(_tblSendEmail, requestAddress2);

                return response;


            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }
        public async Task<string> GetAuthorizeClass(string _prno, string _personid)
        {
            try
            {

                // var config1 = ip;
                string requestAddress = BaseURL + "/Ron/GetAuthorizeClass";
                string AuthorizeClass = string.Empty;
                var query = new Dictionary<string, string>()
                {
                    ["prno"] = _prno,
                    ["personid"] = _personid,
                };


                requestAddress = Utility.UtilitiesSched.GetUrlWithQueryString(requestAddress, query);

                using (var _httpClient = new HttpClient())
                {
                    var response = await _httpClient.GetAsync(requestAddress);
                    //response.EnsureSuccessStatusCode();
                    AuthorizeClass = await response.Content.ReadAsStringAsync();
                }

                return AuthorizeClass;
                //await UtilitesHttpClient<string>.PostAsync(_prno, requestAddress2);



            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }
        public async Task<IList<TblPaymentRequestAuth>> GetLatestAuthoriztionByAuthorizeLevel()
        {
            try
            {
                string requestAddress = BaseURL + "/Ron/GetLatestAuthoriztionByAuthorizeLevel_ALL";
                //requestAddress = Utilities.GetUrlWithQueryString(requestAddress);
                IList<TblPaymentRequestAuth> vlist = await UtilitesHttpClientSched<TblPaymentRequestAuth>.GetJsonlist(requestAddress);
                return vlist;
                //await UtilitesHttpClient<string>.PostAsync(_prno, requestAddress2);
            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }

        public async Task<string> GetPositioncode(string _personid)
        {
            try
            {
                string poscode = string.Empty;
                // var config1 = ip;
                string requestAddress = BaseURL + "/Ron/GetPositionCode";

                var query = new Dictionary<string, string>()
                {

                    ["personid"] = _personid,
                };


                requestAddress = Utility.UtilitiesSched.GetUrlWithQueryString(requestAddress, query);

                using (var _httpClient = new HttpClient())
                {
                    var response = await _httpClient.GetAsync(requestAddress);
                    //response.EnsureSuccessStatusCode();
                    poscode = await response.Content.ReadAsStringAsync();
                }

                return poscode;
                //await UtilitesHttpClient<string>.PostAsync(_prno, requestAddress2);



            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }

        public async Task<IList<qryPRAuthorizationList>> GetPRAuthorizationLists(string personid)
        {
            try
            {
                // var config1 = ip;
                string requestAddress = BaseURL + "/Ron/GetPRAuthorizationLists?personid=" + personid + "";


                //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<qryPRAuthorizationList> vlist = await UtilitesHttpClientSched<qryPRAuthorizationList>.GetJsonlist(requestAddress);
                return vlist;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }
        private async Task<string> BodyEMAIL_Authorization(IList<string> listprno, string personid, string AuthorizeClass, int cntPR)
        {

            string Name = await this.GetNameofAuthorizer(personid);
            StringBuilder sb = new StringBuilder();

           // string Gender = await _RepositoryUnit.PRAuthorizationRepository.GetGenderByPersonID(personid, BaseUrlRepo);

            //string anotation;

            //if (Gender == "MALE")
            //{
            //    anotation = "Mr.";
            //}
            //else
            //{
            //    anotation = "Ms.";
            //}








            sb.Append("<div>Dear " + Name + " - " + AuthorizeClass + ",   </div>");

            //sb.Append("<div class = 'clearfix'></div><br>");
            //sb.Append("<div> You have been requested to approve the following payment requisition: ");
            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div> You have been requested to approve <strong>" + cntPR.ToString() + "</strong> requisition of the following Payment Request No(s):.");

            sb.Append("<div>");
            sb.Append("<table>");
            sb.Append("<thead>");
            sb.Append("<tr>");
            sb.Append("<th>PR No.</th>");


            sb.Append("</tr>");
            sb.Append("</thead>");
            sb.Append("<tbody>");

            foreach (var item in listprno)
            {
                sb.Append("<tr>");
                sb.Append("<td>" + item + "</td>");
                sb.Append("</tr>");
            }

            sb.Append("</tbody>");
            sb.Append("</table>");
            sb.Append("</div>");
            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("<div>");
            sb.Append("<Button>");
            sb.Append(" <a href=\"https://localhost:7137/\" > Go Website to log in </ a > ");
            sb.Append("</Button>");
            sb.Append("</div>");
            sb.Append("<div class = 'clearfix'></div><br>");

            sb.Append("<style type='text/css'>");
            sb.Append("table { border-collapse:collapse; }");
            sb.Append("table,th, td {border: 1px solid black;padding:5px;}");
            sb.Append(".clearfix:after { visibility: hidden; display: block; font-size: 0; content: ' '; clear: both; height: 0; }");
            sb.Append(".clearfix { display: inline-block; }");
            sb.Append(".clearfix { display: block; zoom: 1; ");
            sb.Append("</style>");
            sb.Append("<div class = 'clearfix'></div><br>");
            sb.Append("This is an automated email sent by https://emailnotification.stpeter.com.ph; do not reply to or forward this email. You are receiving this email because  <br> ");
            sb.Append("you are a workflow participant of this request." + " </div><br>");

            sb.Append("<br><div class = 'clearfix'></div>");

            return sb.ToString();
        }


        public async Task<string> GetNameofAuthorizer(string _personid)
        {
            try
            {
                string poscode = string.Empty;
                // var config1 = ip;
                string requestAddress = BaseURL + "/Ron/GetNameofAuthorizer";

                var query = new Dictionary<string, string>()
                {

                    ["personid"] = _personid,
                };


                requestAddress = Utility.UtilitiesSched.GetUrlWithQueryString(requestAddress, query);

                using (var _httpClient = new HttpClient())
                {
                    var response = await _httpClient.GetAsync(requestAddress);
                    //response.EnsureSuccessStatusCode();
                    poscode = await response.Content.ReadAsStringAsync();
                }

                return poscode;
                //await UtilitesHttpClient<string>.PostAsync(_prno, requestAddress2);



            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }


    }
}
