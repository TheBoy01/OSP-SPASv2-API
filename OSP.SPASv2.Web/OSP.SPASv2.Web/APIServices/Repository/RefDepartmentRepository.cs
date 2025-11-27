using OSP.SPASv2.Web.Utility;
using System.Data;
using OSP.SPASv2.Domain;

namespace OSP.SPASv2.Web.APIServices.Repository
{
    public class RefDepartmentRepository
    {
        string APIBaseURLRepo;
        string APIBaseURLCommonRepo;

        public RefDepartmentRepository()
        {
        }

        public async Task<IList<RefDepartment>> GetDeptByPersonID(string personid, string companytype, string url)
        {
            try
            {

                //string requestAddress = "http://192.168.23.185/OSPRepo/api/CommonRepository/GetDeptByPersonID";

                //string requestAddress = "http://192.168.23.185/OSPRepo/api/CommonRepository/GetDeptByPersonID";
                string requestAddress = url + "/CommonRepository/GetDeptByPersonID";

                var query = new Dictionary<string, string>()
                {
                    ["personid"] = personid,
                    ["companytype"] = companytype,

                };

                GenericModel<string, string> model1 = ModelBuilder1.BuildModel<string, string>(personid, companytype);

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);
                IList<RefDepartment> branches = await UtilitiesHttpClient<RefDepartment>.GetJsonlist(requestAddress);
                //IList<RefDepartment> branches = await UtilitiesHttpClient<RefDepartment>.JsonUrlList<RefDepartment>(requestAddress);

                return branches;

            }
            catch (Exception ex)
            {

                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);
            }
        }

        public async Task<RefDepartment> GetCompanyCode(string deptDesc, string companyType, string url)
        {
            try
            {
                //string requestAddress = "http://192.168.23.185/OSPRepo/api/CommonRepository/GetCompanyCode";
                string requestAddress = url + "/CommonRepository/GetCompanyCode";

                var query = new Dictionary<string, string>()
                {
                    ["DeptDesc"] = deptDesc,
                    ["CompanyType"] = companyType,
                };

                //GenericModel<string, string> model1 = ModelBuilder1.BuildModel<string, string>(personid, companytype);

                requestAddress = Utilities.GetUrlWithQueryString(requestAddress, query);

                return await UtilitiesHttpClient<RefDepartment>.GetJsonlist1(requestAddress);
                //IList<RefDepartment> branches = await UtilitiesHttpClient<RefDepartment>.JsonUrlList<RefDepartment>(requestAddress); 
            }
            catch (Exception ex)
            {
                string errorMessage = DateTime.Now + "\n" + ex.Message + "\n" + ex.InnerException + "\n" + ex.ToString();
                throw new Exception(errorMessage);
            }
        }
    }
}
