using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Web.Utility;
using System;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.SqlTypes;
using System.Security.Policy;
using System.Web.Mvc;
using static System.Data.Odbc.ODBC32;

namespace OSP.SPASv2.Web.APIServices.Repository
{
    public class TblRequisitionRepository
    {
        public async Task<TblRequisitiondtl> ReadRequsitionDtlByPRNo(string URL, string ReqNo, string CompanyCode, string DeptCode, string ItemCode)
        {
            string requestAddress = URL + "/Rudy/ReadRequsitionDtlByPRNo";

            var query = new Dictionary<string, string>()
            {
                ["ReqNo"] = ReqNo,
                ["CompanyCode"] = CompanyCode,
                ["DeptCode"] = DeptCode,
                ["ItemCode"] = ItemCode
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            TblRequisitiondtl vlist = await UtilitiesHttpClient<TblRequisitiondtl>.GetJsonlist1(requestAddress);
            return vlist;
        }


        public async Task<TblRequisitionhdr> ReadRequestByPRNo(string URL, string ReqNo)
        {
            string requestAddress = URL + "/Rudy/ReadRequisitionHdr";

            var query = new Dictionary<string, string>()
            {
                ["ReqNo"] = ReqNo
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            TblRequisitionhdr vlist = await UtilitiesHttpClient<TblRequisitionhdr>.GetJsonlist1(requestAddress);
            return vlist;
        }

        public async Task<qryRequisitionInfo> GetRequisitionInfo(string URL, string ReqNo)
        {
            string requestAddress = URL + "/Jon/GetRequisitionInfo";

            var query = new Dictionary<string, string>()
            {
                ["ReqNo"] = ReqNo
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            qryRequisitionInfo vlist = await UtilitiesHttpClient<qryRequisitionInfo>.GetJsonlist1(requestAddress);
            return vlist;
        }

        public async Task<DashboardParams> GetMaxEditDate(string URL)
        {
            string requestAddress = URL + "/Jon/GetMaxEditdate";

            var vlist = await UtilitiesHttpClient<DashboardParams>.PostAsyncT<DashboardParams>(new DashboardParams() { TblResponse = new TblResponse() },requestAddress);
            return vlist;
        }

        public async Task<qryRequisitionInfo> GetRequisitionInfoByApprovalNo(string URL, string ApprovalNo)
        {
            string requestAddress = URL + "/Jon/GetRequisitionInfoByApprovalNo";

            var query = new Dictionary<string, string>()
            {
                ["ApprovalNo"] = ApprovalNo
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            qryRequisitionInfo vlist = await UtilitiesHttpClient<qryRequisitionInfo>.GetJsonlist1(requestAddress);
            return vlist;
        }

        public async Task<qryRequisitionInfo> GetRequisitionInfoByRefNo(string URL, string RefNo)
        {
            string requestAddress = URL + "/Jon/GetRequisitionInfoByRefNo";

            var query = new Dictionary<string, string>()
            {
                ["RefNo"] = RefNo
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            qryRequisitionInfo vlist = await UtilitiesHttpClient<qryRequisitionInfo>.GetJsonlist1(requestAddress);
            return vlist;
        }

        public async Task<IList<qryRequisitionInfo>> GetRequisitionListInfoByMainReqNo(string URL, string MainReqNo)
        {
            string requestAddress = URL + "/Jon/GetRequisitionListInfoByMainReqNo";

            var query = new Dictionary<string, string>()
            {
                ["MainReqNo"] = MainReqNo
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            IList<qryRequisitionInfo> itemList = await UtilitiesHttpClient<qryRequisitionInfo>.GetJsonlist(requestAddress);
            return itemList;
        }

        public async Task<IList<qryRequisitionItem>> GetRequisitionItemList(string URL, string ReqNo)
        {
            string requestAddress = URL + "/Jon/GetRequisitionItemList";

            var query = new Dictionary<string, string>()
            {
                ["ReqNo"] = ReqNo
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            IList<qryRequisitionItem> itemList = await UtilitiesHttpClient<qryRequisitionItem>.GetJsonlist(requestAddress);
            return itemList;
            ;


        }



        public async Task<TblResponse> PostCreateRequisitionHdr(string URL, TblRequisitionhdr _TblRequisitionhdr)
        {
            TblResponse response = new TblResponse();
            try
            {
                string requestAddress = URL + "/Rudy/CreateRequisitionHdr";

                //string requestAddress = "http://192.168.23.185:80/api/Repository/CreatePaymentRequestHdr";
                //string requestAddress = ip + "/api/Repository/GetCompanylist"; 

                response = await UtilitiesHttpClient<TblRequisitionhdr>.PostAsync(_TblRequisitionhdr, requestAddress);
                return response;
            }
            catch (Exception ex)
            {

                response.ErrorMessage = ex.Message;
                return response;
            }

        }
        public async Task<TblResponse> PostCreatePaymentRequisitionHdr(string URL, TblPaymentrequisitionhdr tblPaymentrequisitionhdr)
        {
            string requestAddress = URL + "/Jon/CreatePaymentRequisitionHdr";

            //string requestAddress = "http://192.168.23.185:80/api/Repository/CreatePaymentRequestHdr";
            //string requestAddress = ip + "/api/Repository/GetCompanylist"; 

            TblResponse response = await UtilitiesHttpClient<TblPaymentrequisitionhdr>.PostAsync(tblPaymentrequisitionhdr, requestAddress);
            return response;
        }

        public async Task<TblResponse> PostCreatePaymentRequisitionHdr(string URL, TblRequisitionhdr _TblRequisitionhdr)
        {
            string requestAddress = URL + "/Rudy/CreateRequisitionHdr";

            //string requestAddress = "http://192.168.23.185:80/api/Repository/CreatePaymentRequestHdr";
            //string requestAddress = ip + "/api/Repository/GetCompanylist"; 

            TblResponse response = await UtilitiesHttpClient<TblRequisitionhdr>.PostAsync(_TblRequisitionhdr, requestAddress);
            return response;
        }

        public async Task<TblResponse> PostCreateRequisitionDtl(string URL, TblRequisitiondtl _TblRequisitionDtl)

        {
            string requestAddress = URL + "/Rudy/CreateRequisitionDtl";

            //string requestAddress = "http://192.168.23.185:80/api/Repository/CreatePaymentRequestHdr";
            //string requestAddress = ip + "/api/Repository/GetCompanylist"; 

            TblResponse response = await UtilitiesHttpClient<TblRequisitiondtl>.PostAsync(_TblRequisitionDtl, requestAddress);
            return response;
        }

        public async Task<TblRequisitionhdr> GetLatestPRRow(string URL, string companyCode, string deptCode)
        {
            string requestAddress = URL + "/Rudy/GetLatestPRRow";

            var query = new Dictionary<string, string>()
            {
                ["companycode"] = companyCode,
                ["deptCode"] = deptCode,
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            TblRequisitionhdr vlist = await UtilitiesHttpClient<TblRequisitionhdr>.GetJsonlist1(requestAddress);
            return vlist;
        }

        public async Task<TblResponse> CreateLoanHdr(string URL, RequisitionParams RequisitionParams)
        {
            string requestAddress = URL + "/Rudy/CreateLoanHdr";

            //string requestAddress = "http://192.168.23.185:80/api/Repository/CreatePaymentRequestHdr";
            //string requestAddress = ip + "/api/Repository/GetCompanylist"; 

            TblResponse response = await UtilitiesHttpClient<RequisitionParams>.PostAsync(RequisitionParams, requestAddress);
            return response;
        }

        public async Task<TblResponse> CanVoidRequisition(string URL, RequisitionParams RequisitionParams)
        {
            string requestAddress = URL + "/Wa/CanVoidRequisition";

            //TblResponse response = await UtilitiesHttpClient<RequisitionParams>.PostAsync(RequisitionParams, requestAddress);

            RequisitionParams _RequisitionParams = await UtilitiesHttpClient<RequisitionParams>.PostAsyncT<RequisitionParams>(RequisitionParams, requestAddress);

            return _RequisitionParams.TblResponse;
        }

        public async Task<TblResponse> CheckWa(string URL, qryCompany qry)

        {
            string requestAddress = URL + "/Wa/CheckWa";

            //string requestAddress = "http://192.168.23.185:80/api/Repository/CreatePaymentRequestHdr";
            //string requestAddress = ip + "/api/Repository/GetCompanylist"; 

            TblResponse response = await UtilitiesHttpClient<qryCompany>.PostAsync(qry, requestAddress);
            return response;
        }

        public async Task<string> GetReqPOPY(string URL, string ReqNo)
        {
            string requestAddress = URL + "/Rudy/GetReqPOPY";


            var query = new Dictionary<string, string>()
            {
                ["ReqNo"] = ReqNo
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

            var response = await UtilitiesHttpClient<string>.GetJsonstring(requestAddress);
            return response;
        }

        public async Task<TblResponse> PostUpdateRequestQtySummary(string URL, AuthorizationParams ReqNo)
        { 
            string requestAddress = URL + "/Jon/UpdateRequestQtySummary";
            AuthorizationParams response = await UtilitiesHttpClient<AuthorizationParams>.PostAsyncT<AuthorizationParams>(ReqNo, requestAddress);
            return response.Response;
        }

        public async Task<TblResponse> PostVoidRequisitionByReqNo(string URL, string ReqNo , string UserId)
        {
            string requestAddress = URL + "/Jon/VoidRequisitionByReqNo?ReqNo=" + ReqNo +"&UserId="+ UserId;
            TblResponse response = await UtilitiesHttpClient<string>.PostAsync(ReqNo, requestAddress);
            return response;
        }

        public async Task<IList<qryVendorRunningBalance>> GetVendorRunningBalance(string ip, string PayClassCode, string AsOfMode)
        {
            string requestAddress = ip + "/Jon/GetVendorRunningBalance?PayClassCode=" + PayClassCode + "&AsOfMode=" + AsOfMode;

            IList<qryVendorRunningBalance> vlist = await UtilitiesHttpClient<IList<qryVendorRunningBalance>>.GetJsonlist1(requestAddress);
            return vlist;
        }

        public async Task<string> GetCompanyCode(string URL, string ReqNo)
        {
            string requestAddress = URL + "/Wa/GetCompanyCodeByReqNo";


            var query = new Dictionary<string, string>()
            {
                ["ReqNo"] = ReqNo
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

            var response = await UtilitiesHttpClient<string>.GetJsonstring(requestAddress);
            return response;
        }

        public async Task<TblResponse> CreateDtLSummary(string URL, RequisitionParams requisitionParams)
        {
            string requestAddress = URL + "/Rudy/CreateDtLSummary";

            //var query = new Dictionary<string, string>()
            //{
            //    ["ReqNo"] = ReqNo,
            //    ["AuditUser"] = UserID
            //};
             

            TblResponse response = await UtilitiesHttpClient<RequisitionParams>.PostAsync(requisitionParams, requestAddress);
            return response;
        }

        public async Task<List<TblRequisitionDtlSummary>> GetDtLSummary(string URL, RequisitionParams requisitionParams)
        {
            string requestAddress = URL + "/Rudy/GetDtlSummary";

            //var query = new Dictionary<string, string>()
            //{
            //    ["ReqNo"] = ReqNo,
            //    ["AuditUser"] = UserID
            //};

            RequisitionParams _requisitionParams = await UtilitiesHttpClient<RequisitionParams>.PostAsyncT<RequisitionParams>(requisitionParams, requestAddress);
            return _requisitionParams.TblRequisitionDtlSummary;
        }

        public async Task<TblResponse> InsertBulk(string URL, BatchUploadParams BatchUploadParams)
        {
            string requestAddress = URL + "/Wa/BulkInsertRequisition";

            //var query = new Dictionary<string, string>()
            //{
            //    ["ReqNo"] = ReqNo,
            //    ["AuditUser"] = UserID
            //};

            RequisitionParams _requisitionParams = await UtilitiesHttpClient<BatchUploadParams>.PostAsyncT<RequisitionParams>(BatchUploadParams, requestAddress);
            return _requisitionParams.TblResponse;
        }

        public async Task<BatchUploadParams> InsertRequisitionList(string URL, BatchUploadParams BatchUploadParams)
        {
            string requestAddress = URL + "/Wa/InsertRequisitionPO";

            //var query = new Dictionary<string, string>()
            //{
            //    ["ReqNo"] = ReqNo,
            //    ["AuditUser"] = UserID
            //};

            BatchUploadParams _BatchUploadParams = await UtilitiesHttpClient<BatchUploadParams>.PostAsyncT<BatchUploadParams>(BatchUploadParams, requestAddress);
            return _BatchUploadParams;
        }

        public async Task<BatchUploadParams> ReadRequisitionList(string URL, BatchUploadParams BatchUploadParams)
        {
            string requestAddress = URL + "/Rudy/ReadRequisitionList";

            //var query = new Dictionary<string, string>()
            //{
            //    ["ReqNo"] = ReqNo,
            //    ["AuditUser"] = UserID
            //};

            BatchUploadParams _BatchUploadParams = await UtilitiesHttpClient<BatchUploadParams>.PostAsyncT<BatchUploadParams>(BatchUploadParams, requestAddress);
            return _BatchUploadParams;
        }

        public async Task<BatchUploadParams> InsertBatchPaymentList(string URL, BatchUploadParams BatchUploadParams)
        {
            string requestAddress = URL + "/Wa/InsertBatchPaymentList";

            //var query = new Dictionary<string, string>()
            //{
            //    ["ReqNo"] = ReqNo,
            //    ["AuditUser"] = UserID
            //};

            BatchUploadParams _BatchUploadParams = await UtilitiesHttpClient<BatchUploadParams>.PostAsyncT<BatchUploadParams>(BatchUploadParams, requestAddress);
            return _BatchUploadParams;
        }


        public async Task<BatchUploadParams> ReadBatchPaymentList(string URL, BatchUploadParams BatchUploadParams)
        {
            string requestAddress = URL + "/Wa/ReadBatchPaymentList";

            //var query = new Dictionary<string, string>()
            //{
            //    ["ReqNo"] = ReqNo,
            //    ["AuditUser"] = UserID
            //};

            BatchUploadParams _BatchUploadParams = await UtilitiesHttpClient<BatchUploadParams>.PostAsyncT<BatchUploadParams>(BatchUploadParams, requestAddress);
            return _BatchUploadParams;
        }

        public async Task<BatchUploadParams> GetDRListByReqNo(string URL, string ReqNo)
        {
            string requestAddress = URL + "/Rudy/GetDRListByReqNo";

            //var query = new Dictionary<string, string>()
            //{
            //    ["ReqNo"] = ReqNo,
            //    ["AuditUser"] = UserID
            //};

            var query = new Dictionary<string, string>()
            {
                ["ReqNo"] = ReqNo
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

            var BatchUploadParams = await UtilitiesHttpClient<BatchUploadParams>.GetJsonlist1(requestAddress);
            return BatchUploadParams;
        }

        public async Task<TblResponse> UpdateReqDetails(string URL, qryUpdateReqDetails _qryUpdateReqDetails)
        {
            string requestAddress = URL + "/Rudy/UpdateReqDetails";

            //var query = new Dictionary<string, string>()
            //{
            //    ["ReqNo"] = ReqNo,
            //    ["AuditUser"] = UserID
            //};

            var _respo = await UtilitiesHttpClient<qryUpdateReqDetails>.PostAsyncT<TblResponse>(_qryUpdateReqDetails, requestAddress);
            return _respo;
        }

        public async Task<TblResponse> CanUpdateReqDetails(string URL, qryUpdateReqDetails _qryUpdateReqDetails)
        {
            string requestAddress = URL + "/Rudy/CanUpdateReqDetails";

            var _respo = await UtilitiesHttpClient<qryUpdateReqDetails>.PostAsyncT<TblResponse>(_qryUpdateReqDetails, requestAddress);
            return _respo;
        }
    }
}
