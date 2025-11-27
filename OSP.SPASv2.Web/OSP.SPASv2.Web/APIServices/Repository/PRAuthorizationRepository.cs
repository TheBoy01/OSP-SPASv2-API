using Microsoft.Reporting.Map.WebForms.BingMaps;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Web.Utility;
using System.Collections.Generic;
using System.Data;
using System.Formats.Asn1;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;

namespace OSP.SPASv2.Web.APIServices.Repository
{
    public class PRAuthorizationRepository
    {

        
        string BaseURL;
        string BaseURLCommon;
        private IConfiguration _configuration;
        TblResponse _response = new TblResponse();
        public PRAuthorizationRepository()
        {
            ////BaseURL = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("APIBaseURL")["SPASv2.Repository"];
           
            //BaseURL = "http://192.168.23.185/SPASv2Repo/api";
            //BaseURLCommon = "http://192.168.23.185/OSPRepo/api";




        }


        public async Task<IList<qryPRAuthorizationList>> GetPRAuthorizationLists(string personid,string url)
        {
            try
            {
                // var config1 = ip;
                string requestAddress = url + "/Ron/GetPRAuthorizationLists?personid=" +personid+"";


                //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<qryPRAuthorizationList> vlist = await UtilitiesHttpClient<qryPRAuthorizationList>.GetJsonlist(requestAddress);
                return vlist;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }

        public async Task<IList<qryPRAuthorizationList>> GetPRAuthorizationLists_Batch(string personid,string BatchPRNo,string url)
        {
            try
            {
                // var config1 = ip;
                string requestAddress = url + "/Ron/GetPRAuthorizationLists_Batch";

             

                var query = new Dictionary<string, string>()
                {
                    ["personid"] = personid,

                    ["BatchPRNo"] = BatchPRNo,
                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<qryPRAuthorizationList> vlist = await UtilitiesHttpClient<qryPRAuthorizationList>.GetJsonlist(requestAddress);
                return vlist;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }

        public async Task<IList<qryPRAuthorizationList>> GetPRAuthorizationLists_BatchByPayclassCode(string personid, string payclasscode, string url)
        {
            try
            {
                // var config1 = ip;
                string requestAddress = url + "/Ron/GetPRAuthorizationLists_BatchByPayclassCode";



                var query = new Dictionary<string, string>()
                {
                    ["personid"] = personid,

                    ["payclasscode"] = payclasscode,
                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<qryPRAuthorizationList> vlist = await UtilitiesHttpClient<qryPRAuthorizationList>.GetJsonlist(requestAddress);
                return vlist;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }



        public async Task<IList<TblPaymentRequestAuth>> GetALLTblPaymentRequestAuthByAuthorizeLevel(string url, string prno)
        {
            try
            {
                // var config1 = ip;
                string requestAddress = url + "/Ron/GetALLTblPaymentRequestAuthByAuthorizeLevel?prno=" + prno + "";


                //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<TblPaymentRequestAuth> vlist = await UtilitiesHttpClient<TblPaymentRequestAuth>.GetJsonlist(requestAddress);
                return vlist;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }

        public async Task<TblPaymentRequestAuth> GetALLTblPaymentRequestAuthByPersonId(string BaseURL, string prno,string personid)
        {
            try
            {
                // var config1 = ip;
                string requestAddress = BaseURL + "/Ron/GetALLTblPaymentRequestAuthByPersonId";

                var query = new Dictionary<string, string>()
                {
                    ["prno"] = prno,

                    ["personid"] = personid,
                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                TblPaymentRequestAuth vlist = await UtilitiesHttpClient<TblPaymentRequestAuth>.GetJsonlist1(requestAddress);
                return vlist;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }

        public async Task<IList<qryListOfAuthorizerPayclass>> GetAuthorizerPayclassLists(string companytype, string payclass)
        {
            try
            {
                // var config1 = ip;
                string requestAddress = BaseURL + "/Ron/GetAuthorizerPayClassLists";


                var query = new Dictionary<string, string>()
                {
                    ["companytype"] = companytype,
               
                    ["payclass"] = payclass,
                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<qryListOfAuthorizerPayclass> vlist = await UtilitiesHttpClient<qryListOfAuthorizerPayclass>.GetJsonlist(requestAddress);
                return vlist;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }

        public async Task<TblResponse> CreatePRAuthorization(string URL, string ip,string _prno,string _reqtype)
        {
            try
            {
                // var config1 = ip;
                string requestAddress = URL + "/Ron/CreatePRAuthorization?prno=" + _prno + "&reqtype="+ _reqtype + "";
                TblResponse response = await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress);
                return response;


            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }

        public async Task<TblResponse> UpdatePRAuthorization(string ip, string _prno,string url)
        {
            try
            {
                // var config1 = ip;
                string requestAddress = url + "/Ron/UpdatePRAuthorization?prno=" + _prno + "";
                TblResponse response = await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress);
                return response;


            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }

        public async Task<TblResponse> ApprovePRAuthorization(qryUpdateStatusAuth _qryUpdateStatusAuth,string url)
        {
            try
            {
                TblResponse response;
                // var config1 = ip;
                string requestAddress = url + "/Ron/ApprovePRAuthorization";
               
                response = await UtilitiesHttpClient<qryUpdateStatusAuth>.PostAsync(_qryUpdateStatusAuth, requestAddress);
                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);
              
                return response;


            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }
        public async Task<TblResponse> ApprovePRAuthorizationRush(qryUpdateStatusAuth _qryUpdateStatusAuth, string url)
        {
            try
            {
                TblResponse response;
                // var config1 = ip;
                string requestAddress = url + "/Ron/ApprovePRAuthorizationRush";

                response = await UtilitiesHttpClient<qryUpdateStatusAuth>.PostAsync(_qryUpdateStatusAuth, requestAddress);
                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);


                return response;


            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }

        public async Task<TblResponse> UpdateReadPRAuthorization(qryUpdateStatusAuth _qryUpdateStatusAuth,string url)
        {
            try
            {
                TblResponse response;
                // var config1 = ip;
                string requestAddress = url + "/Ron/UpdateReadPRAuthorization";

                response = await UtilitiesHttpClient<qryUpdateStatusAuth>.PostAsync(_qryUpdateStatusAuth, requestAddress);
                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);


                return response;


            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }

        public async Task<TblResponse> DisapprovePRAuthorization(qryUpdateStatusAuth _qryUpdateStatusAuth,string url)
        {
            try
            {
                TblResponse response;
                // var config1 = ip;
                string requestAddress = url + "/Ron/DisapprovePRAuthorization";
                response = await UtilitiesHttpClient<qryUpdateStatusAuth>.PostAsync(_qryUpdateStatusAuth, requestAddress);
                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);
                return response;
            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }

        public async Task<TblResponse> EndorseToAccounting(IList<string> _prno, string _personid,string URL)
        {
            try
            {
                TblResponse response;
                // var config1 = ip;
                AuthorizationParams _AuthorizationParams = new AuthorizationParams();
                string requestAddress = URL + "/Rudy/EndorseToAccounting";
                //string _listprno = JsonSerializer.Serialize<IList<string>>(_prno);
                //var query = new Dictionary<string, string>()
                //{
                //    ["prno"] = _listprno,
                //    ["_AuditUser"] = _personid,
                //};

                _AuthorizationParams.ReqNo = _prno;
                _AuthorizationParams.UserCode = _personid;


                //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                response = await UtilitiesHttpClient<AuthorizationParams>.PostAsync(_AuthorizationParams, requestAddress);
                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);

                return response;  
            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }


        public async Task<TblResponse> InsertReqReason(TblRequisitionReason _TblRequisitionReason, string URL)
        {
            try
            {
                TblResponse response;
                string requestAddress = URL + "/Ron/InsertReqReason";
                response = await UtilitiesHttpClient<TblRequisitionReason>.PostAsync(_TblRequisitionReason, requestAddress);
                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);

                return response;
            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }

        public async Task<TblResponse> ProcessAuthorization(AuthorizationParams authorizationParams, string URL)
        {
            try
            {
                
                string requestAddress = URL + "/Ron/ProcessAuthorization";
                var response = await UtilitiesHttpClient<AuthorizationParams>.PostAsync(authorizationParams, requestAddress);

                return response;
            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }







        public async Task<string> GetAuthorizeClass(string _prno, string _personid,string url)
        {
            try
            {

                // var config1 = ip;
                string requestAddress = url + "/Ron/GetAuthorizeClass";
                string AuthorizeClass = string.Empty;
                var query = new Dictionary<string, string>()
                {
                    ["prno"] = _prno,
                    ["personid"] = _personid,
                };


                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                using (var _httpClient = new HttpClient())
                {
                    var response = await _httpClient.GetAsync(requestAddress);
                    //response.EnsureSuccessStatusCode();
                    AuthorizeClass = await response.Content.ReadAsStringAsync();
                }

                return AuthorizeClass;
                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);



            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }

        public async Task<string> GetEmailByPersonID(string _personid, string url)
        {
            try
            {

                // var config1 = ip;
                string requestAddress = url + "/Ron/GetEmailByPersonID";
                string email = string.Empty;
                var query = new Dictionary<string, string>()
                {
                    ["personid"] = _personid,
                };


                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                using (var _httpClient = new HttpClient())
                {
                    var response = await _httpClient.GetAsync(requestAddress);
                    //response.EnsureSuccessStatusCode();
                    email = await response.Content.ReadAsStringAsync();
                }

                return email;
                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);



            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }

        public async Task<IList<qryGroupEmails>> GetEmailsByGroupId(string groupid, string url)
        {
            try
            {
                string requestAddress = url + "/Ron/GetEmailsByGroupId";
                IList<string> email;
                var query = new Dictionary<string, string>()
                {
                    ["groupid"] = groupid,
                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                var vlist = await UtilitiesHttpClient<IList<qryGroupEmails>>.GetJsonlist1(requestAddress);
                return vlist;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);
            }

        }

            public async Task<string> GetGenderByPersonID(string _personid, string url)
        {
            try
            {

                // var config1 = ip;
                string requestAddress = url + "/Ron/GetGenderByPersonID";
                string email = string.Empty;
                var query = new Dictionary<string, string>()
                {
                    ["personid"] = _personid,
                };


                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                using (var _httpClient = new HttpClient())
                {
                    var response = await _httpClient.GetAsync(requestAddress);
                    //response.EnsureSuccessStatusCode();
                    email = await response.Content.ReadAsStringAsync();
                }

                return email;
                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);



            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }

        public async Task<string> GetAuthorizeClass(string _personid,string url)
        {
            try
            {

                string requestAddress = url + "/Ron/GetAuthorizeClassByPersonId";
                string AuthorizeClass = string.Empty;
                var query = new Dictionary<string, string>()
                {
                    ["personid"] = _personid,
                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                using (var _httpClient = new HttpClient())
                {
                    var response = await _httpClient.GetAsync(requestAddress);
                    //response.EnsureSuccessStatusCode();
                    AuthorizeClass = await response.Content.ReadAsStringAsync();
                }
                return AuthorizeClass;
            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }

        public async Task<string> GetAuthorizeClasswURL(string _personid,string url)
        {
            try
            {

                string requestAddress = url + "/Ron/GetAuthorizeClassByPersonId";
                string AuthorizeClass = string.Empty;
                var query = new Dictionary<string, string>()
                {
                    ["personid"] = _personid,
                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                using (var _httpClient = new HttpClient())
                {
                    var response = await _httpClient.GetAsync(requestAddress);
                    //response.EnsureSuccessStatusCode();
                    AuthorizeClass = await response.Content.ReadAsStringAsync();
                }
                return AuthorizeClass;
            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }

        public async Task<TblPaymentRequestAuth> GetLatestAuthoriztionByAuthorizeLevel(string _prno,string url)
        {
            try
            {
                string requestAddress = url + "/Ron/GetLatestAuthoriztionByAuthorizeLevel";
               
                var query = new Dictionary<string, string>()
                {
                    ["prno"] = _prno,
                };
                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                TblPaymentRequestAuth vlist = await UtilitiesHttpClient<TblPaymentRequestAuth>.GetJsonlist1(requestAddress);

              
                return vlist;
                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }

        public async Task<IList<TblPaymentRequestAuth>> GetLatestAuthoriztionByAuthorizeLevel(string url)
        {
            try
            {
                string requestAddress = url + "/Ron/GetLatestAuthoriztionByAuthorizeLevel_ALL";


                //requestAddress = Utilities.GetUrlWithQueryString(requestAddress);
                IList<TblPaymentRequestAuth> vlist = await UtilitiesHttpClient<TblPaymentRequestAuth>.GetJsonlist(requestAddress);

                return vlist;
                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }
        
        public async Task<IList<qryPaymentRequestAuthDtl>> GetPaymentRequestAuthListByPRNO(string _prno, string url)
        {
            try
            {
                string requestAddress = url + "/Ron/GetPaymentRequestAuthListByPRNO";

                var query = new Dictionary<string, string>()
                {
                    ["prno"] = _prno,
                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<qryPaymentRequestAuthDtl> vlist = await UtilitiesHttpClient<IList<qryPaymentRequestAuthDtl>>.GetJsonlist1(requestAddress);


                return vlist;
                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);

            }
            catch (Exception ex)
            {
                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);
            }

        }

        public async Task<TblResponse> SendEmailPRAuthorization(TblSendEmail _tblSendEmail,string url)
        {
            
            try
            {
                // var config1 = ip;
                string requestAddress2 = url + "/SendEmail/SendEmail";
                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);
                _response = await UtilitiesHttpClient<TblSendEmail>.PostAsync(_tblSendEmail, requestAddress2);
                _response.Status = "SUCCESS";
                _response.ErrorMessage = "Process Completed.";
                
                return _response;
            }
            catch (Exception ex)
            {
                
                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                //throw new Exception(errorMessage);

                _response.ErrorMessage = errorMessage;
                return _response;

            }

        }


        public async Task<string> GetPositioncode(string _personid,string url)
        {
            try
            {
                string poscode = string.Empty;
                // var config1 = ip;
                string requestAddress = url + "/Ron/GetPositionCode";
           
                var query = new Dictionary<string, string>()
                {
                  
                    ["personid"] = _personid,
                };


                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                using (var _httpClient = new HttpClient())
                {
                    var response = await _httpClient.GetAsync(requestAddress);
                    //response.EnsureSuccessStatusCode();
                    poscode = await response.Content.ReadAsStringAsync();
                }

                return poscode;
                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);



            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }

        public async Task<IList<qryRequestPaymentRequestbyStatus>> GetRequestPaymentRequestbyStatus(string status, string personid,string ip)
        {
            string requestAddress = ip + "/Ron/GetRequestPaymentRequestbyStatus";

            var query = new Dictionary<string, string>()
            {
                ["status"] = status,
                ["personid"] = personid,

            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            IList<qryRequestPaymentRequestbyStatus> vlist = await UtilitiesHttpClient<IList<qryRequestPaymentRequestbyStatus>>.GetJsonlist1(requestAddress);
            return vlist;
        }

        public async Task<IList<qryRequisitionDepartment>> GetRequisitionDepartment(string personid, string ip)
        {
            string requestAddress = ip + "/Jon/GetRequisitionDepartment";

            var query = new Dictionary<string, string>()
            {
                ["personid"] = personid,

            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            IList<qryRequisitionDepartment> vlist = await UtilitiesHttpClient<IList<qryRequisitionDepartment>>.GetJsonlist1(requestAddress);
            return vlist;
        }


        public async Task<string> GetNameofAuthorizer(string _personid,string url)
        {
            try
            {
                string poscode = string.Empty;
                // var config1 = ip;
                string requestAddress = url + "/Ron/GetNameofAuthorizer";

                var query = new Dictionary<string, string>()
                {

                    ["personid"] = _personid,
                };


                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                using (var _httpClient = new HttpClient())
                {
                    var response = await _httpClient.GetAsync(requestAddress);
                    //response.EnsureSuccessStatusCode();
                    poscode = await response.Content.ReadAsStringAsync();
                }

                return poscode;
                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);



            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }

        public async Task<DateTime> GetRequestDate(string item,string url)
        {
            try
            {
               
                string requestAddress = url + "/Ron/GetRequestDate";
                string Daterequest;
                var query = new Dictionary<string, string>()
                {
                    ["prno"] = item,
                };
                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                

                using (var _httpClient = new HttpClient())
                {
                    var response = await _httpClient.GetAsync(requestAddress);
                    Daterequest = await response.Content.ReadAsStringAsync();

                    Daterequest = JsonSerializer.Deserialize<string>(Daterequest);

                    return Convert.ToDateTime(Daterequest);
                }



                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);



            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }

        public async Task<DateTime> GetRequestDateByBatchNo(string item,string url)
        {
            try
            {

                string requestAddress = url + "/Ron/GetRequestDateByBatchNo";
                string Daterequest;
                var query = new Dictionary<string, string>()
                {
                    ["batchprno"] = item,
                };
                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);


                using (var _httpClient = new HttpClient())
                {
                    var response = await _httpClient.GetAsync(requestAddress);
                    Daterequest = await response.Content.ReadAsStringAsync();

                    Daterequest = JsonSerializer.Deserialize<string>(Daterequest);

                    return Convert.ToDateTime(Daterequest);
                }



                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);



            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }

        public async Task<IList<TblRequisitionhdr>> GetBatchPRNoList(string personid,string url)
        {
            try
            {
                string requestAddress = url + "/Ron/GetBatchPRNo";


                var query = new Dictionary<string, string>()
                {
                    
                    ["personid"] = personid,

                };

               requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<TblRequisitionhdr> vlist = await UtilitiesHttpClient<IList<TblRequisitionhdr>>.GetJsonlist1(requestAddress);
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        public async Task<IList<qryDeclineReason>> GetDeclineReason(string url)
        {
            try
            {
                string requestAddress = url + "/Ron/GetDeclineReason";
                IList<qryDeclineReason> vlist = await UtilitiesHttpClient<IList<qryDeclineReason>>.GetJsonlist1(requestAddress);
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }





        public async Task<IList<TblRequisitionhdr>> GetBatchPRNoListByPayclassCode(string personid,string payclasscode, string url)
        {
            try
            {
                string requestAddress = url + "/Ron/GetBatchPRNoByPayclassCode";


                var query = new Dictionary<string, string>()
                {

                    ["personid"] = personid,
                    ["payclasscode"] = payclasscode,

                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<TblRequisitionhdr> vlist = await UtilitiesHttpClient<IList<TblRequisitionhdr>>.GetJsonlist1(requestAddress);
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        public async Task<IList<qryPaymentClassAuthorization>> GetPaymentClassAuthorization(string personid, string url)
        {
            try
            {
                string requestAddress = url + "/Ron/GetPaymentClassAuthorization";


                var query = new Dictionary<string, string>()
                {
                    ["personid"] = personid,
                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<qryPaymentClassAuthorization> vlist = await UtilitiesHttpClient<IList<qryPaymentClassAuthorization>>.GetJsonlist1(requestAddress);
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        public async Task<IList<qryRptPurchaseOrderDetails>> GetRptPODtl(string pono, string url)
        {
            try
            {
                string requestAddress = url + "/Ron/GetRptPODtl";


                var query = new Dictionary<string, string>()
                {

                    ["pono"] = pono,

                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<qryRptPurchaseOrderDetails> vlist = await UtilitiesHttpClient<IList<qryRptPurchaseOrderDetails>>.GetJsonlist1(requestAddress);
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<IList<qryRptPurchaseOrderConsolidated>> GetRptPurchaseOrderConsolidated(IList<string> reqno,string vendorname, string url)
        {
            try
            {
                string requestAddress = url + "/Ron/GetRptPurchaseOrderConsolidated";


                string _listreqno = JsonSerializer.Serialize<IList<string>>(reqno);
           

                var query = new Dictionary<string, string>()
                {

                    ["reqno"] = _listreqno,
                    ["vendorname"] = vendorname
                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<qryRptPurchaseOrderConsolidated> vlist = await UtilitiesHttpClient<IList<qryRptPurchaseOrderConsolidated>>.GetJsonlist1(requestAddress);
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<qryRptPurchaseOrderHdr> GetRptPOHdr(string pono, string url)
        {
            try
            {
                string requestAddress = url + "/Ron/GetRptPOHdr";


                var query = new Dictionary<string, string>()
                {

                    ["pono"] = pono,

                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                qryRptPurchaseOrderHdr vlist = await UtilitiesHttpClient<qryRptPurchaseOrderHdr>.GetJsonlist1(requestAddress);
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<qryPOSignatories> GetPOSignatories(string pono, string url)
        {
            try
            {
                string requestAddress = url + "/Ron/GetPOSignatories";


                var query = new Dictionary<string, string>()
                {

                    ["pono"] = pono,

                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                qryPOSignatories vlist = await UtilitiesHttpClient<qryPOSignatories>.GetJsonlist1(requestAddress);
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<string> GetBatchNoByPRNo(string prno,string url)
        {
            try
            {
                string poscode = string.Empty;
                // var config1 = ip;
                string requestAddress = url + "/Ron/GetBatchNoByPRNo";

                var query = new Dictionary<string, string>()
                {

                    ["prno"] = prno,
                };


                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                using (var _httpClient = new HttpClient())
                {
                    var response = await _httpClient.GetAsync(requestAddress);
                    //response.EnsureSuccessStatusCode();
                    poscode = await response.Content.ReadAsStringAsync();
                }

                return poscode;
                //await UtilitiesHttpClient<string>.PostAsync(_prno, requestAddress2);



            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }



    }
}
