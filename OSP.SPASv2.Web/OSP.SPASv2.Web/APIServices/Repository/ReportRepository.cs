using OSP.SPASv2.Web.Utility;
using System.Security.Policy;

    namespace OSP.SPASv2.Web.APIServices.Repository
{
    public class ReportRepository
    {
        public async Task<ReportParams> GetReportListByPersonID(string URL, ReportParams ReportParams)
        {
            string requestAddress = URL + "/Jon/GetReportByPersonID";


            ReportParams = await UtilitiesHttpClient<ReportParams>.PostAsyncT<ReportParams>(ReportParams, requestAddress);
            return ReportParams;
        }
    }
}

