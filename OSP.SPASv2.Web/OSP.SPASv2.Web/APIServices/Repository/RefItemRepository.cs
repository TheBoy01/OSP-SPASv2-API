using OSP.SPASv2.Web.Utility;
using System.Data;
using System.Security.Policy;

namespace OSP.SPASv2.Web.APIServices.Repository
{
    public class RefItemRepository
    {
        private string APIBaseURLRepo;
        private string APIBaseURLCommonRepo;

        public RefItemRepository()
        { 
            
        }

        public async Task<string> GetItemCodeByDesc(string URL,string itemDesc)
        {
            string requestAddress = URL + "/Rudy/GetItemCodeByDesc";

            var query = new Dictionary<string, string>()
            {
                ["itemDesc"] = itemDesc
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

            return await UtilitiesHttpClient<string>.GetJsonstring(requestAddress);
        }

        public async Task<string> GetItemDesc(string URL, string itemDesc)
        {
            string requestAddress = URL + "/Rudy/GetItemDesc";

            var query = new Dictionary<string, string>()
            {
                ["itemcode"] = itemDesc
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

            return await UtilitiesHttpClient<string>.GetJsonstring(requestAddress);
        }
    }
}
