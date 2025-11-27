using OSP.SPASv2.Web.Utility;

namespace OSP.SPASv2.Web.APIServices.Repository
{
    public class ChapelRepository
    {
        public async Task<IList<RefChapel>> GetChapels(string url, string company, string chapel)
        {
            try
            {
                // var config1 = ip;
                //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Repository/GetChapels";
                //string requestAddress = "http://192.168.23.185/OSPRepo/api/CommonRepository/GetChapels";
                string requestAddress = url+"/CommonRepository/GetChapels";
                // string requestAddress = "http://192.168.23.185:80/api/Repository/GetBranchlist";
                //string requestAddress = ip + "/api/Repository/GetBranchlist";

                var query = new Dictionary<string, string>()
                {
                    //["chapeldesc"] = chapel,
                    ["companydesc"] = company,


                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<RefChapel> chapels = await UtilitiesHttpClient<RefChapel>.GetJsonlist(requestAddress);
                return chapels;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }

        public async Task<IList<RefChapel>> GetChapelsbyPersonID(string url, string personid)
        {
            try
            {
                // var config1 = ip;
                //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Repository/GetChapels";
                //string requestAddress = "http://192.168.23.185/OSPRepo/api/CommonRepository/GetChapelsByPersonID";
                string requestAddress = url+"/CommonRepository/GetChapelsByPersonID";
                // string requestAddress = "http://192.168.23.185:80/api/Repository/GetBranchlist";
                //string requestAddress = ip + "/api/Repository/GetBranchlist";

                var query = new Dictionary<string, string>()
                {
                    //["chapeldesc"] = chapel,
                    ["personid"] = personid,


                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<RefChapel> chapels = await UtilitiesHttpClient<RefChapel>.GetJsonlist(requestAddress);
                return chapels;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }
    }
}
