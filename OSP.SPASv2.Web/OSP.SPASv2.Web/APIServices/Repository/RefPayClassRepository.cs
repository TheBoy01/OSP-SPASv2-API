using OSP.SPASv2.Web.Utility;

namespace OSP.SPASv2.Web.APIServices.Repository
{
    public class RefPayClassRepository
    {
        public async Task<string> GetPayclassCodeByDesc(string URL,string PayClassDesc)
        {
            string requestAddress = URL + "/Rudy/GetPayclassCodeByDesc";

            var query = new Dictionary<string, string>()
            {
                ["PayClassDesc"] = PayClassDesc
            };

            requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

            return await UtilitiesHttpClient<string>.GetJsonstring(requestAddress);
        }
    }
}
