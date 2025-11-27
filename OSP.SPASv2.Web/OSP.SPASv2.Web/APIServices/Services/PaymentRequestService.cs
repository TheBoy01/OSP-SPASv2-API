using OSP.SPASv2.Web.Utility;

namespace OSP.SPASv2.Web.APIServices.Services
{
    public class PaymentRequestService
    {
        public async Task<string> GenerateNewPRNo(string prno,string companycode, string branchcode,DateTime auditdate,string url)
        {
            //string requestAddress = "https://localhost:7223/api/PaymentRequest/GenerateNewPRNo";
            string requestAddress = url+"/PaymentRequest/GenerateNewPRNo";

            var query = new Dictionary<string, string>()
            {
                ["lastno"] = prno,
                ["companycode"] = companycode,
                ["branchcode"] = branchcode,
                ["auditdate"] = auditdate.ToString("yyyy-MM-ddTHH:mm:ss"),

            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            string str = await UtilitiesHttpClient<string>.GetJsonstring(requestAddress);
            return str;

        }

        public async Task<qryComputeBreakdown> ComputeBreakDown(qryComputeBreakdown _qry,string url)
        {
            try
            {
              
                //string requestAddress = "https://localhost:7223/api/PaymentRequest/ComputeBreakDown";
                string requestAddress = url+"/PaymentRequest/ComputeBreakDown";
                //string requestAddress = "http://192.168.23.185:80/api/Repository/ComputeBreakDown";
                //string requestAddress = ip + "/api/Repository/GetCompanylist";

                //var query = new Dictionary<string, string>()
                //{
                //    ["qty"] = Convert.ToString(_qry.Qty),
                //    ["gross"] = Convert.ToString(_qry.Gross),
                //    ["vatrate"] = Convert.ToString(_qry.VatRate),
                //    ["discount"] = Convert.ToString(_qry.Discount),
                //    ["discountcode"] = _qry.Disccode,

                //};
                qryComputeBreakdown entity = new qryComputeBreakdown();
                //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                //TblAPIResponse<qryComputeBreakdown> response = await UtilitiesHttpClient<qryComputeBreakdown>.PostAsyncEntity<qryComputeBreakdown, TblAPIResponse<qryComputeBreakdown>>(_qry, requestAddress);
                TblAPIResponse<qryComputeBreakdown> response = await UtilitiesHttpClient<qryComputeBreakdown>.PostAsyncEntity<qryComputeBreakdown, TblAPIResponse<qryComputeBreakdown>>(_qry, requestAddress);

                //TblAPIResponse<qryComputeBreakdown,TblResponse> response = await UtilitiesHttpClient<qryComputeBreakdown>.PostAsyncEntity<qryComputeBreakdown, TblAPIResponse<qryComputeBreakdown,TblResponse>>(_qry, requestAddress);


                //qryComputeBreakdown trx = await UtilitiesHttpClient<qryComputeBreakdown>.GetJsonlist1(requestAddress);
                return response.Data;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public  Task<decimal> ComputeTotalAmountItems(IList<tmpPaymentRequestInventory> tmp)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GenerateNewPRBatchNo(string URL, RequisitionParams RequisitionParams)
        {
            string requestAddress = URL + "/Requisition/GenerateBatchNo";

            //var query = new Dictionary<string, string>()
            //{
            //    ["lastno"] = lastno,
            //    ["AuditDate"] = Convert.ToString(AuditDate), 

            //};

            RequisitionParams = await UtilitiesHttpClient<RequisitionParams>.PostAsyncT<RequisitionParams>(RequisitionParams, requestAddress);
            return RequisitionParams.LastNo;
        }

        public async Task<qryRequisitionHdrComputation> ComputeBreakDownHdr(string URL, List<qryRequisitionDtl> qryRequisitionDtl)
        {
            string requestAddress = URL + "/PaymentRequest/ComputeHdrBreakDown";

            //var query = new Dictionary<string, string>()
            //{
            //    ["lastno"] = lastno,
            //    ["AuditDate"] = Convert.ToString(AuditDate), 

            //};

           return  await UtilitiesHttpClient<List<qryRequisitionDtl>>.PostAsyncT<qryRequisitionHdrComputation>(qryRequisitionDtl, requestAddress);
            
        }
    }
}
