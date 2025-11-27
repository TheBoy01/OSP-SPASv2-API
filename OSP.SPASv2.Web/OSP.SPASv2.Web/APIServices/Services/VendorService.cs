using OSP.SPASv2.Web.Utility;

namespace OSP.SPASv2.Web.APIServices.Services
{
    public class VendorService
    {
        public async Task<string> GenerateNewVendorCode(string LatestVendorCode)
        {
            string requestAddress = "https://localhost:7223/api/Vendor/GenerateNewVendorCode";

            var query = new Dictionary<string, string>()
            {
                ["LatestVendorCode"] = LatestVendorCode,
                //["branchcode"] = branch,  
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
            string str = await UtilitiesHttpClient<string>.GetJsonstring(requestAddress);
            return str;
        }
    }
}
