using OSP.SPASv2.Web.Utility;

namespace OSP.SPASv2.Web.APIServices.Repository
{
    public class ReftrxweekRepository
    {
        public async Task<RefTrxweek> Getreftrxweek(DateTime auditdate,string url)
        {
            try
            {
                //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Wa/GetReftrxweek";
                string requestAddress = url + "/Wa/GetReftrxweek";
                //string requestAddress = "http://192.168.23.185:80/api/Repository/GetPaymenttypes";
                //string requestAddress = ip + "/api/Repository/GetCompanylist";

                var query = new Dictionary<string, DateTime>()
                {
                    //["auditdate"] = auditdate.ToString("yyyy-MM-DDTHH:mm:ss"),
                    ["auditdate"] = auditdate,

                };

                requestAddress = Utilities.GetUrlWithQueryStringDateTime(requestAddress, query);

                RefTrxweek trx = await UtilitiesHttpClient<RefTrxweek>.GetJsonlist1(requestAddress);
                return trx;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
    }
}
