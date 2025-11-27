
using OSP.Common.Domain.References;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using OSP.SPASv2.Web.Utility;


namespace OSP.SPASv2.Web.APIServices.Services
{
    public class AccountService
    {



        public async Task<string> EncryptPW(string password)
        {
            try
            {


                string requestAddress = "https://localhost:7090/api/OSPCommon/EncryptPW";
                //string requestAddress = "http://192.168.23.185:80/api/Repository/GetCompanylist";
                //string requestAddress = ip + "/api/Repository/GetCompanylist";

                var query = new Dictionary<string, string>()
                {
                    ["password"] = password,

                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                //var encryptedpw = await UtilitiesHttpClientExtensions.GetString(requestAddress);
                var encryptedpw = await UtilitiesHttpClientExtensions.GetWithQueryStringAsync(requestAddress, query);
                return encryptedpw.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
