using OSP.SPASv2.Web.Utility;

namespace OSP.SPASv2.Web.APIServices.Repository
{
    public class RefPaymentTypeRepository
    {
        public async Task<IList<RefPaymentClass>> GetPaymentTypes(string url, string paydesc)
        {
            //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Repository/GetPaymentTypes";
            string requestAddress = url+ "/Repository/GetPaymentTypes";
            //string requestAddress = "http://192.168.23.185:80/api/Repository/GetPaymentTypes";
            //string requestAddress = ip + "/api/Repository/GetCompanylist";

            var query = new Dictionary<string, string>()
            {
                ["paydesc"] = paydesc,

            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

            IList<RefPaymentClass> paytypes = await UtilitiesHttpClient<RefPaymentClass>.GetJsonlist(requestAddress);
            return paytypes;
        }

        public async Task<IList<RefPaymentClass>> GetPaymenttypes(string url)
        {
            try
            {
                //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Repository/GetPaymenttypes";
                string requestAddress = url + "/Repository/GetPaymenttypes";
                //string requestAddress = "http://192.168.23.185:80/api/Repository/GetPaymenttypes";
                //string requestAddress = ip + "/api/Repository/GetCompanylist";

                //var query = new Dictionary<string, string>()
                //{
                //    ["paydesc"] = paydesc,

                //};

                //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                IList<RefPaymentClass> paytypes = await UtilitiesHttpClient<RefPaymentClass>.GetJsonlist(requestAddress);
                return paytypes;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
           
        }
        public async Task<IList<RefPaymentClass>> GetPaymenttypeList(string url)
        {
            try
            {
                //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Repository/GetPaymentTypeList";
                string requestAddress = url+"/Repository/GetPaymentTypeList";
                //string requestAddress = "http://192.168.23.185:80/api/Repository/GetPaymenttypeList";
                //string requestAddress = ip + "/api/Repository/GetCompanylist";
                //string requestAddress = ip + "/api/Repository/GetCompanylist";

                //var query = new Dictionary<string, string>()
                //{
                //    ["paydesc"] = paydesc,

                //};

                //requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                IList<RefPaymentClass> paytypes = await UtilitiesHttpClient<RefPaymentClass>.GetJsonlist(requestAddress);
                return paytypes;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<RefPaymentClass> PaymentClassDetails(string url, string payclass)
        {
            try
            {
                string requestAddress = url + "/Repository/PaymentClassDetails";
                var query = new Dictionary<string, string>()
                {
                    ["PayClassCode"] = payclass,

                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                RefPaymentClass paytypes = await UtilitiesHttpClient<RefPaymentClass>.GetJsonlist1(requestAddress);
                return paytypes;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }



    }
}
