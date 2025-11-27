using OSP.Common.Domain.Tables;
using OSP.Common.Domain.View;
using OSP.SPASv2.Domain.Tables;
using OSP.SPASv2.Domain.View;

namespace OSP.SPASv2.Domain.Params
{
    public class DashboardParams
    {
        public TblResponse TblResponse { get; set; }
        public DateTime MaxDateTime { get; set; }
    }
}
