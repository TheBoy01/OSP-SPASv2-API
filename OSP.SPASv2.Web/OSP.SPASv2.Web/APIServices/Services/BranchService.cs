
using OSP.Common.Domain.References;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

using OSP.SPASv2.Web.Utility;

namespace OSP.SPASv2.Web.APIServices.Services
{
    public class BranchService
    {

       

        public async Task<IList<RefBranch>> GetBranchlist(string url)
        {
            try
            {
                // var config1 = ip;
                //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Repository/GetBranchlist";
                string requestAddress = url+"/Repository/GetBranchlist";
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

        public async Task<IList<RefBranch>> GetBranchlist1(string url,string company,string branch)
        {
            try
            {
                // var config1 = ip;
                //string requestAddress = "http://192.168.23.185/SPASv2Repo/api/Repository/GetBranchlist";
                string requestAddress = url+"/Repository/GetBranchlist";
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
    }
}
