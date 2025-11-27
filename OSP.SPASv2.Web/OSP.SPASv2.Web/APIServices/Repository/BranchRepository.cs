using OSP.SPASv2.Web.Utility;

namespace OSP.SPASv2.Web.APIServices.Repository
{
    public class BranchRepository
    {
        public async Task<IList<RefPaymentClass>> GetPaymentTypeList_Jon(string url)
        {
            try
            {
                // var config1 = ip;
                //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Jon/GetPaymentTypeList_Jon";
                string requestAddress = url + "/Jon/GetPaymentTypeList_Jon";
                // string requestAddress = "http://192.168.23.185:80/api/Repository/GetBranchlist";
                //string requestAddress = ip + "/api/Repository/GetBranchlist";
                IList<RefPaymentClass> branches = await UtilitiesHttpClient<RefPaymentClass>.GetJsonlist(requestAddress);
                return branches;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }

        public async Task<IList<RefBranch>> GetBranchlist(string url)
        {
            try
            {
                // var config1 = ip;
                //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Repository/GetBranchlist";
                string requestAddress = url + "/Repository/GetBranchlist";
                // string requestAddress = "http://192.168.23.185:80/api/Repository/GetBranchlist";
                //string requestAddress = ip + "/api/Repository/GetBranchlist";
                IList<RefBranch> branches = await UtilitiesHttpClient<RefBranch>.GetJsonlist(requestAddress);
                return branches;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }

        public async Task<IList<RefBranch>> GetBranches(string url, string company, string branch)
        {
            try
            {
                // var config1 = ip;
                //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Repository/GetBranches";
                //string requestAddress = "http://192.168.23.185/OSPRepo/api/CommonRepository/GetBranches";
                string requestAddress = url + "/CommonRepository/GetBranches";
                //string requestAddress = "http://192.168.23.185:80/api/Repository/GetBranchlist";
                //string requestAddress = ip + "/api/Repository/GetBranchlist";

                var query = new Dictionary<string, string>()
                {
                    //["branchdesc"] = "",
                    ["companydesc"] = company,
                   

                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<RefBranch> branches = await UtilitiesHttpClient<RefBranch>.GetJsonlist(requestAddress);
                return branches;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }

        public async Task<IList<RefBranch>> GetBranchesbyPersonID(string url, string personid)
        {
            try
            {
                // var config1 = ip;
                //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Repository/GetBranches";
                //string requestAddress = "http://192.168.23.185/OSPRepo/api/CommonRepository/GetBranchesbyPersonID";
                string requestAddress = url+"/CommonRepository/GetBranchesbyPersonID";
                //string requestAddress = "http://192.168.23.185:80/api/Repository/GetBranchlist";
                //string requestAddress = ip + "/api/Repository/GetBranchlist";

                var query = new Dictionary<string, string>()
                {
                    //["branchdesc"] = "",
                    ["personid"] = personid,


                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<RefBranch> branches = await UtilitiesHttpClient<RefBranch>.GetJsonlist(requestAddress);
                return branches;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }

        public async Task<qryBranch> GetBranchdetails(string company, string branch,string url)
        {
            try
            {

                
                // var config1 = ip;
                //string requestAddress = "http://192.168.23.185/OSPRepo/CommonRepository/GetBranchdetails";
                string requestAddress = url + "/CommonRepository/GetBranchdetails";
                // string requestAddress = "http://192.168.23.185:80/api/Repository/GetBranchdetails";
                //string requestAddress = ip + "/api/Repository/GetBranchlist";

                var query = new Dictionary<string, string>()
                {
                    ["companydesc"] = company,
                    ["branchcode"] = branch,
                  


                };

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                qryBranch branches = await UtilitiesHttpClient<qryBranch>.GetJsonlist1(requestAddress);
                return branches;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);


            }
        }
    }
}
