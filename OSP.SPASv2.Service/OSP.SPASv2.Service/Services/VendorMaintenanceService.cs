using OSP.SPASv2.Domain.View;
using System.Text;

namespace OSP.SPASv2.Service.Services
{
    public class VendorMaintenanceService
    {
        ServiceUnit _ServiceUnit = new ServiceUnit();

        public async Task<string> GenerateVendorCode(string LatestVendorCode)
        {
            string YearMonth = DateTime.Now.ToString("yyMM");
            string VendorCode = string.Empty;

            if (!string.IsNullOrEmpty(LatestVendorCode)) //meron data
            {
                int NewBatchNo = int.Parse(LatestVendorCode) + 1;
                VendorCode = YearMonth + NewBatchNo.ToString().PadLeft(6, '0');
            }
            else //wala
            {
                VendorCode = YearMonth + "000001";
            }

            return await Task.FromResult(VendorCode.ToUpper());
        }

    }
}
