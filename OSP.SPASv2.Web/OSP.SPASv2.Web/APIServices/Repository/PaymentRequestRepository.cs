using OSP.SPASv2.Web.Utility;

namespace OSP.SPASv2.Web.APIServices.Repository
{
    public class PaymentRequestRepository
    {

        private string APIBaseURLRepo;
        private string APIBaseURLCommonRepo;

        public PaymentRequestRepository()
        {
            
          
        }

        public async Task<IList<tmpPaymentRequestInventory>> GettmpPaymentRequestInventory(string url)
        {
            // var s = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build().GetSection("AppSettings")["APP_Name
            //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Wa/GettmpPaymentRequestInventory";
            string requestAddress = url+"/Wa/GettmpPaymentRequestInventory";
            //string requestAddress = "http://192.168.23.185:80/api/Repository/GettmpPaymentRequestInventory";
            //string requestAddress = ip + "/api/Repository/GetCompanylist";

            //var query = new Dictionary<string, string>()
            //{
            //    ["PRNo"] = PRNo,

            //};

            //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

            IList<tmpPaymentRequestInventory> inventory = await UtilitiesHttpClient<tmpPaymentRequestInventory>.GetJsonlist(requestAddress);
            return inventory;


        }

        public async Task<IList<tmpPaymentRequestInventory>> GettmpPaymentRequestInventoryA(string audituser, string prno, string url)
        {
            //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Wa/GettmpPaymentRequestInventoryA";
            string requestAddress = url+"/Wa/GettmpPaymentRequestInventoryA";
            //string requestAddress = "http://192.168.23.185:80/api/Repository/GettmpPaymentRequestInventoryA";
            //string requestAddress = ip + "/api/Repository/GetCompanylist";

            var query = new Dictionary<string, string>()
            {
                ["audituser"] = audituser,
                ["prno"] = prno,

            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

            IList<tmpPaymentRequestInventory> inventory = await UtilitiesHttpClient<tmpPaymentRequestInventory>.GetJsonlist(requestAddress);
            return inventory;


        }

        public async Task<TblResponse> PosttmpPaymentRequestInventory(tmpPaymentRequestInventory tmp,string url)
        {
            //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Wa/PosttmpPaymentRequestInventory";
            string requestAddress = url + "/Wa/PosttmpPaymentRequestInventory";
            //string requestAddress = "http://192.168.23.185:80/api/Repository/GettmpPaymentRequestInventory";
            //string requestAddress = ip + "/api/Repository/GetCompanylist";

            //var query = new Dictionary<string, string>()
            //{
            //    ["PRNo"] = PRNo,

            //};

            //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

            TblResponse response = await UtilitiesHttpClient<tmpPaymentRequestInventory>.PostAsync(tmp, requestAddress);
            return response;


        } 

        public async Task<TblResponse> PostCreatePaymentRequestHdr(TblPaymentrequesthdr tblprhdr,string url)
        {
            //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Wa/CreatePaymentRequestHdr";
            string requestAddress = url+"/Wa/CreatePaymentRequestHdr";

            //string requestAddress = "http://192.168.23.185:80/api/Repository/CreatePaymentRequestHdr";
            //string requestAddress = ip + "/api/Repository/GetCompanylist";




            TblResponse response = await UtilitiesHttpClient<TblPaymentrequesthdr>.PostAsync(tblprhdr, requestAddress);
            return response;


        }

        public async Task<TblResponse> CreatePaymentRequestDtl(IList<TblPaymentrequestdtl> tblprdtl,string url)
        {
            //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Wa/CreatePaymentRequestDtl";
            string requestAddress = url + "/Wa/CreatePaymentRequestDtl";

            TblResponse response = await UtilitiesHttpClient<IList<TblPaymentrequestdtl>>.PostAsync(tblprdtl, requestAddress);
            return response;


        }

        public async Task<TblPaymentrequesthdr> GetLatestPRRow(string companycode, string branchcode,string url)
        {
            //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Wa/GetLatestPRRow";
            string requestAddress = url+"/Wa/GetLatestPRRow";

            var query = new Dictionary<string, string>()
            {
                ["companycode"] = companycode,
                ["branchcode"] = branchcode,


            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            TblPaymentrequesthdr vlist = await UtilitiesHttpClient<TblPaymentrequesthdr>.GetJsonlist1(requestAddress);
            return vlist;

        }

        public async Task<qryPaymentRequestHdr> GetPaymentRequestHdr(string prno,string url)
        {
            //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Wa/GetPaymentrequesthdr";
            string requestAddress = url+"/Wa/GetPaymentrequesthdr";

            var query = new Dictionary<string, string>()
            {
                ["prno"] = prno,



            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            qryPaymentRequestHdr vlist = await UtilitiesHttpClient<qryPaymentRequestHdr>.GetJsonlist1(requestAddress);
            return vlist;
        }

      

        public async Task<TblPaymentrequesthdr> ReadRequestByPRNo(string prno,string url)
        {
            try
            {
                //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Wa/ReadPaymentrequesthdr";
                string requestAddress = url+"/Wa/ReadPaymentrequesthdr";

                var query = new Dictionary<string, string>()
                {
                    ["prno"] = prno,



                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                TblPaymentrequesthdr vlist = await UtilitiesHttpClient<TblPaymentrequesthdr>.GetJsonlist1(requestAddress);
                return vlist;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<TblResponse> UpdatePaymentRequestHdr(TblPaymentrequesthdr prhdr,string url)
        {
            try
            {
                //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Wa/UpdatePaymentRequestHdr";
                string requestAddress = url + "/Wa/UpdatePaymentRequestHdr";

                //string requestAddress = "http://192.168.23.185:80/api/Repository/UpdatePaymentRequestHdr";


                TblResponse response = await UtilitiesHttpClient<TblPaymentrequesthdr>.PostAsync(prhdr, requestAddress);
                return response;


            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        //public async Task<TblResponse> UploadFiles(List<IFormFile> files, string ReferenceNo, string UploadingFilePath)
        //{
        //    try
        //    {
        //        string requestAddress = "http://192.168.23.185/OSPRepo/api/Wa/ReadPaymentrequesthdr";

        //        var query = new Dictionary<string, string>()
        //        {
        //            ["files"] = files,
        //            ["ReferenceNo"] = ReferenceNo,
        //            ["UploadingFilePath"] = UploadingFilePath,
        //        };

        //        requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
        //        TblPaymentrequesthdr vlist = await UtilitiesHttpClient<TblPaymentrequesthdr>.GetJsonlist1(requestAddress);
        //        return vlist;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

    }
}
