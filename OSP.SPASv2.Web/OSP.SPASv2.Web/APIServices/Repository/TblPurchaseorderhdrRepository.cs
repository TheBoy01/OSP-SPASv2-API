 using OSP.SPASv2.Web.Utility;
using System.Security.Policy;

namespace OSP.SPASv2.Web.APIServices.Repository
{
    public class TblPurchaseorderhdrRepository
    {
        public async Task<TblPurchaseorderhdr> GetPOHdrByReqNo(string URL, string ReqNo)
        {
            string requestAddress = URL + "/Jon/GetPOHdrByReqNo";

            var query = new Dictionary<string, string>()
            {
                ["ReqNo"] = ReqNo
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            TblPurchaseorderhdr vlist = await UtilitiesHttpClient<TblPurchaseorderhdr>.GetJsonlist1(requestAddress);
            return vlist;
        }
        public async Task<TblPurchaseorderhdr> GetPOHdrByPONo(string URL,string PONo)
        {
            string requestAddress = URL + "/Jon/GetPOHdrByPONo";

            var query = new Dictionary<string, string>()
            {
                ["PONo"] = PONo
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            TblPurchaseorderhdr vlist = await UtilitiesHttpClient<TblPurchaseorderhdr>.GetJsonlist1(requestAddress);
            return vlist;
        }

        public async Task<TblResponse> PostCreatePurchaseOrderHdr(string URL, TblPurchaseorderhdr _TblPurchaseorderhdr)
        {
            string requestAddress = URL + "/Jon/CreateTblPurchaseOrderHdr";

            //string requestAddress = "http://192.168.23.185:80/api/Repository/CreatePaymentRequestHdr";
            //string requestAddress = ip + "/api/Repository/GetCompanylist"; 

            TblResponse response = await UtilitiesHttpClient<TblPurchaseorderhdr>.PostAsync(_TblPurchaseorderhdr, requestAddress);
            return response;
        }

        public async Task<TblResponse> CreateBatchApproval(string URL, TblBatchApproval _TblBatchApproval)
        {
            string requestAddress = URL + "/Ron/CreateTblBatchApproval";
            TblResponse response = await UtilitiesHttpClient<TblBatchApproval>.PostAsync(_TblBatchApproval, requestAddress);
            return response;
        }


        public async Task<string> GetLatestPONo(string URL,string CompanyCode)
        {
            try
            {

                string requestAddress = URL + "/Jon/GetLatestPONo";
                string LatestPONo = string.Empty;
                var query = new Dictionary<string, string>()
                {
                    ["CompanyCode"] = CompanyCode,
                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                using (var _httpClient = new HttpClient())
                {
                    var response = await _httpClient.GetAsync(requestAddress);
                    //response.EnsureSuccessStatusCode();
                    LatestPONo = await response.Content.ReadAsStringAsync();
                }
                return LatestPONo;
            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }

        public async Task<string> GetLastestBANo(string URL, string reqtype,string payclasscode)
        {
            try
            {

                string requestAddress = URL + "/Ron/GetLastestBANo";
                string LatestPONo = string.Empty;
                var query = new Dictionary<string, string>()
                {
                    ["reqtype"] = reqtype,
                    ["payclasscode"] = payclasscode,

                };
                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                using (var _httpClient = new HttpClient())
                {
                    var response = await _httpClient.GetAsync(requestAddress);
                    //response.EnsureSuccessStatusCode();
                    LatestPONo = await response.Content.ReadAsStringAsync();
                }
                return LatestPONo;
            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }

        }




    }
}
