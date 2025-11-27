using OSP.SPASv2.Web.Utility;

namespace OSP.SPASv2.Web.APIServices.Repository
{
    public class RefDiscountRepository
    {
        public async Task<IList<RefDiscount>> GetRefDiscount(string url)
        {
            try
            {
                //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Wa/GetRefDiscount";
                string requestAddress = url+"/Wa/GetRefDiscount";
                //string requestAddress = "http://192.168.23.185:80/api/Repository/GetRefDiscount";
                //string requestAddress = ip + "/api/Repository/GetCompanylist";

                //var query = new Dictionary<string, DateTime>()
                //{
                //    //["auditdate"] = auditdate.ToString("yyyy-MM-DDTHH:mm:ss"),
                //    ["auditdate"] = auditdate,

                //};

                //requestAddress = Utilities.GetUrlWithQueryStringDateTime(requestAddress, query);

                IList<RefDiscount> trx = await UtilitiesHttpClient<RefDiscount>.GetJsonlist(requestAddress);
                return trx;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
    }
}
